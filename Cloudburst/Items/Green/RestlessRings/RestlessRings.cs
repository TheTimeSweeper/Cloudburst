using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static Cloudburst.Modules.ItemDisplays;

namespace Cloudburst.Items.Green.RestlessRings
{
    internal class RestlessRings
    {
        public static ItemDef restlessRingsItem;

        public static BuffDef armorBuff;
        public static BuffDef attackSpeedBuff;
        public static BuffDef regenBuff;
        public static BuffDef[] ringBuffs;

        public static Material armorMaterial;
        public static Material attackSpeedMaterial;
        public static Material regenMaterial;
        public static Material basicMaterial;

        public static float BuffInterval = 10;
        public static float ArmorPerStack = 8;
        public static float AttackSpeedPerStack = 0.1f;
        public static float RegenPerStack = 0.2f;

        public static void Setup()
        {
            restlessRingsItem = ScriptableObject.CreateInstance<ItemDef>();
            restlessRingsItem.deprecatedTier = ItemTier.Tier2;
            restlessRingsItem.name = "itemrestlessrings";
            restlessRingsItem.nameToken = "ITEM_RESTLESSRINGS_NAME";
            restlessRingsItem.pickupToken = "ITEM_RESTLESSRINGS_PICKUP";
            restlessRingsItem.descriptionToken = "ITEM_RESTLESSRINGS_DESCRIPTION";
            restlessRingsItem.loreToken = "ITEM_RESTLESSRINGS_LORE";
            restlessRingsItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            restlessRingsItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("Assets/Cloudburst/Items/RestlessRings/IMDLRestlessRings.prefab");
            restlessRingsItem.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Assets/Cloudburst/Items/RestlessRings/icon.png");
            restlessRingsItem.tags = new ItemTag[]
            {
                ItemTag.CanBeTemporary,
                ItemTag.Utility,
                ItemTag.Healing,
                ItemTag.Damage
            };

            armorMaterial = Cloudburst.OldCloudburstAssets.LoadAsset<Material>("matRestlessRingArmor");
            attackSpeedMaterial = Cloudburst.OldCloudburstAssets.LoadAsset<Material>("matRestlessRingAttack");
            regenMaterial = Cloudburst.OldCloudburstAssets.LoadAsset<Material>("matRestlessRingHeal");
            basicMaterial = Cloudburst.OldCloudburstAssets.LoadAsset<Material>("matRestfulRing");

            ItemAPI.Add(new CustomItem(restlessRingsItem, CreateItemDisplayRules()));

            armorBuff = CreateBuff("bdRestlessRingsArmor", "RestlessSquare");
            attackSpeedBuff = CreateBuff("bdRestlessRingsAttackSpeed", "RestlessTriangle");
            regenBuff = CreateBuff("bdRestlessRingsRegen", "RestlessCircle");
            ringBuffs = new BuffDef[] { armorBuff, attackSpeedBuff, regenBuff };

            Modules.Language.Add("ITEM_RESTLESSRINGS_NAME", "Restless Rings");
            Modules.Language.Add("ITEM_RESTLESSRINGS_PICKUP", "Every " + BuffInterval + " seconds, gain a buff that can grant you armor, or regeneration, or attack speed.");
            Modules.Language.Add("ITEM_RESTLESSRINGS_DESCRIPTION", "Every " + BuffInterval + " seconds, gain a buff which can grant you one of the following:"
                + "\n<style=cIsUtility>+" + ArmorPerStack + " <style=cStack>(+" + ArmorPerStack + " per stack)</style> armor</style>"
                + "\n<style=cIsHealing>+" + (RegenPerStack * 100) + "% <style=cStack>(+" + (RegenPerStack * 100) + "% per stack)</style> regeneration</style>"
                + "\n<style=cIsDamage>+" + (AttackSpeedPerStack * 100) + "% <style=cStack>(+" + (AttackSpeedPerStack * 100) + "% per stack)</style> attack speed</style>");
            Modules.Language.Add("ITEM_RESTLESSRINGS_LORE", "A pantheon of siblings. \nA celestial mystery. \nA throne awaiting a heir. \n\nThe one with the most brilliant mind among them had found the thread that connects them. Born of grand divinity and the arcane, it was of his nature to peel back the \nof existence and seek the shimmering, guiding light underneath. \nSo close he came to touching the source of said light, but in his hubris he had wronged so many. The scythe of death had fell upon him before, and had condemned him until the earth had revolved about the sun one hundred times. In his freedom, derrangement and fervor proliferated within him, and soon enough he would make a fatal mistake. \nHe had tried to sacrifice the blood of his nemesis' next of kin. With this folly, he would die thrice more. Each at the hand of malice itself.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nfar...\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nFar did he fall.\n");

            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private static BuffDef CreateBuff(string name, string iconName)
        {
            BuffDef buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.name = name;
            buff.canStack = false;
            buff.isDebuff = false;
            buff.buffColor = Color.white;
            buff.iconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>(iconName);

            ContentAddition.AddBuffDef(buff);

            return buff;
        }

        private static void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            if (NetworkServer.active && self.inventory)
            {
                self.AddItemBehavior<RestlessRingsBehavior>(self.inventory.GetItemCountEffective(restlessRingsItem));
            }
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!sender.inventory)
                return;

