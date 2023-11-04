using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Gray
{
    internal class GlassHarvesterOld
    {
        public static ItemDef glassHarvesterItem;
        private static ItemDef glassHarvesterConsumedItem;
        public static void Setup()
        {
            glassHarvesterItem = ScriptableObject.CreateInstance<ItemDef>();
            glassHarvesterItem.tier = ItemTier.Tier1;
            (glassHarvesterItem as ScriptableObject).name = "GlassHarvester";
            glassHarvesterItem.AutoPopulateTokens();
            glassHarvesterItem.pickupIconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texGlassHarvester");
            glassHarvesterItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("IMDLHarvester");
            glassHarvesterItem.requiredExpansion = Cloudburst.cloudburstExpansion;

            glassHarvesterConsumedItem = ScriptableObject.CreateInstance<ItemDef>();
            (glassHarvesterConsumedItem as ScriptableObject).name = "GlassHarvesterConsumed";

            glassHarvesterConsumedItem.tier = ItemTier.NoTier;
            glassHarvesterConsumedItem.name = "itemexponhitconsumed";
            glassHarvesterConsumedItem.AutoPopulateTokens();
            glassHarvesterConsumedItem.pickupIconSprite = Cloudburst.WyattAssetBundle.LoadAsset<Sprite>("iconGlassHarvesterBroken");
            glassHarvesterConsumedItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            glassHarvesterConsumedItem.tags = new ItemTag[]
            {
                ItemTag.CannotCopy,
                ItemTag.CannotSteal,
                ItemTag.CannotDuplicate,
            };
            ContentAddition.AddItemDef(glassHarvesterItem);
            ContentAddition.AddItemDef(glassHarvesterConsumedItem);

            Modules.Language.Add("ITEM_EXPONHIT_NAME", "Glass Harvester");
            Modules.Language.Add("ITEM_EXPONHIT_PICKUP", "Gain experience on hit.");
            Modules.Language.Add("ITEM_EXPONHIT_DESCRIPTION", "Gain 3 <style=cStack>(+2 per stack)</style> points of <style=cIsUtility>experience</style> on hit.");
            Modules.Language.Add("ITEM_EXPONHIT_LORE", "Does it harvest glass or does it harvest with glass?\nI don't know and I don't care get out of my house"); ;
            Modules.Language.Add("ITEM_EXPONHITCONSUMED_NAME", "Glass Harvester");
            Modules.Language.Add("ITEM_EXPONHITCONSUMED_PICKUP", "Gain experience on hit.");

            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            IL.RoR2.HealthComponent.UpdateLastHitTime += HealthComponent_UpdateLastHitTime;
        }

        private static void Test(HealthComponent self)
        {
            int itemCount = self.body.inventory.GetItemCount(glassHarvesterItem);

            if (itemCount > 0 && self.isHealthLow)
            {
                self.body.inventory.RemoveItem(glassHarvesterItem, itemCount);
                self.body.inventory.GiveItem(glassHarvesterConsumedItem, itemCount);
                if (self.body.teamComponent)
                {
                    TeamIndex teamIndex = self.body.teamComponent.teamIndex;
                    ulong expNeed = TeamManager.instance.GetTeamNextLevelExperience(teamIndex) - TeamManager.instance.GetTeamCurrentLevelExperience(teamIndex);
                    expNeed = (ulong)(expNeed * 0.15f);
                    TeamManager.instance.GiveTeamExperience(teamIndex, expNeed);
                    CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, glassHarvesterItem.itemIndex, glassHarvesterConsumedItem.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                }
            }
        }

        private static void HealthComponent_UpdateLastHitTime(MonoMod.Cil.ILContext il)
        {
            ILCursor ilCursor = new ILCursor(il);
            if (ilCursor.TryGotoNext(MoveType.Before,
                inst => inst.MatchBrfalse(out _)))
            {
                ilCursor.Index += 3;
                ilCursor.Emit(OpCodes.Ldarg_0);
                ilCursor.EmitDelegate(Test);
                Debug.Log("TESSST");
            }
            else
            {
                Debug.Log("SKI BODY");
            }
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if(sender && sender.inventory)
            {
                int itemCount = sender.inventory.GetItemCount(glassHarvesterItem);
                args.critAdd += itemCount > 0 ? 5 : 0;
                args.critDamageMultAdd += itemCount > 0 ? itemCount * 30 + 10 : 0;
            }
        }
    }
}