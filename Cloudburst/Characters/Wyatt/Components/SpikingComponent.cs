using Cloudburst.Characters;
using Cloudburst.Characters.Wyatt;
using EntityStates.Merc;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Wyatt.Components
{
    public class SpikingComponent : MonoBehaviour
    {
        public CharacterBody spikerBody;
        private Vector3 direction;

        private CharacterMotor characterMotor;
        private RigidbodyMotor rigidMotor;
        private bool useRigidBody = false;
        public GameObject originalSpiker;
        private WyattNetworkCombat networkCombat;

        public float interval = 0;
        private float stopwatch;
        public void Start()
        {
            characterMotor = base.gameObject.GetComponent<CharacterMotor>();
            if (characterMotor == null && useRigidBody == false)
            {
                useRigidBody = true;
                rigidMotor = base.gameObject.GetComponent<RigidbodyMotor>();
                rigidMotor.moveVector = Vector3.zero;
            }
            spikerBody = originalSpiker.GetComponent<CharacterBody>();
            networkCombat = originalSpiker.GetComponent<WyattNetworkCombat>();
            direction = Vector3.down;
            if (characterMotor)
            {
                characterMotor.velocity = Vector3.zero;
                characterMotor.disableAirControlUntilCollision = true;

                characterMotor.onHitGroundServer += Motor_onHitGround;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.gameObject.layer == LayerIndex.world.intVal)
            {
                bigSlam(collision.contacts[0].point);
                Destroy(this);
            }
        }

        void Motor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            Vector3 position = hitGroundInfo.position;

            bigSlam(position);

            characterMotor.onHitGroundServer -= Motor_onHitGround;

            Destroy(this);
        }

        private void bigSlam(Vector3 position)
        {
            if (NetworkServer.active)
            {
                EffectManager.SpawnEffect(WyattEffects.tiredOfTheDingDingDing, new EffectData
                {
                    scale = 10,
                    rotation = Quaternion.identity,
                    origin = position,
                }, true);

                BlastAttack blastAttack = new BlastAttack
                {
                    position = position,
                    //baseForce = 3000,
                    attacker = originalSpiker,
                    inflictor = originalSpiker,
                    teamIndex = spikerBody.teamComponent.teamIndex,
                    baseDamage = spikerBody.damage * WyattConfig.SpikeDamage.Value, //3,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    //bonusForce = new Vector3(0, -3000, 0),
                    damageType = DamageType.Stun1s, //| DamageTypeCore.spiked,
                    crit = spikerBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    falloffModel = BlastAttack.FalloffModel.None,
                    //impactEffect = BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/PulverizedEffect").GetComponent<EffectIndex>(),
                    procCoefficient = 0,
                    radius = 10
                };
                //R2API.DamageAPI.AddModdedDamageType(blastAttack, WyattDamageTypes.antiGravDamage);
                blastAttack.Fire();

            }

            List<CharacterBody> hitBodies = HG.CollectionPool<CharacterBody, List<CharacterBody>>.RentCollection();
            CCUtilities.CharacterOverlapSphereAll(ref hitBodies, transform.position, 10, LayerIndex.CommonMasks.bullet);

            for (int i = 0; i < hitBodies.Count; i++)
            {
                CharacterBody characterBody = hitBodies[i];

                bool canHit = CCUtilities.ShouldKnockup(characterBody, spikerBody.teamComponent.teamIndex);
                if (canHit && characterBody != characterMotor.body && characterBody.gameObject != originalSpiker)
                {
                    networkCombat.ApplyKnockupAuthority(characterBody.gameObject, WyattConfig.SpikeImpactLiftForce.Value);// 10);
                    //CCUtilities.AddExplosionForce(characterBody.characterMotor, characterBody.characterMotor.mass * 25, transform.position, 25, 5, false);
                    //CCUtilities.AddUpwardForceToBody(characterBody.gameObject, 10);                    
                }
            }
            HG.CollectionPool<CharacterBody, List<CharacterBody>>.ReturnCollection(hitBodies);
        }

        public void OnDestroy()
        {
            if (characterMotor)
            {
                characterMotor.onHitGroundServer -= Motor_onHitGround;
            }
        }

        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;

            if (characterMotor == null && rigidMotor == null)
                Destroy(this);

            if (NetworkServer.active)
            {
                if (stopwatch >= (interval - 0.001f))
                {
                    if (useRigidBody == false)
                    {
                        characterMotor.ApplyForce((direction * 62.5f * Assaulter2.speedCoefficient), true, false);
                    }
                    else
                    {
                        //rigidMotor.rigid.AddForce((direction * 62.5f * Assaulter2.speedCoefficient), ForceMode.VelocityChange);
                    }
                }
                var wow = (direction * 2 * Assaulter2.speedCoefficient) * Time.fixedDeltaTime;
                if (useRigidBody == false)
                {
                    characterMotor.rootMotion += wow;
                }
                else
                {
                    rigidMotor.AddDisplacement(wow);
                }
            }

            if (stopwatch >= interval)
            {
                //		protected void PlayCrossfade(string layerName, string animationStateName, float crossfadeDuration)
                Destroy(this);
                //base.PlayCrossfade("Fullbody, Override", "BufferEmpty", 0.5f);
            }
        }
    }
}