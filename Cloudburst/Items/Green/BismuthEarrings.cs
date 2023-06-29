using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Green
{
    internal class BismuthEarrings
    {
        public static ItemDef bismuthEarringsItem;

        //public static Material
        public static void Setup()
        {
            bismuthEarringsItem = ScriptableObject.CreateInstance<ItemDef>();
            bismuthEarringsItem.tier = ItemTier.Tier2;
            bismuthEarringsItem.name = "itembarriercrit";
            bismuthEarringsItem.nameToken = "ITEM_BARRIERONCRIT_NAME";
            bismuthEarringsItem.descriptionToken = "ITEM_BARRIERONCRIT_DESCRIPTION";
            bismuthEarringsItem.loreToken = "ITEM_BARRIERONCRIT_LORE";
            bismuthEarringsItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            bismuthEarringsItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("IMDLBismuthRings");
            bismuthEarringsItem.pickupIconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texBismuthEarring");

            ContentAddition.AddItemDef(bismuthEarringsItem);

            LanguageAPI.Add("ITEM_BARRIERONCRIT_NAME", "Bismuth Earrings");

            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.inventory)
            {
                int itemCount = sender.inventory.GetItemCount(bismuthEarringsItem);
                if (itemCount > 0)
                {
                    args.critAdd += 5;
                }
            }

        }

        private static void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo damageInfo, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
            orig(self, body, damageInfo, master, procCoefficient, procChainMask);
            if (body.inventory)
            {
                int earringCount = body.inventory.GetItemCount(bismuthEarringsItem);
                if (earringCount > 0)
                {
                    body.healthComponent.AddBarrier(5 + (earringCount - 1) * 3);
                }
            }
        }
    }
}
