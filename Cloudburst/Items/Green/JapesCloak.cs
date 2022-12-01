using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Green
{
    internal class JapesCloak
    {
        public static ItemDef japesCloakItem;
        public static BuffDef japesBuff;
        public static void Setup()
        {
            japesCloakItem = ScriptableObject.CreateInstance<ItemDef>();
            japesCloakItem.tier = ItemTier.Tier1;
            japesCloakItem.name = "itempickupbuff";
            japesCloakItem.nameToken = "ITEM_PICKUPBUFF_NAME";
            japesCloakItem.descriptionToken = "ITEM_PICKUPBUFF_DESCRIPTION";
            japesCloakItem.loreToken = "ITEM_PICKUPBUFF_LORE";
            japesCloakItem.requiredExpansion = Cloudburst.cloudburstExpansion;

            ContentAddition.AddItemDef(japesCloakItem);

            LanguageAPI.Add("ITEM_PICKUPBUFF_NAME", "Jape's Cloak");

            On.RoR2.GenericPickupController.AttemptGrant += GenericPickupController_AttemptGrant;

            japesBuff = ScriptableObject.CreateInstance<BuffDef>();
            japesBuff.canStack = true;

            ContentAddition.AddBuffDef(japesBuff);

            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
        }

        private static float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen)
        {
            int buffCount = self.body.GetBuffCount(japesBuff);
            if (buffCount > 0)
            {
                amount *= 1.3f * buffCount;
            }
            return orig(self, amount, procChainMask, nonRegen);
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(japesBuff))
            {
                args.armorAdd += 5;
            }
        }

        private static void GenericPickupController_AttemptGrant(On.RoR2.GenericPickupController.orig_AttemptGrant orig, GenericPickupController self, CharacterBody body)
        {
            orig(self, body);
            int itemCount = body.inventory.GetItemCount(japesCloakItem);
            if (itemCount > 0)
            {
                int buffCount = body.GetBuffCount(japesBuff);
                if (buffCount < itemCount * 2 + 1)
                {
                    body.AddBuff(japesBuff);
                }
            }
        }
    }
}