            int itemCount = sender.inventory.GetItemCountEffective(restlessRingsItem);
            if (itemCount <= 0)
                return;

            if (sender.HasBuff(armorBuff))
            {
                args.armorAdd += ArmorPerStack * itemCount;
            }
            if (sender.HasBuff(attackSpeedBuff))
            {
                args.attackSpeedMultAdd += AttackSpeedPerStack * itemCount;
            }
            if (sender.HasBuff(regenBuff))
            {
                args.regenMultAdd += RegenPerStack * itemCount;
            }
        }

        private static ItemDisplayRuleDict CreateItemDisplayRules()
        {
            //the rings float around the body. the dummy is what gets parented to the body, the follower smoothly chases it in world space
            GameObject followerPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("RestlessFollower");
            GameObject display = CreateItemDisplayPrefab(Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("RestlessFollowerDummy"), "DisplayRestlessRings");

            ItemFollowerSmooth itemFollower = display.AddComponent<ItemFollowerSmooth>();
            itemFollower.itemDisplay = display.GetComponent<ItemDisplay>();
            itemFollower.followerPrefab = followerPrefab;
            itemFollower.targetObject = display;
            itemFollower.distanceDampTime = 0.3f;
            itemFollower.distanceMaxSpeed = 50;
            itemFollower.SmoothingNumber = 0.1f;
            display.AddComponent<RestlessRingsGlow>();

            Vector3 angles = new Vector3(270f, 0f, 0f);
            Vector3 scale = new Vector3(0.15f, 0.15f, 0.15f);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", CreateDisplayRuleArray(display, "Base", new Vector3(-1.66776f, -0.21747f, -0.52862f), angles, scale));
            rules.Add("mdlHuntress", CreateDisplayRuleArray(display, "Base", new Vector3(-1.32395f, 0.51393f, -0.71607f), angles, scale));
            rules.Add("mdlToolbot", CreateDisplayRuleArray(display, "Base", new Vector3(8.79843f, 2.4368f, 5.00038f), new Vector3(270f, 180f, 0f), scale));
            rules.Add("mdlEngi", CreateDisplayRuleArray(display, "Base", new Vector3(-2.26939f, -1.21903f, -0.00543f), angles, scale));
            rules.Add("mdlMage", CreateDisplayRuleArray(display, "Base", new Vector3(-0.94248f, -0.05662f, -0.01945f), angles, scale));
            rules.Add("mdlMerc", CreateDisplayRuleArray(display, "Base", new Vector3(-0.91384f, -0.37104f, 0.1309f), angles, scale));
            rules.Add("mdlTreebot", CreateDisplayRuleArray(display, "Base", new Vector3(-2.34945f, 0.69277f, -1.27591f), angles, scale));
            rules.Add("mdlLoader", CreateDisplayRuleArray(display, "Base", new Vector3(-1.26621f, -0.11008f, 0.05336f), angles, scale));
            rules.Add("mdlCroco", CreateDisplayRuleArray(display, "Base", new Vector3(-7.26764f, -5.35658f, 4.52467f), new Vector3(90f, 0f, 0f), new Vector3(0.2f, 0.2f, 0.2f)));
            rules.Add("mdlCaptain", CreateDisplayRuleArray(display, "Base", new Vector3(-1.0712f, -0.0798f, -0.59302f), angles, scale));
            rules.Add("mdlBandit2", CreateDisplayRuleArray(display, "Base", new Vector3(-0.45782f, -0.51298f, -0.39241f), new Vector3(347.6326f, 269.9879f, 89.96059f), new Vector3(0.1f, 0.1f, 0.1f)));
            return rules;
        }
    }
}
