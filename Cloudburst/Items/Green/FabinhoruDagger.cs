using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Green
{
    internal class FabinhoruDagger
    {
        public static ItemDef fabinhorusDaggerItem;
        public static BuffDef fabinhorusBuff;
        public static void Setup()
        {
            fabinhorusDaggerItem = ScriptableObject.CreateInstance<ItemDef>();
            fabinhorusDaggerItem.tier = ItemTier.Tier1;
            fabinhorusDaggerItem.name = "itembleedcripple";
            fabinhorusDaggerItem.nameToken = "ITEM_BLEEDCRIPPLE_NAME";
            fabinhorusDaggerItem.descriptionToken = "ITEM_BLEEDCRIPPLE_DESCRIPTION";
            fabinhorusDaggerItem.loreToken = "ITEM_BLEEDCRIPPLE_LORE";

            ContentAddition.AddItemDef(fabinhorusDaggerItem);

            fabinhorusBuff = ScriptableObject.CreateInstance<BuffDef>();
            fabinhorusBuff.canStack = true;
            fabinhorusBuff.isDebuff = true;
            fabinhorusBuff.buffColor = Color.red;
            fabinhorusDaggerItem.requiredExpansion = Cloudburst.cloudburstExpansion;

            ContentAddition.AddBuffDef(fabinhorusBuff);

            LanguageAPI.Add("ITEM_BLEEDCRIPPLE_NAME", "Fabinhoru's Dagger");

            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self.inventory)
            {
                if (self.inventory.GetItemCount(fabinhorusDaggerItem) > 0)
                {
                    self.bleedChance += 5;
                }
            }
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            int buffCount = sender.GetBuffCount(fabinhorusBuff);
            if (buffCount > 0)
            {
                args.armorAdd -= buffCount * 15 + 15;
            }
            
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            if (victimBody)
            {
                if (victimBody.HasBuff(RoR2Content.Buffs.Bleeding) || victimBody.HasBuff(RoR2Content.Buffs.SuperBleed))
                {
                    victimBody.AddTimedBuff(fabinhorusBuff, 5);
                }
            }
        }
    }
}
