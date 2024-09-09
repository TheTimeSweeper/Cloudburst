using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Gray
{
    internal class GlassHarvester
    {
        public static ItemDef glassHarvesterItem;
        private static ItemDef glassHarvesterConsumedItem;
        public static void Setup()
        {
            glassHarvesterItem = ScriptableObject.CreateInstance<ItemDef>();
            glassHarvesterItem.tier = ItemTier.Tier1;
            glassHarvesterItem.deprecatedTier = ItemTier.Tier1;
            (glassHarvesterItem as ScriptableObject).name = "GlassHarvester";
            glassHarvesterItem.AutoPopulateTokens();
            glassHarvesterItem.pickupIconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texGlassHarvester");
            glassHarvesterItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("IMDLHarvester");
            glassHarvesterItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            glassHarvesterItem.tags = new ItemTag[]
            {
                ItemTag.Damage,
                ItemTag.LowHealth,
            }
            ;
            glassHarvesterConsumedItem = ScriptableObject.CreateInstance<ItemDef>();
            glassHarvesterConsumedItem.tier = ItemTier.NoTier;
            glassHarvesterConsumedItem.deprecatedTier = ItemTier.NoTier;
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

            Modules.Language.Add("ITEM_GLASSHARVESTER_NAME", "Glass Harvester");
            Modules.Language.Add("ITEM_GLASSHARVESTER_PICKUP", "Your 'Critical Strikes' deal an additional 40% damage. Breaks at low health, granting experience.");
            Modules.Language.Add("ITEM_GLASSHARVESTER_DESC", "Gain <style=cIsDamage>5% critical strike chance</style>. <style=cIsDamage>Critical Strikes</style> deal an additional <style=cIsDamage>40% damage</style><style=cStack>(+30% per stack)</style>. Taking damage to below <style=cIsHealth>25% health</style> <style=cIsUtility>breaks</style> this item and grants 15% of your current level's <style=cIsUtility>experience</style>.");
            Modules.Language.Add("ITEM_GLASSHARVESTER_LORE", "Does it harvest glass or does it harvest with glass?\nI don't know and I don't care get out of my house"); ;
            Modules.Language.Add("ITEM_ITEMEXPONHITCONSUMED_NAME", "Glass Harvester (Broken)");
            Modules.Language.Add("ITEM_ITEMEXPONHITCONSUMED_PICKUP", "Harvest Reaped. Was it worth the experience?");
            Modules.Language.Add("ITEM_ITEMEXPONHITCONSUMED_DESC", "A spent item with no remaining power.");

            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            IL.RoR2.HealthComponent.UpdateLastHitTime += HealthComponent_UpdateLastHitTime;
        }

        private static void Test(HealthComponent self)
        {
            if (self == null) return;
            if (self.body == null) return;
            if (self.body.master == null) return;
            if (self.body.inventory == null) return;

            int itemCount = self.body.inventory.GetItemCount(glassHarvesterItem);

            if (itemCount > 0 && self.isHealthLow)
            {
                self.body.inventory.RemoveItem(glassHarvesterItem, itemCount);
                self.body.inventory.GiveItem(glassHarvesterConsumedItem, itemCount); 

                CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, glassHarvesterItem.itemIndex, glassHarvesterConsumedItem.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
            }

            // apply experience one by one so new levels get proper amount
            while (itemCount > 0 && self.isHealthLow)
            {
                itemCount--;
                if (self.body.teamComponent)
                {
                    TeamIndex teamIndex = self.body.teamComponent.teamIndex;
                    ulong expNeed = TeamManager.instance.GetTeamNextLevelExperience(teamIndex) - TeamManager.instance.GetTeamCurrentLevelExperience(teamIndex);
                    expNeed = (ulong)(expNeed * 0.15f);
                    TeamManager.instance.GiveTeamExperience(teamIndex, expNeed);
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
                ilCursor.EmitDelegate<Action<HealthComponent>>((healthComponent) => { Test(healthComponent); });
                //Log.Warning ("TESSST");
            }
            else
            {
                //Log.Warning("SKI BODY");
            }
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if(sender && sender.inventory)
            {
                int itemCount = sender.inventory.GetItemCount(glassHarvesterItem);
                args.critAdd += itemCount > 0 ? 5 : 0;
                args.critDamageMultAdd += itemCount > 0 ? itemCount * 0.30f + 0.10f : 0;
            }
        }
    }
}