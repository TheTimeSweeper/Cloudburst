using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using static Cloudburst.Modules.ItemDisplays;

namespace Cloudburst.Items.Red
{
    internal class Lemdog
    {
        public static ItemDef lemdogItem;

        public static float BaseConvertChance = 25;
        public static float StackingConvertChance = 2.5f;
        public static float BaseDurationBonus = 2;
        public static float StackingDurationBonus = 1;

        private static List<BuffDef> _convertedBuffs = null;
        public static List<BuffDef> convertedBuffs
        {
            get
            {
                if (_convertedBuffs == null)
                {
                    _convertedBuffs = new List<BuffDef>
                    {
                        RoR2Content.Buffs.Warbanner,
                        RoR2Content.Buffs.CloakSpeed,
                        //was MeatRegenBoost, which the game no longer uses
                        RoR2Content.Buffs.CrocoRegen,
                        RoR2Content.Buffs.WhipBoost,
                        RoR2Content.Buffs.TeamWarCry
                    };
                }
                return _convertedBuffs;
            }
        }

        private static List<BuffDef> _durationBlacklist = null;
        public static List<BuffDef> durationBlacklist
        {
            get
            {
                if (_durationBlacklist == null)
                {
                    _durationBlacklist = new List<BuffDef>
                    {
                        RoR2Content.Buffs.MedkitHeal,
                        RoR2Content.Buffs.HiddenInvincibility,
                        RoR2Content.Buffs.ElementalRingsCooldown,
                        RoR2Content.Buffs.EngiShield
                    };
                }
                return _durationBlacklist;
            }
        }

        public static void Setup()
        {
            lemdogItem = ScriptableObject.CreateInstance<ItemDef>();
            lemdogItem.deprecatedTier = ItemTier.Tier3;
            lemdogItem.name = "itemlemdog";
            lemdogItem.nameToken = "ITEM_LEMDOG_NAME";
            lemdogItem.pickupToken = "ITEM_LEMDOG_PICKUP";
            lemdogItem.descriptionToken = "ITEM_LEMDOG_DESCRIPTION";
            lemdogItem.loreToken = "ITEM_LEMDOG_LORE";
            lemdogItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            lemdogItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("Assets/Cloudburst/Items/Lemdog/IMDLLemDog.prefab");
            lemdogItem.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Assets/Cloudburst/Items/Lemdog/LemDog_TexIcon.png");
            lemdogItem.tags = new ItemTag[]
            {
                ItemTag.CanBeTemporary,
                ItemTag.Utility
            };

            ItemAPI.Add(new CustomItem(lemdogItem, CreateItemDisplayRules()));

            Modules.Language.Add("ITEM_LEMDOG_NAME", "Lemdog");
            Modules.Language.Add("ITEM_LEMDOG_PICKUP", "Chance for debuffs to become buffs when applied. Also extend the duration of buffs.");
            Modules.Language.Add("ITEM_LEMDOG_DESCRIPTION", "<style=cIsUtility>" + (BaseConvertChance + StackingConvertChance) + "%</style> <style=cStack>(+" + StackingConvertChance + "% per stack)</style> chance for <style=cIsUtility>applied debuffs to become beneficial buffs</style>. Also extend <style=cIsUtility>positive buff duration</style> by <style=cIsUtility>" + (BaseDurationBonus) + "</style> <style=cStack>(+" + StackingDurationBonus + " per stack)</style> seconds.");
            Modules.Language.Add("ITEM_LEMDOG_LORE", "");

            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
        }

        private static void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration)
        {
            if (self && self.inventory && buffDef)
            {
                int itemCount = self.inventory.GetItemCountEffective(lemdogItem);
                if (itemCount > 0)
                {
                    if (buffDef.isDebuff && !buffDef.isCooldown && Util.CheckRoll(BaseConvertChance + StackingConvertChance * itemCount, self.master))
                    {
                        buffDef = convertedBuffs[UnityEngine.Random.Range(0, convertedBuffs.Count)];
                    }

                    if (!buffDef.isDebuff && !buffDef.isCooldown && !buffDef.isHidden && !durationBlacklist.Contains(buffDef))
                    {
                        duration += BaseDurationBonus + StackingDurationBonus * (itemCount - 1);
                    }
                }
            }

            orig(self, buffDef, duration);
        }

        private static ItemDisplayRuleDict CreateItemDisplayRules()
        {
            GameObject display = CreateItemDisplayPrefab(lemdogItem.pickupModelPrefab, "DisplayLemdog");

            Vector3 headPos = new Vector3(0f, 0.4f, 0f);
            Vector3 headAngles = new Vector3(0f, 90f, 0f);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", CreateDisplayRuleArray(display, "Head", headPos, headAngles, Vector3.one));
            rules.Add("mdlHuntress", CreateDisplayRuleArray(display, "Head", headPos, headAngles, Vector3.one));
            rules.Add("mdlBandit2", CreateDisplayRuleArray(display, "Head", headPos, headAngles, Vector3.one));
            rules.Add("mdlToolbot", CreateDisplayRuleArray(display, "HeadCenter", new Vector3(0.08f, 0.899f, 1.84f), new Vector3(0f, -90f, 0f), new Vector3(5f, 5f, 5f)));
            rules.Add("mdlEngi", CreateDisplayRuleArray(display, "HeadCenter", new Vector3(0f, 0.3f, 0f), headAngles, Vector3.one));
            rules.Add("mdlMage", CreateDisplayRuleArray(display, "Head", headPos, headAngles, Vector3.one));
            rules.Add("mdlMerc", CreateDisplayRuleArray(display, "Head", headPos, headAngles, Vector3.one));
            rules.Add("mdlTreebot", CreateDisplayRuleArray(display, "FlowerBase", new Vector3(0f, 2f, 0f), new Vector3(90f, 90f, 180f), new Vector3(2f, 2f, 2f)));
            rules.Add("mdlLoader", CreateDisplayRuleArray(display, "Head", headPos, headAngles, Vector3.one));
            rules.Add("mdlCroco", CreateDisplayRuleArray(display, "Head", new Vector3(0.48f, 5f, -0.01f), new Vector3(15.79083f, 90f, 106.9842f), new Vector3(5f, 5f, 5f)));
            rules.Add("mdlCaptain", CreateDisplayRuleArray(display, "Head", headPos, headAngles, Vector3.one));
            return rules;
        }
    }
}
