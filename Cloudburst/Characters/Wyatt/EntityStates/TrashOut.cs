using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using Cloudburst.Wyatt.Components;
using Cloudburst.GlobalComponents;

namespace Cloudburst.CEntityStates.Wyatt
{
    //https://i.redd.it/w8n9ovbtr0p51.jpg
    public class TrashOut : BaseSkillState
    {
        enum ActionStage
        {
            StartUp,
            NoTarget,
            FoundTarget,
            HitTarget
        }

        private ActionStage stage;
        private GenericCursorEnemyTracker tracker;
        private HurtBox target;

        private float _stopwatch;

        private GameObject _winch = null;

        public override void OnEnter()
        {
            base.OnEnter();
            tracker = base.gameObject.GetComponent<GenericCursorEnemyTracker>();
            _winch = null;
            if (characterBody)
            {
                characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            }
            if (base.isAuthority)
            {
                stage = ActionStage.StartUp;
                target = tracker.GetTrackingTarget();

                if (!isGrounded)
                {
                    base.SmallHop(base.characterMotor, 10f);
                    //we good
                }

                if (target && target.healthComponent && target.healthComponent.alive)
                {
                    stage = ActionStage.FoundTarget;
                    /*_winch = null;
                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        crit = false,
                        damage = 0,
                        damageColorIndex = DamageColorIndex.Default,
                        force = 0f,
                        owner = base.gameObject,
                        position = target.transform.position,
                        procChainMask = default(ProcChainMask),
                        projectilePrefab = ProjectileCore.winch,
                        rotation = Util.QuaternionSafeLookRotation(GetAimRay().direction),
                        target = target.gameObject,
                        useSpeedOverride = true,
                        speedOverride = 500
                    };
                    EffectManager.SimpleMuzzleFlash(BandaidConvert.Resources.Load<GameObject>("prefabs/effects/muzzleflashes/MuzzleflashWinch"), base.gameObject, "WinchHole", true);*/
                    //ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                    base.PlayAnimation("Fullbody, Override", "kick");
                }

                //Log.Info("Stage: " + stage.ToString());
            }
        }

        public void SetHookReference(GameObject winch)
        {
            //Log.Info("got winch!");
            _winch = winch;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Log.Info("Stage: " + stage.ToString());
            _stopwatch += Time.deltaTime;

            if (stage == ActionStage.FoundTarget)
            {

                if (base.isAuthority)
                {
                    if (this.target)
                    {
                        Vector3 velocity = (target.transform.position - base.transform.position).normalized * 120f;
                        base.characterMotor.velocity = velocity;
                        base.characterDirection.forward = base.characterMotor.velocity.normalized;
                        float distance = Util.SphereVolumeToRadius(target.volume);

                        if (Vector3.Distance(base.transform.position, target.transform.position) < distance + 5f && target)
                        {
                            base.PlayAnimation("Fullbody, Override", "kickSwing");
                        }

                        if (_stopwatch > 2)
                        {
                            //Log.Info(stopwatch);
                            this.activatorSkillSlot.AddOneStock();
                            Object.Destroy(_winch);
                            characterMotor.velocity = Vector3.zero;
                            //CCUtilities.LogW("Can't reach target, skill refunded!");
                            this.outer.SetNextStateToMain();
                        }


                        if (Vector3.Distance(base.transform.position, target.transform.position) < distance + 5f && target)
                        {
                            //base.PlayAnimation("Fullbody, Override", "kickSwing");


                            new BlastAttack
                            {
                                position = target.transform.position,
                                baseForce = 3000,
                                attacker = base.gameObject,
                                inflictor = base.gameObject,
                                teamIndex = base.GetTeam(),
                                baseDamage = 3,//(3 + (characterBody.GetBuffCount(Custodian.instance.wyattCombatDef) * .25f)) * this.damageStat,
                                attackerFiltering = AttackerFiltering.NeverHitSelf,
                                // bonusForce = new Vector3(0, -3000, 0),
                                damageType = DamageType.Stun1s,// | DamageTypeCore.spiked,
                                crit = RollCrit(),
                                damageColorIndex = DamageColorIndex.Default,
                                falloffModel = BlastAttack.FalloffModel.None,
                                //impactEffect = BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/PulverizedEffect").GetComponent<EffectIndex>(),
                                procCoefficient = 1f,
                                radius = 5
                            }.Fire();

                            if (target.healthComponent.body && !target.healthComponent.body.isChampion)
                            {
                                Log.Info($"Check1: {NetworkServer.active}"); //
                                if ((target.healthComponent.GetComponent<CharacterMotor>() && !target.healthComponent.body.characterMotor.isGrounded) && !target.healthComponent.GetComponent<SpikingComponent>())
                                {
                                    Log.Info($"Check2: {NetworkServer.active}");

                                    SpikingComponent based = target.healthComponent.gameObject.AddComponent<SpikingComponent>();
                                    based.interval = 1f;
                                    based.originalSpiker = this.gameObject;
                                }

                                else if (target.healthComponent.GetComponent<RigidbodyMotor>() && !target.healthComponent.GetComponent<SpikingComponent>())
                                {
                                    Log.Info($"Check3: {NetworkServer.active}");
                                    SpikingComponent based = target.healthComponent.gameObject.AddComponent<SpikingComponent>();
                                    based.interval = 1.5f;
                                    based.originalSpiker = this.gameObject;
                                }
                            }

                            //Log.Info("called onhit!!!");
                            Object.Destroy(_winch);

                            EffectData effectData = new EffectData
                            {
                                rotation = Quaternion.identity,
                                scale = 20f,
                                //start = base.transform.position,
                                origin = target.transform.position
                            };

                            //hmm, today, i will stream :]
                          //  EffectManager.SpawnEffect(BandaidConvert.Resources.Load<GameObject>("prefabs/effects/MaulingRockImpact"), effectData, true);
                         //   EffectManager.SpawnEffect(BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionSolarFlare"), effectData, true);

                            base.characterMotor.velocity = Vector3.up * 25f;
                            //characterMotor.ApplyForce(-(GetAimRay().direction * (-characterMotor.mass * 10)), true, false);
                            stage = ActionStage.HitTarget;


                            //Log.Info("Stage: " + stage.ToString());

                            this.outer.SetNextStateToMain();
                        }
                    }
                    else
                    {
                        Object.Destroy(_winch);
                        characterMotor.velocity = Vector3.zero; ;
                        outer.SetNextStateToMain();
                        return;
                    }
                }
            }
            else
            {
                //CCUtilities.LogE("Something is seriously fucked. Stage: " + stage.ToString());

                characterMotor.velocity = Vector3.zero;
                this.outer.SetNextStateToMain();

                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Object.Destroy(_winch);
            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
