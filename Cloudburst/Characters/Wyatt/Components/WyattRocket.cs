using Cloudburst.Characters;
using Cloudburst.Characters.Wyatt;
using EntityStates.Merc;
using EntityStates.Toolbot;
using RoR2;
using RoR2.Orbs;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Wyatt.Components
{
    public class WyattRocket : MonoBehaviour
    {
        private CharacterBody characterBody;
        private Vector3 direction;
        private CharacterMotor motor;
        private CharacterDirection characterDirection;
        private OverlapAttack attack = new OverlapAttack();

        public float interval = 0;
        private float stopwatch;
        private bool hit = false;

        private List<HurtBox> victimsStruck = new List<HurtBox>();

        private float hitStopwatch = 0;
        private TeamComponent teamComponent;

        public void Start()
        {
            characterBody = GetComponent<CharacterBody>();
            direction = characterBody.inputBank.aimDirection;
            motor = characterBody.characterMotor;
            characterDirection = GetComponent<CharacterDirection>();
            teamComponent = GetComponent<TeamComponent>();

            attack = new OverlapAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                damage = 8 * characterBody.damage,
                damageColorIndex = DamageColorIndex.Default,
                //forceVector = new Vector3(0, force, 0),
                //hitEffectPrefab = hitEffectPrefab,
                impactSound = RoR2.Audio.NetworkSoundEventIndex.Invalid,
                inflictor = base.gameObject,
                isCrit = characterBody.RollCrit(),
                maximumOverlapTargets = 100,
                procChainMask = default,
                procCoefficient = 1,
                //pushAwayForce = 3500,
                forceVector = direction * 3000,
                hitBoxGroup = CCUtilities.FindHitBoxGroup("TempHitboxLunge", characterBody.modelLocator.modelTransform),
                teamIndex = characterBody.teamComponent.teamIndex
            };
            //R2API.DamageAPI.AddModdedDamageType(attack, DamageTypeCore.antiGrav);

            //motor.onHitGroundServer += Motor_onHitGround;

            //just explode and that's it
            //component over. move this to entitystate cause it's probably not networked?
            BigExplode(transform.position);

        }

        public void OnDestroy()
        {
            //motor.onHitGroundServer -= Motor_onHitGround;
        }

        private void SetToEmpty()
        {
            //var thing = base.GetComponent<ModelLocator>().modelTransform.GetComponent<Animator>();
            //thing.speed = 1f;
            //thing.Update(0f);
            //int layerIndex = thing.GetLayerIndex("FullBody, Override");
            //thing.CrossFadeInFixedTime("BuffEmpty", 0.5f, layerIndex);

            Animator animator = base.GetComponent<ModelLocator>().modelTransform.GetComponent<Animator>();
            //EntityStates.EntityState.PlayAnimationOnAnimator(animator, "FullBody, Override", "kickSwing");
            animator.Play("kickSwing");
        }

        private void Motor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            Vector3 position = hitGroundInfo.position;

            BigExplode(position);
        }

        private void BigExplode(Vector3 position)
        {
            SetToEmpty();

            EffectManager.SpawnEffect(WyattEffects.bigZapEffectPrefabArea,// Effects.wyattSlam, 
                                      new EffectData
                                      {
                                          scale = 30,
                                          rotation = Quaternion.identity,
                                          origin = position,
                                      },
                                      true);

            /*EffectManager.SpawnEffect(BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact")/*Effects.wyattSlam/*BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleGuardGroundSlam"), new EffectData
            {
                scale = 1,
                rotation = Quaternion.identity,
                origin = hitGroundInfo.position,
            }, true);
            EffectManager.SpawnEffect(BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleGuardGroundSlam")/*Effects.wyattSlam/*BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleGuardGroundSlam"), new EffectData
            {
                scale = 1,
                rotation = Quaternion.identity,
                origin = hitGroundInfo.position,
            }, true);*/

            BlastAttack blast = new BlastAttack
            {
                position = position,
                //baseForce = 3000,
                attacker = base.gameObject,
                inflictor = gameObject,
                teamIndex = characterBody.teamComponent.teamIndex,
                baseDamage = characterBody.damage * 8,
                attackerFiltering = default,
                //bonusForce = new Vector3(0, -3000, 0),
                damageType = DamageType.Stun1s, //| DamageTypeCore.spiked,
                crit = characterBody.RollCrit(),
                damageColorIndex = DamageColorIndex.WeakPoint,
                falloffModel = BlastAttack.FalloffModel.None,
                //impactEffect = BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/PulverizedEffect").GetComponent<EffectIndex>(),
                procCoefficient = 0,
                radius = 30
            };
            R2API.DamageAPI.AddModdedDamageType(blast, WyattDamageTypes.antiGravDamage);


            Collider[] sphere = Physics.OverlapSphere(transform.position, 30);
            foreach (Collider body in sphere)
            {
                var cb = body.gameObject.GetComponentInParent<CharacterBody>();
                if (cb)
                {
                    //if (cb.isChampion)
                    //    continue;
                    //if (cb.baseNameToken == "BROTHER_BODY_NAME")
                    //    continue;
                    //fuck it. float all
                    if (cb.teamComponent.teamIndex == teamComponent.teamIndex)
                        continue;
                    if (cb.characterMotor && cb != characterBody)
                    {
                        AddExplosionForce(cb.characterMotor, cb.characterMotor.mass * 25, transform.position, 25, 5, false);
                    }
                }
            }

            Destroy(this);
        }

        //todo ugly transfer
        public static void AddExplosionForce(CharacterMotor body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier = 0, bool useWearoff = false) {
            var dir = (body.transform.position - explosionPosition);

            Vector3 baseForce = Vector3.zero;

            if (useWearoff) {
                float wearoff = 1 - (dir.magnitude / explosionRadius);
                baseForce = dir.normalized * explosionForce * wearoff;
            } else {
                baseForce = dir.normalized * explosionForce;
            }
            //baseForce.z = 0;
            //body.ApplyForce(baseForce);

            //if (upliftModifier != 0)
            //{
            float upliftWearoff = 1 - upliftModifier / explosionRadius;
            Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
            //upliftForce.z = 0;
            body.ApplyForce(upliftForce);
            //}

        }

        public void FixedUpdate()
        {
            hitStopwatch += Time.fixedDeltaTime;
            stopwatch += Time.fixedDeltaTime;
            if (NetworkServer.active)
            {
                if (stopwatch >= (interval - 0.001f) && hit == false)
                {
                    hit = true;
                    motor.ApplyForce((direction * 125 * Assaulter2.speedCoefficient), true, false);
                    characterDirection.forward = motor.rootMotion.normalized;
                }

                if (stopwatch >= interval)
                {
                    SetToEmpty();
                    //		protected void PlayCrossfade(string layerName, string animationStateName, float crossfadeDuration)
                    Destroy(this);
                    //base.PlayCrossfade("Fullbody, Override", "BufferEmpty", 0.5f);
                }

                var wow = (direction * 3 * Assaulter2.speedCoefficient) * Time.fixedDeltaTime;
                motor.rootMotion += wow;
                characterDirection.forward = motor.rootMotion.normalized;

                if (attack.Fire(victimsStruck))
                {
                    motor.Motor.ForceUnground();
                    characterBody.healthComponent.TakeDamageForce(direction * -5000, true, false);
                    EffectManager.SpawnEffect(WyattEffects.ericAndreMoment, new EffectData
                    {
                        scale = 2,
                        rotation = Quaternion.identity,
                        origin = victimsStruck[0].transform.position,
                    }, true);
                    //motor.ApplyForce(-(direction * 125 * Assaulter2.speedCoefficient), true, false);
                    SetToEmpty();
                    //		protected void PlayCrossfade(string layerName, string animationStateName, float crossfadeDuration)
                    Destroy(this);
                }
            }
        }
    }
}
