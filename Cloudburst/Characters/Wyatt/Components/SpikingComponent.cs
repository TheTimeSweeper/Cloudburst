using Cloudburst.Characters;
using Cloudburst.Characters.Wyatt;
using EntityStates.Merc;
using RoR2;
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
            Log.Warning("collided with " + LayerMask.LayerToName(collision.collider.gameObject.layer));
            Log.Warning("moveVector " + rigidMotor.moveVector);
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

                new BlastAttack
                {
                    position = position,
                    //baseForce = 3000,
                    attacker = originalSpiker,
                    inflictor = originalSpiker,
                    teamIndex = spikerBody.teamComponent.teamIndex,
                    baseDamage = spikerBody.damage * 5,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    //bonusForce = new Vector3(0, -3000, 0),
                    damageType = DamageType.Stun1s, //| DamageTypeCore.spiked,
                    crit = spikerBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    falloffModel = BlastAttack.FalloffModel.None,
                    //impactEffect = BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/PulverizedEffect").GetComponent<EffectIndex>(),
                    procCoefficient = 0,
                    radius = 15
                }.Fire();

            }

            Collider[] sphereColliders = Physics.OverlapSphere(transform.position, 10);
            foreach (Collider body in sphereColliders)
            {
                CharacterBody characterBody = body.gameObject.GetComponentInParent<CharacterBody>();
                if (characterBody)
                {
                    bool cannotHit = false;
                    if (characterBody.isChampion)
                    {
                        cannotHit = true;
                    }
                    if (characterBody.baseNameToken == "BROTHER_BODY_NAME")
                    {
                        cannotHit = false;
                    }
                    if (characterBody.characterMotor && characterBody != characterMotor.body && cannotHit == false && !(characterBody.gameObject == originalSpiker))
                    {
                        CCUtilities.AddExplosionForce(characterBody.characterMotor, characterBody.characterMotor.mass * 25, transform.position, 25, 5, false);
                    }
                }
            }
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

                if (stopwatch >= interval)
                {
                    //		protected void PlayCrossfade(string layerName, string animationStateName, float crossfadeDuration)
                    Destroy(this);
                    //base.PlayCrossfade("Fullbody, Override", "BufferEmpty", 0.5f);
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
        }
    }
}