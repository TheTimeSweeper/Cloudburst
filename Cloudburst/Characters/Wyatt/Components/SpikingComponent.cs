using Cloudburst.Characters;
using Cloudburst.Characters.Wyatt;
using EntityStates.Merc;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Wyatt.Components
{
    public class SpikingComponent : MonoBehaviour
    {
        public enum MotorType
        {
            NONE,
            CHARACTERMOTOR,
            RIGIDBODYMOTOR,
            RIGIDBODY
        }

        public CharacterBody spikerBody;
        public GameObject originalSpiker;
        public float interval = 0;
        private Vector3 direction;

        private CharacterMotor characterMotor;
        private RigidbodyMotor rigidMotor;
        private Rigidbody rigidBody;
        private MonoBehaviour disabledMotor;

        private WyattNetworkCombat networkCombat;

        private MotorType motorType = MotorType.NONE;
        private float stopwatch;
        private float initialPositionY;
        private float currentSpeed;
        private float speedGrowth;
        private bool hitGround;

        public void Start()
        {
            spikerBody = originalSpiker.GetComponent<CharacterBody>();
            networkCombat = originalSpiker.GetComponent<WyattNetworkCombat>();
            direction = Vector3.down;
            currentSpeed = WyattConfig.SpikeInitialSpeed.Value;
            speedGrowth = WyattConfig.SpikeSpeedGrowth.Value;

            initialPositionY = transform.position.y;

            CheckMotor();
        }

        private void CheckMotor()
        {
            characterMotor = base.gameObject.GetComponent<CharacterMotor>();
            if (characterMotor != null)
            {
                motorType = MotorType.CHARACTERMOTOR;
                characterMotor.velocity = Vector3.zero;
                characterMotor.disableAirControlUntilCollision = true;

                characterMotor.onHitGroundAuthority += Motor_onHitGround;
                return;
            }

            rigidMotor = base.gameObject.GetComponent<RigidbodyMotor>();
            if (rigidMotor != null)
            {
                motorType = MotorType.RIGIDBODYMOTOR;
                rigidMotor.moveVector = Vector3.zero;
                return;
            }

            rigidBody = base.gameObject.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                motorType = MotorType.RIGIDBODY;

                disabledMotor = FindOtherMotor();
                if(disabledMotor != null)
                {
                    disabledMotor.enabled = false;
                }

                rigidBody.velocity = Vector3.zero;
                return;
            }
            //no spikable motors found
            Destroy(this);
        }

        private MonoBehaviour FindOtherMotor()
        {
            if(TryGetComponent(out HoverVehicleMotor hoverMotor))
            {
                return hoverMotor;
            }
            if (TryGetComponent(out WheelVehicleMotor wheelMotor))
            {
                return wheelMotor;
            }
            if (TryGetComponent(out RailMotor railMotor))
            {
                return railMotor;
            }
            return null;
        }

        public void OnDestroy()
        {
            if (characterMotor)
            {
                characterMotor.onHitGroundAuthority -= Motor_onHitGround;
            }
            if(disabledMotor != null)
            {
                disabledMotor.enabled = true;
            }
        }

        void OnCollisionStay(Collision collision)
        {
            if(collision.collider.gameObject.layer == LayerIndex.world.intVal && !hitGround)
            {
                Vector3 point = collision.contacts[0].point;
                RigidBodyHitGround(point);
            }
        }

        private void RigidBodyHitGround(Vector3 point)
        {
            hitGround = true;
            bigSlam(point);
            Destroy(this);
        }

        void Motor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            Vector3 position = hitGroundInfo.position;
            
            bigSlam(position);

            characterMotor.onHitGroundAuthority -= Motor_onHitGround;

            Destroy(this);
        }

        private void bigSlam(Vector3 position)
        {
            float blastRadius = 10;
            if (NetworkServer.active)
            {
                float amountFell = initialPositionY - transform.position.y;
                if (amountFell < 1)
                    return;
                float blastDamage = WyattConfig.SpikeDamage.Value + amountFell * WyattConfig.SpikeDamagePerMeterFell.Value;
                //Log.Warning("amountFell: " + amountFell + "damage: " + blastDamage);

                EffectManager.SpawnEffect(WyattEffects.tiredOfTheDingDingDing, new EffectData
                {
                    scale = blastRadius,
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
                    baseDamage = spikerBody.damage * blastDamage, //3,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    //bonusForce = new Vector3(0, -3000, 0),
                    damageType = DamageType.Stun1s, //| DamageTypeCore.spiked,
                    crit = spikerBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    falloffModel = BlastAttack.FalloffModel.SweetSpot,
                    //impactEffect = BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/PulverizedEffect").GetComponent<EffectIndex>(),
                    procCoefficient = 1,
                    radius = blastRadius
                };
                //R2API.DamageAPI.AddModdedDamageType(blastAttack, WyattDamageTypes.antiGravDamage);
                blastAttack.Fire();
            }

            Util.PlaySound("Play_grandParent_attack1_boulderLarge_impact",gameObject);

            List<CharacterBody> hitBodies = HG.CollectionPool<CharacterBody, List<CharacterBody>>.RentCollection();
            CCUtilities.CharacterOverlapSphereAll(ref hitBodies, transform.position, blastRadius, LayerIndex.CommonMasks.bullet);

            for (int i = 0; i < hitBodies.Count; i++)
            {
                CharacterBody characterBody = hitBodies[i];

                bool canHit = CCUtilities.ShouldKnockup(characterBody, spikerBody.teamComponent.teamIndex);
                if (canHit && characterBody.gameObject != gameObject && characterBody.gameObject != originalSpiker)
                {
                    networkCombat.ApplyKnockupAuthority(characterBody.gameObject, WyattConfig.SpikeImpactLiftForce.Value);// 10);
                    //CCUtilities.AddExplosionForce(characterBody.characterMotor, characterBody.characterMotor.mass * 25, transform.position, 25, 5, false);
                    //CCUtilities.AddUpwardForceToBody(characterBody.gameObject, 10);                    
                }
            }
            HG.CollectionPool<CharacterBody, List<CharacterBody>>.ReturnCollection(hitBodies);
        }

        public void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;

            //if (characterMotor == null && rigidMotor == null)
            //    Destroy(this);

            if (NetworkServer.active)
            {
                currentSpeed += speedGrowth * Time.fixedDeltaTime;

                if (!hitGround)
                {
                    Vector3 wow = (direction * 2 * currentSpeed) * Time.fixedDeltaTime;
                    HandleMotorMove(wow);
                }
            }

            if (stopwatch >= interval)
            {
                //		protected void PlayCrossfade(string layerName, string animationStateName, float crossfadeDuration)
                Destroy(this);
                //base.PlayCrossfade("Fullbody, Override", "BufferEmpty", 0.5f);
            }
        }

        private void HandleMotorMove(Vector3 wow)
        {
            if(motorType == MotorType.CHARACTERMOTOR)
            {
                characterMotor.rootMotion += wow;
                return; 
            }
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, wow, out hitInfo, wow.magnitude * 2, LayerIndex.world.mask))
            {
                RigidBodyHitGround(hitInfo.point);
                return;
            }

            if (motorType == MotorType.RIGIDBODYMOTOR)
            {
                rigidMotor.AddDisplacement(wow);
            }

            if (motorType == MotorType.RIGIDBODY)
            {
                rigidBody.MovePosition(transform.position + wow);
            }
        }
    }
}