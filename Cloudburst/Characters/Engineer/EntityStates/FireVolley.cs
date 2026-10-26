using EntityStates;
using EntityStates.Engi.EngiWeapon;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Characters.Engineer.EntityStates
{
    //Impact Grenades. uses vanilla FireGrenades' values so it stays in line with engi's primary
    public class FireVolley : BaseSkillState
    {
        public static int grenadeCountMax = 1;
        public static float fireDuration = 2f;

        private Ray projectileRay;
        private Transform modelTransform;
        private float duration;
        private float fireTimer;
        private int grenadeCount;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = FireGrenades.baseDuration / attackSpeedStat;
            modelTransform = GetModelTransform();
            StartAimMode(duration, false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            fireTimer -= Time.fixedDeltaTime;
            if (fireTimer <= 0f && grenadeCount < grenadeCountMax)
            {
                fireTimer += fireDuration / attackSpeedStat / grenadeCountMax;

                FireGrenade("MuzzleLeft");
                PlayCrossfade("Gesture Left Cannon, Additive", "FireGrenadeLeft", 0.1f);
                FireGrenade("MuzzleRight");
                PlayCrossfade("Gesture Right Cannon, Additive", "FireGrenadeRight", 0.1f);

                grenadeCount++;
            }

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        private void FireGrenade(string targetMuzzle)
        {
            Util.PlaySound(FireGrenades.attackSoundString, gameObject);
            projectileRay = GetAimRay();
            if (modelTransform)
            {
                ChildLocator childLocator = modelTransform.GetComponent<ChildLocator>();
                if (childLocator)
                {
                    Transform muzzle = childLocator.FindChild(targetMuzzle);
                    if (muzzle)
                    {
                        projectileRay.origin = muzzle.position;
                    }
                }
            }

            AddRecoil(-1f * FireGrenades.recoilAmplitude, -2f * FireGrenades.recoilAmplitude, -1f * FireGrenades.recoilAmplitude, 1f * FireGrenades.recoilAmplitude);

            if (FireGrenades.effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(FireGrenades.effectPrefab, gameObject, targetMuzzle, false);
            }

            if (isAuthority)
            {
                float x = Random.Range(0f, characterBody.spreadBloomAngle);
                float z = Random.Range(0f, 360f);
                Vector3 up = Vector3.up;
                Vector3 axis = Vector3.Cross(up, projectileRay.direction);
                Vector3 spread = Quaternion.Euler(0f, 0f, z) * (Quaternion.Euler(x, 0f, 0f) * Vector3.forward);
                float y = spread.y;
                spread.y = 0f;
                float angle = Mathf.Atan2(spread.z, spread.x) * Mathf.Rad2Deg - 90f;
                float angle2 = Mathf.Atan2(y, spread.magnitude) * Mathf.Rad2Deg + FireGrenades.arcAngle;
                Vector3 forward = Quaternion.AngleAxis(angle, up) * (Quaternion.AngleAxis(angle2, axis) * projectileRay.direction);

                ProjectileManager.instance.FireProjectile(
                    EngineerContent.impactGrenadeProjectile,
                    projectileRay.origin,
                    Util.QuaternionSafeLookRotation(forward),
                    gameObject,
                    damageStat * FireGrenades.damageCoefficient,
                    0f,
                    RollCrit(),
                    DamageColorIndex.Default,
                    null,
                    -1f);
            }

            characterBody.AddSpreadBloom(FireGrenades.spreadBloomValue);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
