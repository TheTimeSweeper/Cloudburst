using Cloudburst.Cores;
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
            }
            spikerBody = originalSpiker.GetComponent<CharacterBody>();
            direction = Vector3.down;
            if (characterMotor)
            {
                characterMotor.disableAirControlUntilCollision = true;

                characterMotor.onHitGroundServer += Motor_onHitGround;
            }
        }

        void Motor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {

            EffectManager.SpawnEffect(Effects.tiredOfTheDingDingDing, new EffectData
            {
                scale = 10,
                rotation = Quaternion.identity,
                origin = hitGroundInfo.position,
            }, true);

            new BlastAttack
            {
                position = hitGroundInfo.position,
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

            var sphere = Physics.OverlapSphere(transform.position, 10);
            foreach (var body in sphere)
            {
                var cb = body.gameObject.GetComponentInParent<CharacterBody>();
                if (cb)
                {
                    bool cannotHit = false;
                    if (cb.isChampion)
                    {
                        cannotHit = true;
                    }
                    if (cb.baseNameToken == "BROTHER_BODY_NAME")
                    {
                        cannotHit = false;
                    }
                    if (cb.characterMotor && cb != characterMotor.body && cannotHit == false && !(cb.gameObject == originalSpiker))
                    {
                        CCUtilities.AddExplosionForce(cb.characterMotor, cb.characterMotor.mass * 25, transform.position, 25, 5, false);
                    }
                }
            }

            characterMotor.onHitGroundServer -= Motor_onHitGround;

            Destroy(this);
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
                    rigidMotor.rootMotion += wow;
                }
            }
        }
    }
}