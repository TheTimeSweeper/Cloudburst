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
            japesCloakItem.deprecatedTier = ItemTier.Tier2;
            japesCloakItem.name = "itempickupbuff";
            japesCloakItem.nameToken = "ITEM_PICKUPBUFF_NAME";
            japesCloakItem.descriptionToken = "ITEM_PICKUPBUFF_DESCRIPTION";
            japesCloakItem.pickupToken = "ITEM_PICKUPBUFF_PICKUP";
            japesCloakItem.loreToken = "ITEM_PICKUPBUFF_LORE";
            japesCloakItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            japesCloakItem.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Assets/Cloudburst/Items/Cloak/JapeIcon.png");
            japesCloakItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("IMDLCloak");
            japesCloakItem.tags = new ItemTag[]
            {
                ItemTag.InteractableRelated,
                ItemTag.Utility
            };
                        
            ContentAddition.AddItemDef(japesCloakItem);

            Modules.Language.Add("ITEM_PICKUPBUFF_NAME", "Jape's Cloak");
            Modules.Language.Add("ITEM_PICKUPBUFF_PICKUP", "Gain a buff that grants armor and healing on item pickup.");
            Modules.Language.Add("ITEM_PICKUPBUFF_DESCRIPTION", "Gain a buff that grants you <style=cIsUtility>+5 armor</style> and <style=cIsHealing>30% healing</style> when picking up an item. Maximum cap of 3 buffs <style=cStack>(+2 per stack)</style>.");
            Modules.Language.Add("ITEM_PICKUPBUFF_LORE", @"""Quartermaster’s log. 17 days after the crash.

It has been more than half a month since we’ve nearly burnt ourselves into smoldering black paste during our unwilling introduction to the atmosphere of this murder-planet. Due to the nature of our arrival, that being escape from a collapsing cargo ship that had been ripped out of warp travel, our supply of necessities has been dwindling from an already dangerously low base count. We have been forced to ration what little food and water we have. To keep in check of this, I have been elected as Quartermaster of our outpost. 
Our supplies are as follows:

-Enough food and water for around 1 week and a half.
-3 and a half cardboard boxes full of salvaged metal and circuitry from destroyed drones and sentries.
-4 bags of medical equipment 
-2 keychains, each with 13 rusted keys? (This must be an error, Juarez says it was brought in alongside the rest of the haul that those three blokes brought in 2 days ago, are they printing these or something?)
-6 boxes full of ammunition (potentially dwindling at an exponential rate)

Something isn’t lining up. The bulletin board we put up to track how much supplies everyone’s been taking isn’t covering all of it. While this may be just simple miscommunication on everyone’s part, the chance of somebody taking more than what they need is steadily increasing with each day. I don’t have any evidence to point towards this exactly, but it is a (very frightening) possibility. 


If it’s just one person taking it all, then at the rates that have been shown... 
They just might be more prepared than the rest of us.

End log.


Addendum: WHO. IN GOD ABOVE’S HOLY NAME. TOOK. MY. CIGARETTES??”""");

            On.RoR2.GenericPickupController.AttemptGrant += GenericPickupController_AttemptGrant;

            japesBuff = ScriptableObject.CreateInstance<BuffDef>();
            japesBuff.canStack = true;
            japesBuff.buffColor = new Color(1f, 0.7882353f, 0.05490196f);
            japesBuff.iconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texBuffJapesCloak");

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
                    body.AddTimedBuff(japesBuff, 10);
                }
            }
        }
    }
}
