using Cloudburst.Wyatt.Components;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattAssets
    {
        public static GameObject wyattMaidBubble;
        private static GameObject winchGhost;
        public static GameObject winch;

        public static void InitAss()
        {
            CreateProjectiles();
        }

        private static void CreateProjectiles()
        {
            wyattMaidBubble = CreateWyattMaidProjectile();
            //CreateWinchGhost();
            //CreateWinchProjectile();
        }

        private static GameObject CreateWyattMaidProjectile()
        {
            GameObject prefab = Addressables.LoadAssetAsync<GameObject>("9ca7d392fa3bb444b827d475b36b9253").WaitForCompletion().InstantiateClone("MAIDProjectile");
            //GameObject hurtbox = new GameObject("TempHurtbox");

            Modules.Assets.ConvertAllRenderersToHopooShader(prefab);

            /*TeamComponent comp = prefab.AddComponent<TeamComponent>();
            SkillLocator empt = prefab.AddComponent<SkillLocator>();
            CharacterBody body = prefab.AddComponent<CharacterBody>();

            HealthComponent health = prefab.AddComponent<HealthComponent>();

           // body.name = prefabName;
            body.bodyFlags = CharacterBody.BodyFlags.Masterless;
            body.rootMotionInMainState = false;
            body.mainRootSpeed = 0;
            body.bodyIndex = -1;
            //body.aimOriginTransform = aimOrigin.transform;
            body.hullClassification = HullClassification.Human;

            hurtbox.transform.SetParent(prefab.transform);*/
            GameObject awful = new GameObject("Awful");
            awful.transform.SetParent(prefab.transform);

            awful.layer = LayerIndex.entityPrecise.intVal;

            var orb = prefab.AddComponent<ProjectileProximityBeamController>();
            orb.attackFireCount = 2;
            orb.attackInterval = 1;
            orb.listClearInterval = 0.1f;
            orb.attackRange = 25;
            orb.minAngleFilter = 0;
            orb.maxAngleFilter = 180;
            orb.procCoefficient = 1f;
            orb.damageCoefficient = 0.5f;
            orb.bounces = 0;
            orb.inheritDamageType = false;
            orb.lightningType = RoR2.Orbs.LightningOrb.LightningType.Ukulele;
            orb.enabled = false;

            var goost = Modules.Assets.LoadAsset<GameObject>("WyattMaidBoomerangProjectile");
            MaterialSwapper.RunSwappers(goost);

            goost.AddComponent<ProjectileGhostController>();

            prefab.GetComponent<ProjectileController>().ghostPrefab = goost;
            prefab.GetComponent<ProjectileController>().procCoefficient = 1;
            prefab.GetComponent<ProjectileDotZone>().fireFrequency = 0.5f; ;
            BoomerangProjectile boomerangProjectile = prefab.GetComponent<BoomerangProjectile>();
            boomerangProjectile.distanceMultiplier = 0.2f;
            boomerangProjectile.canHitWorld = false;
            prefab.GetComponent<ProjectileOverlapAttack>().damageCoefficient = 1;
            prefab.AddComponent<MAIDProjectileController>();
            awful.AddComponent<SphereCollider>().radius = 3;
            awful.GetComponent<SphereCollider>().isTrigger = true;
            prefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            GameObject maidCleanEffect = WyattEffects.maidCleanseEffect;

            Modules.Assets.AddNewEffectDef(maidCleanEffect);

            prefab.GetComponent<ProjectileDotZone>().impactEffect = maidCleanEffect;

            return prefab;
        }
    }
}