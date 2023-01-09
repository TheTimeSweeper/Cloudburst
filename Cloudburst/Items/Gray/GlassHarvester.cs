using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Gray
{
    internal class GlassHarvester
    {
        public static ItemDef glassHarvesterItem;
        public static void Setup()
        {
            glassHarvesterItem = ScriptableObject.CreateInstance<ItemDef>();
            glassHarvesterItem.tier = ItemTier.Tier1;
            glassHarvesterItem.name = "itemexponhit";
            glassHarvesterItem.nameToken = "ITEM_EXPONHIT_NAME";
            glassHarvesterItem.pickupToken = "ITEM_EXPONHIT_PICKUP";
            glassHarvesterItem.descriptionToken = "ITEM_EXPONHIT_DESCRIPTION";
            glassHarvesterItem.loreToken = "ITEM_EXPONHIT_LORE";
            glassHarvesterItem.pickupIconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texGlassHarvester");
            glassHarvesterItem.pickupModelPrefab = Cloudburst.CloudburstAssets.LoadAsset<GameObject>("GlassHarvesterPickup");
            glassHarvesterItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            
            ContentAddition.AddItemDef(glassHarvesterItem);

            LanguageAPI.Add("ITEM_EXPONHIT_NAME", "Glass Harvester");
            LanguageAPI.Add("ITEM_EXPONHIT_PICKUP", "Gain experience on hit.");
            LanguageAPI.Add("ITEM_EXPONHIT_DESCRIPTION", "Gain 3 <style=cStack>(+2 per stack)</style> points of <style=cIsUtility>experience</style> on hit.");

            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;

        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);
            if (damageInfo.attacker)
            {
                CharacterBody body = damageInfo.attacker.GetComponent<CharacterBody>();
                if (body && body.inventory)
                {
                    int harvesterCount = body.inventory.GetItemCount(glassHarvesterItem);
                    if (harvesterCount > 0)
                    {
                        int expAmount = 3 + (harvesterCount - 1) * 2;
                        TeamManager.instance.GiveTeamExperience(TeamComponent.GetObjectTeam(damageInfo.attacker), (uint)(expAmount * Run.instance.compensatedDifficultyCoefficient));
                    }
                }
            }
        }

    }
}
