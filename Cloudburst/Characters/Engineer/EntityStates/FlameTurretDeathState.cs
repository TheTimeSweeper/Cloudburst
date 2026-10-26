using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Characters.Engineer.EntityStates
{
    //flame turrets blow up when they die
    public class FlameTurretDeathState : GenericCharacterDeath
    {
        public static float healthDamageCoefficient = 3f;
        public static float radiusPerHealth = 0.2f;
        public static float minRadius = 8f;
        public static float maxRadius = 30f;

        private static GameObject initialExplosion;
        private static GameObject deathExplosion;

        private float deathDuration;

        public override bool shouldAutoDestroy => false;

        public override void OnEnter()
        {
            if (!initialExplosion)
            {
                initialExplosion = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ExplosionEngiTurret");
                deathExplosion = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFX");
            }
            base.OnEnter();
        }

        public override void PlayDeathAnimation(float crossfadeDuration = 0.1f)
        {
            Animator modelAnimator = GetModelAnimator();
            if (modelAnimator)
            {
                int layerIndex = modelAnimator.GetLayerIndex("Body");
                modelAnimator.PlayInFixedTime("Death", layerIndex);
                modelAnimator.Update(0f);
                deathDuration = modelAnimator.GetCurrentAnimatorStateInfo(layerIndex).length;
                if (initialExplosion)
                {
                    Object.Instantiate(initialExplosion, transform.position, transform.rotation, transform);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge > deathDuration && NetworkServer.active)
            {
                //radius used to scale with health uncapped, which got silly with transcendence/infusion
                float radius = Mathf.Clamp(healthComponent.fullCombinedHealth * radiusPerHealth, minRadius, maxRadius);

                if (deathExplosion)
                {
                    EffectManager.SpawnEffect(deathExplosion, new EffectData
                    {
                        origin = transform.position,
                        scale = radius
                    }, true);
                }

                new BlastAttack
                {
                    attacker = gameObject,
                    inflictor = gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = healthComponent.fullCombinedHealth * healthDamageCoefficient,
                    baseForce = 0,
                    crit = RollCrit(),
                    damageColorIndex = DamageColorIndex.WeakPoint,
                    damageType = DamageType.IgniteOnHit,
                    falloffModel = BlastAttack.FalloffModel.None,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = transform.position,
                    procChainMask = default,
                    procCoefficient = 1,
                    radius = radius,
                    teamIndex = GetTeam()
                }.Fire();

                Destroy(gameObject);
            }
        }

        public override void OnExit()
        {
            DestroyModel();
            base.OnExit();
        }
    }
}
