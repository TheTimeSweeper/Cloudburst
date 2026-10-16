using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Items.Red
{
    internal class Extractor
    {
        public static ItemDef extractorItem;

        public static float BaseChance = 15;
        public static float StackingChance = 5;
        public static float BarrierPerProjectile = 5;
        public static float ProjectileRadius = 10;

        public static GameObject dropEffect;

        public static void Setup()
        {
            extractorItem = ScriptableObject.CreateInstance<ItemDef>();
            extractorItem.deprecatedTier = ItemTier.Tier3;
            extractorItem.name = "itemextractor";
            extractorItem.nameToken = "ITEM_EXTRACTOR_NAME";
            extractorItem.pickupToken = "ITEM_EXTRACTOR_PICKUP";
            extractorItem.descriptionToken = "ITEM_EXTRACTOR_DESCRIPTION";
            extractorItem.loreToken = "ITEM_EXTRACTOR_LORE";
            extractorItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            extractorItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("Assets/Cloudburst/Items/Grinder/IMDLGrinder.prefab");
            extractorItem.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Assets/Cloudburst/Items/Grinder/icon.png");
            extractorItem.tags = new ItemTag[]
            {
                ItemTag.CanBeTemporary,
                ItemTag.Utility,
                ItemTag.OnKillEffect,
                ItemTag.AIBlacklist
            };

            //no item displays in the original
            ItemAPI.Add(new CustomItem(extractorItem, new ItemDisplayRuleDict()));

            Modules.Language.Add("ITEM_EXTRACTOR_NAME", "Extractor");
            Modules.Language.Add("ITEM_EXTRACTOR_PICKUP", "Bosses have a chance to drop a boss item on death. Nearby projectiles are also destroyed, gain barrier for each destroyed projectile.");
            Modules.Language.Add("ITEM_EXTRACTOR_DESCRIPTION", "Killing a boss has a <style=cIsUtility>" + (BaseChance + StackingChance) + "%</style> <style=cStack>(+" + StackingChance + "% per stack)</style> chance to drop a <style=cIsUtility>random boss item</style>. Enemy projectiles within <style=cIsUtility>" + ProjectileRadius + "m</style> of the boss are destroyed, granting a <style=cIsHealing>temporary barrier</style> for <style=cIsHealing>" + BarrierPerProjectile + " health</style> <style=cStack>(+" + BarrierPerProjectile + " per stack)</style> for each one.");
            Modules.Language.Add("ITEM_EXTRACTOR_LORE", "");

            dropEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/PodGroundImpact");

            GlobalEventManager.onDeathProcServer += GlobalEventManager_onDeathProcServer;
        }

        private static void GlobalEventManager_onDeathProcServer(DamageReport damageReport)
        {
            if (!NetworkServer.active || !damageReport.victimIsBoss)
                return;

            CharacterBody attackerBody = damageReport.attackerBody;
            CharacterBody victimBody = damageReport.victimBody;
            if (!attackerBody || !victimBody || !attackerBody.inventory)
                return;

            int itemCount = attackerBody.inventory.GetItemCountEffective(extractorItem);
            if (itemCount <= 0)
                return;

            Vector3 position = victimBody.corePosition;

            Util.PlaySound("ui_obj_casinoChest_open", attackerBody.gameObject);

            if (Run.instance && Run.instance.availableBossDropList.Count > 0
                && Util.CheckRoll(BaseChance + StackingChance * itemCount, damageReport.attackerMaster))
            {
                List<PickupIndex> bossDrops = Run.instance.availableBossDropList;
                PickupIndex pickupIndex = bossDrops[Run.instance.bossRewardRng.RangeInt(0, bossDrops.Count)];

                if (dropEffect)
                {
                    EffectManager.SpawnEffect(dropEffect, new EffectData
                    {
                        origin = position,
                        scale = 15f
                    }, true);
                }

                PickupDropletController.CreatePickupDroplet(new UniquePickup(pickupIndex), position, new Vector3(0f, 25f, 0f));
            }

            HashSet<ProjectileController> destroyedProjectiles = new HashSet<ProjectileController>();
            Collider[] colliders = Physics.OverlapSphere(position, ProjectileRadius, LayerIndex.projectile.mask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < colliders.Length; i++)
            {
                ProjectileController projectile = colliders[i].GetComponentInParent<ProjectileController>();
                if (!projectile || destroyedProjectiles.Contains(projectile))
                    continue;

                if (projectile.teamFilter && projectile.teamFilter.teamIndex == attackerBody.teamComponent.teamIndex)
                    continue;

                destroyedProjectiles.Add(projectile);
                UnityEngine.Object.Destroy(projectile.gameObject);
            }

            if (destroyedProjectiles.Count > 0)
            {
                attackerBody.healthComponent.AddBarrier(BarrierPerProjectile * itemCount * destroyedProjectiles.Count);
            }
        }
    }
}
