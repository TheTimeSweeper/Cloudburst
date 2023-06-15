using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Green
{
    internal class EnigmaticKeycard
    {
        public static ItemDef enigmaticKeycardItem;
        public static GameObject NullifierSpawnEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierSpawnEffect.prefab").WaitForCompletion();
        public static GameObject RedAffixMisileProjectile = Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/EliteFire/RedAffixMissileProjectile.prefab").WaitForCompletion();
        public static GameObject ParentSlamEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Parent/ParentSlamEffect.prefab").WaitForCompletion();
        public static GameObject NullifierExplosion = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierExplosion.prefab").WaitForCompletion();

        public static GameObject orbitalOrbProjectile;
        public static GameObject orbitalImpactEffect;
        public static void Setup()
        {
            enigmaticKeycardItem = ScriptableObject.CreateInstance<ItemDef>();
            enigmaticKeycardItem.deprecatedTier = ItemTier.Tier2;
            enigmaticKeycardItem.name = "itemenigmatickeycard";
            enigmaticKeycardItem.nameToken = "ITEM_ENIGMATICKEYCARD_NAME";
            enigmaticKeycardItem.pickupToken = "ITEM_ENIGMATICKEYCARD_PICKUP";
            enigmaticKeycardItem.descriptionToken = "ITEM_ENIGMATICKEYCARD_DESCRIPTION";
            enigmaticKeycardItem.loreToken = "ITEM_ENIGMATICKEYCARD_LORE";
            enigmaticKeycardItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            enigmaticKeycardItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("IMDLPricard.prefab");
            //enigmaticKeycardItem.pickupIconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texBismuthEarring");

            ContentAddition.AddItemDef(enigmaticKeycardItem);

            LanguageAPI.Add("ITEM_ENIGMATICKEYCARD_NAME", "Enigmatic Keycard");
            //LanguageAPI.Add("ITEM_ENIGMATICKEYCARD_PICKUP", "Critical strikes grant temporary barrier");
            //LanguageAPI.Add("ITEM_ENIGMATICKEYCARD_DESCRIPTION", "On Critical Strike, grant a small amount of temporary barrier.");

            CreateProjectile();

            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }


        public static void CreateProjectile()
        {
            CreateImpact();

            orbitalOrbProjectile = RedAffixMisileProjectile.InstantiateClone("EnigmaticOrb", true);
            orbitalOrbProjectile.GetComponent<BoxCollider>().isTrigger = false;

            ProjectileProximityBeamController orb = orbitalOrbProjectile.AddComponent<ProjectileProximityBeamController>();
            ProjectileImpactExplosion explosion = orbitalOrbProjectile.AddComponent<ProjectileImpactExplosion>();
            ProjectileSingleTargetImpact impact = orbitalOrbProjectile.GetComponent<ProjectileSingleTargetImpact>();
            MissileController missile = orbitalOrbProjectile.GetComponent<MissileController>();

            missile.giveupTimer = float.MaxValue;
            missile.maxSeekDistance = float.MaxValue;
            missile.deathTimer = float.MaxValue;

            impact.impactEffect = orbitalImpactEffect;

            explosion.impactEffect = NullifierExplosion;
            explosion.offsetForLifetimeExpiredSound = 0;
            explosion.destroyOnEnemy = true;
            explosion.destroyOnWorld = true;
            explosion.timerAfterImpact = false;
            explosion.falloffModel = BlastAttack.FalloffModel.None;
            explosion.lifetime = 15;
            explosion.lifetimeAfterImpact = 0;
            explosion.lifetimeRandomOffset = 0;
            explosion.blastRadius = 8;
            explosion.blastDamageCoefficient = 5;
            explosion.blastProcCoefficient = 0;
            explosion.blastAttackerFiltering = AttackerFiltering.Default;
            explosion.childrenCount = 0;
            explosion.transformSpace = ProjectileImpactExplosion.TransformSpace.World;

            orb.attackFireCount = 1;
            orb.attackInterval = 1;
            orb.listClearInterval = 0.1f;
            orb.attackRange = 50;
            orb.minAngleFilter = 0;
            orb.maxAngleFilter = 180;
            orb.procCoefficient = 0.3f;
            orb.damageCoefficient = 1;
            orb.bounces = 0;
            orb.inheritDamageType = false;
            orb.lightningType = RoR2.Orbs.LightningOrb.LightningType.Tesla;

            ContentAddition.AddProjectile(orbitalOrbProjectile);
        }

        private static void CreateImpact()
        {
            orbitalImpactEffect = ParentSlamEffect.InstantiateClone("OrbitalImpactEffect", false);

            var particleParent = orbitalImpactEffect.transform.Find("Particles");
            var debris = particleParent.Find("Debris, 3D");
            var debris2 = particleParent.Find("Debris");
            var sphere = particleParent.Find("Nova Sphere");

            debris.gameObject.SetActive(false);
            debris2.gameObject.SetActive(false);
            sphere.gameObject.SetActive(false);

            ContentAddition.AddEffect(orbitalImpactEffect);
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);
            if(damageInfo.attacker && victim)
            {
                CharacterBody body = damageInfo.attacker.GetComponent<CharacterBody>();
                CharacterBody victimBody = victim.GetComponent<CharacterBody>();
                if (victimBody && body && body.inventory)
                {
                    CharacterMaster master = body.master;

                    int itemCount = body.inventory.GetItemCount(enigmaticKeycardItem);
                    if (itemCount > 0 && victim && Util.CheckRoll(4 * damageInfo.procCoefficient, master))
                    {
                        
                        if(victimBody.mainHurtBox)
                        {
                            float radius = 15f;
                            var originPoint = victimBody.mainHurtBox.transform.position + 
                                new Vector3(
                                    UnityEngine.Random.Range(-radius, radius), 
                                    UnityEngine.Random.Range(-radius, radius), 
                                    UnityEngine.Random.Range(-radius, radius));

                            EffectData data = new EffectData()
                            {
                                rotation = Quaternion.Euler(victimBody.transform.forward),
                                scale = 1,
                                origin = originPoint,
                            };

                            EffectManager.SpawnEffect(NullifierSpawnEffect, data, true);
                            FireProjectileInfo _info = new FireProjectileInfo()
                            {
                                crit = false,
                                damage = body.damage * (1.4f + itemCount),
                                damageColorIndex = RoR2.DamageColorIndex.Default,
                                damageTypeOverride = DamageType.Generic,
                                force = 0,
                                owner = body.gameObject,
                                position = originPoint,
                                procChainMask = default,
                                projectilePrefab = orbitalOrbProjectile,
                                rotation = Util.QuaternionSafeLookRotation(victimBody.transform.position),
                                target = victim,
                                useFuseOverride = false,
                                useSpeedOverride = true,
                                _speedOverride = 100
                            };
                            ProjectileManager.instance.FireProjectile(_info);
                        }
                    }
                }
            }
        }
    }
}
