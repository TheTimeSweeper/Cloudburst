using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using Cloudburst.Wyatt.Components;
using Cloudburst.GlobalComponents;
using Cloudburst.Characters.Wyatt;

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
            base.gameObject.layer = LayerIndex.fakeActor.intVal;
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

                        if (_stopwatch > 2)
                        {
                            this.activatorSkillSlot.AddOneStock();
                            this.outer.SetNextStateToMain();
                        }

                        if (Vector3.Distance(base.transform.position, target.transform.position) < distance + 5f && target)
                        {
                            stage = ActionStage.HitTarget;

                            //moving to entity state
                            //ApplyBlastAuthority();

                            //if (target.healthComponent.body /*&& !target.healthComponent.body.isChampion*/)
                            //{
                            //    if ((target.healthComponent.GetComponent<CharacterMotor>() && !target.healthComponent.body.characterMotor.isGrounded))
                            //    {
                            //        GetComponent<WyattNetworkCombat>().ApplyBasedAuthority(target.healthComponent.gameObject, gameObject, 1);
                            //    }

                            //    else if (target.healthComponent.GetComponent<RigidbodyMotor>())
                            //    {
                            //        GetComponent<WyattNetworkCombat>().ApplyBasedAuthority(target.healthComponent.gameObject, gameObject, 1.5f);
                            //    }
                            //}

                            this.outer.SetNextState(new TrashOutHit());
                        }
                    }
                    else
                    {
                        outer.SetNextStateToMain();
                        return;
                    }
                }
            }
            else
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        //moving to entity state
        private void ApplyBlastAuthority()
        {
            new BlastAttack
            {
                position = target.transform.position,
                baseForce = 0,
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = base.GetTeam(),
                baseDamage = damageStat * WyattConfig.M2Damage.Value,//(3 + (characterBody.GetBuffCount(Custodian.instance.wyattCombatDef) * .25f)) * this.damageStat,
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
        }

        public override void OnExit()
        {
            base.OnExit();
            Object.Destroy(_winch);
            characterMotor.velocity = Vector3.zero;
            characterMotor.Motor.ForceUnground();

            if (stage != ActionStage.HitTarget)
            {
                base.gameObject.layer = LayerIndex.defaultLayer.intVal;
                PlayAnimation("FullBody, Override", "BufferEmpty");
            }
            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
