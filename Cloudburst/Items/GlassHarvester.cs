using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items
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
            glassHarvesterItem.descriptionToken = "ITEM_EXPONHIT_DESCRIPTION";
            glassHarvesterItem.loreToken = "ITEM_EXPONHIT_LORE";

            ContentAddition.AddItemDef(glassHarvesterItem);

            LanguageAPI.Add("ITEM_EXPONHIT_NAME", "Glass Harvester");
            //LanguageAPI.Add("ITEM_EXPONHIT_DESCRIPTION", "Gain 3d ");

            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (damageInfo.attacker)
            {
                CharacterBody body = damageInfo.attacker.GetComponent<CharacterBody>();
                if (body && body.inventory)
                {
                    int harvesterCount = body.inventory.GetItemCount(glassHarvesterItem);
                    if (harvesterCount > 0)
                    {
                        int expAmount = 3 + (harvesterCount * 2);
                        TeamManager.instance.GiveTeamExperience(TeamComponent.GetObjectTeam(damageInfo.attacker), (uint)expAmount);
                    }
                }
            }
        }

    }
}
