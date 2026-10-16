using EntityStates;
using EntityStates.EngiTurret.EngiTurretWeapon;
using EntityStates.Mage.Weapon;
using RoR2;
using UnityEngine;

namespace Cloudburst.Characters.Engineer.EntityStates
{
    //flame turret primary. copies the walker turret beam's values, and swaps the laser out for artificer's flamethrower
    public class FireFlameThrower : BaseState
    {
        public static float damageCoefficient = 1f;
        public static float bonusFireFrequency = 0.4f;
        public static float bonusMaxDistance = 5f;

        private static bool initialized;
        private static GameObject flamethrowerEffectPrefab;
        private static float baseFireFrequency;
        private static float procCoefficient;
        private static float force;
        private static float minSpread;
        private static float maxSpread;
        private static float maxDistance;
        private static string muzzleString;
        private static string attackSoundString;

        private float fireFrequency;
        private float fireTimer;
        private Transform muzzleTransform;
        private GameObject flamethrowerEffectInstance;

        private static void InitializeValues()
        {
            if (initialized)
                return;
            initialized = true;

            if (EntityStateCatalog.InstantiateState(typeof(FireBeam)) is FireBeam beamState)
            {
                baseFireFrequency = beamState.fireFrequency;
                procCoefficient = beamState.procCoefficient;
                force = beamState.force;
                minSpread = beamState.minSpread;
                maxSpread = beamState.maxSpread;
                maxDistance = beamState.maxDistance;
                muzzleString = beamState.muzzleString;
                attackSoundString = beamState.attackSoundString;
            }

            if (EntityStateCatalog.InstantiateState(typeof(Flamethrower)) is Flamethrower flamethrowerState)
            {
                flamethrowerEffectPrefab = flamethrowerState.flamethrowerEffectPrefab;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            InitializeValues();

            //was a += on the cached state in the original, which kept stacking every time the state was made
            fireFrequency = baseFireFrequency + bonusFireFrequency;

            Util.PlaySound(attackSoundString, gameObject);

            Transform modelTransform = GetModelTransform();
            if (modelTransform)
            {
                ChildLocator childLocator = modelTransform.GetComponent<ChildLocator>();
                if (childLocator)
                {
                    muzzleTransform = childLocator.FindChild(muzzleString);
                }
            }

            SpawnFlamethrowerEffect();
        }

        private void SpawnFlamethrowerEffect()
        {
            if (muzzleTransform && flamethrowerEffectPrefab)
            {
                flamethrowerEffectInstance = Object.Instantiate(flamethrowerEffectPrefab, muzzleTransform.position, muzzleTransform.rotation, muzzleTransform);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //artificer's flamethrower effect times itself out, keep it alive for as long as we're firing
            if (!flamethrowerEffectInstance)
            {
                SpawnFlamethrowerEffect();
            }

            fireTimer += Time.fixedDeltaTime;
            if (fireTimer > 1f / (fireFrequency * attackSpeedStat))
            {
                FireBullet(GetAimRay());
                fireTimer = 0f;
            }

            if (isAuthority && !(inputBank && inputBank.skill1.down))
            {
                outer.SetNextStateToMain();
            }
        }

        private void FireBullet(Ray aimRay)
        {
            if (Flamethrower.impactEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(Flamethrower.impactEffectPrefab, gameObject, muzzleString, false);
            }

            if (isAuthority)
            {
                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = minSpread,
                    maxSpread = maxSpread,
                    bulletCount = 1u,
                    damage = damageCoefficient * damageStat / fireFrequency,
                    procCoefficient = procCoefficient / fireFrequency,
                    force = force,
                    muzzleName = muzzleString,
                    hitEffectPrefab = Flamethrower.impactEffectPrefab,
                    isCrit = RollCrit(),
                    HitEffectNormal = false,
                    radius = 0f,
                    maxDistance = maxDistance + bonusMaxDistance,
                    damageType = DamageType.IgniteOnHit
                }.Fire();
            }
        }

        public override void OnExit()
        {
            if (flamethrowerEffectInstance)
            {
                Destroy(flamethrowerEffectInstance);
            }
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
