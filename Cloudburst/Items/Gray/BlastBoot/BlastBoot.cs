using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Cloudburst.Modules.ItemDisplays;

namespace Cloudburst.Items.Gray.BlastBoot
{
    internal class BlastBoot
    {
        public static ItemDef blastBootItem;

        public static GameObject fireworkPrefab;

        public static float Cooldown = 3;
        public static int BaseFireworks = 3;
        public static float BaseDamage = 1;
        public static float StackingDamage = 0.5f;

        public static void Setup()
        {
            blastBootItem = ScriptableObject.CreateInstance<ItemDef>();
            blastBootItem.deprecatedTier = ItemTier.Tier1;
            blastBootItem.name = "BlastBoot";
            blastBootItem.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Assets/Cloudburst/Items/CarePackageRequester/icon.png");
            blastBootItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("IMDLBlastBoot");
            blastBootItem.nameToken = "ITEM_SECONDARYBOOST_NAME";
            blastBootItem.descriptionToken = "ITEM_SECONDARYBOOST_DESCRIPTION";
            blastBootItem.pickupToken = "ITEM_SECONDARYBOOST_PICKUP";
            blastBootItem.loreToken = "ITEM_SECONDARYBOOST_LORE";
            blastBootItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            blastBootItem.tags = new ItemTag[]
            {
                ItemTag.Damage,
                ItemTag.Utility,
                ItemTag.AIBlacklist,
                ItemTag.CanBeTemporary,
            };

            CreateProjectile();

            ItemAPI.Add(new CustomItem(blastBootItem, CreateItemDisplayRules()));

            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;

            Modules.Language.Add("ITEM_SECONDARYBOOST_NAME", "Blast Boot");
            Modules.Language.Add("ITEM_SECONDARYBOOST_PICKUP", "Firework-powered double jump upon Secondary Skill activation.");
            Modules.Language.Add("ITEM_SECONDARYBOOST_DESCRIPTION", "Activating your <style=cIsUtility>Secondary skill</style> while airborne also <style=cIsUtility>blasts you through the air</style> with a flurry of <style=cIsDamage>" + (BaseFireworks + 1) + "</style> <style=cStack>(+1 per stack)</style> fireworks that deal <style=cIsDamage>" + (BaseDamage * 100) + "%</style> <style=cStack>(+" + (StackingDamage * 100) + "% per stack)</style> damage and <style=cIsDamage>stun</style>. Recharges every <style=cIsUtility>" + Cooldown + "</style> seconds.");
            Modules.Language.Add("ITEM_SECONDARYBOOST_LORE", "she on my boot til I blast");
        }

        public static void CreateProjectile()
        {
            //acrid's spit as a base so the fireworks fly straight instead of homing like the vanilla ones
            GameObject firework = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Croco/CrocoSpit.prefab").WaitForCompletion().InstantiateClone("BlastBootFirework", true);
            GameObject orig = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Firework/FireworkProjectile.prefab").WaitForCompletion();

            ProjectileController controller = firework.GetComponent<ProjectileController>();
            ProjectileController origController = orig.GetComponent<ProjectileController>();
            controller.ghostPrefab = origController.ghostPrefab;
            controller.ghostPrefabAddress = origController.ghostPrefabAddress;

            foreach (var soundEvents in firework.GetComponents<AkEvent>())
            {
                UnityEngine.Object.Destroy(soundEvents);
            }

            foreach (var originalSound in orig.GetComponents<AkEvent>())
            {
                CCUtilities.CopyComponent<AkEvent>(originalSound, firework);
            }

            firework.GetComponent<ProjectileImpactExplosion>().impactEffect = orig.GetComponent<ProjectileImpactExplosion>().impactEffect;
            firework.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;

            fireworkPrefab = firework;
            ContentAddition.AddProjectile(fireworkPrefab);
        }

        private static void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            self.AddItemBehavior<BlastBootBehavior>(self.inventory.GetItemCountEffective(blastBootItem));
        }

        private static ItemDisplayRuleDict CreateItemDisplayRules()
        {
            GameObject display = CreateItemDisplayPrefab(blastBootItem.pickupModelPrefab, "DisplayBlastBoot");

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", CreateDisplayRuleArray(display, "CalfL", new Vector3(0.02731f, 0.28558f, -0.01864f), new Vector3(7.92373f, 187.8987f, 185.4126f), new Vector3(0.3f, 0.3f, 0.3f)));
            rules.Add("mdlHuntress", CreateDisplayRuleArray(display, "FootL", new Vector3(0.01535f, 0.00952f, -0.11311f), new Vector3(290.3859f, 87.12296f, 267.2033f), new Vector3(0.2f, 0.2f, 0.2f)));
            rules.Add("mdlToolbot", CreateDisplayRuleArray(display, "Base", new Vector3(-0.00199f, 3.38665f, -2.82651f), new Vector3(350.9689f, 354.3693f, 177.4269f), new Vector3(1.4f, 1.4f, 1.4f)));
            rules.Add("mdlEngi", CreateDisplayRuleArray(display, "CalfL", new Vector3(0.07311f, 0.09037f, -0.00888f), new Vector3(12.57096f, 162.2495f, 178.7365f), new Vector3(0.47f, 0.47f, 0.6f)));
            rules.Add("mdlMage", CreateDisplayRuleArray(display, "FootL", new Vector3(-0.00998f, 0.01119f, -0.06469f), new Vector3(315.5827f, 187.5823f, 189.1592f), new Vector3(0.2f, 0.2f, 0.2f)));
            rules.Add("mdlMerc", CreateDisplayRuleArray(display, "FootL", new Vector3(0.03039f, 0.01253f, -0.11448f), new Vector3(304.5077f, 186.6602f, 173.0096f), new Vector3(0.25f, 0.25f, 0.25f)));
            rules.Add("mdlLoader", CreateDisplayRuleArray(display, "FootL", new Vector3(0.006f, -0.10728f, -0.20047f), new Vector3(318.6605f, 150.0478f, 194.3605f), new Vector3(0.4f, 0.4f, 0.4f)));
            rules.Add("mdlCroco", CreateDisplayRuleArray(display, "CalfL", new Vector3(0.17916f, 2.85797f, -0.95134f), new Vector3(10.90281f, 181.5084f, 186.1528f), new Vector3(3f, 3f, 3f)));
            rules.Add("mdlCaptain", CreateDisplayRuleArray(display, "CalfL", new Vector3(0.06974f, 0.19848f, -0.15316f), new Vector3(18.68582f, 179.9198f, 188.5853f), new Vector3(0.4f, 0.4f, 0.4f)));
            rules.Add("mdlTreebot", CreateDisplayRuleArray(display, "FootFrontLEnd", new Vector3(-0.02173f, 0.0097f, -0.02386f), new Vector3(0.31136f, 150.9228f, 175.0508f), new Vector3(0.2f, 0.2f, 0.2f)));
            rules.Add("mdlBandit2", CreateDisplayRuleArray(display, "CalfL", new Vector3(0.05839f, 0.35845f, -0.08628f), new Vector3(9.67126f, 197.2447f, 199.8692f), new Vector3(0.32f, 0.32f, 0.32f)));
            rules.Add("mdlVoidSurvivor", CreateDisplayRuleArray(display, "CalfL", new Vector3(0.0255f, 0.36213f, 0.02881f), new Vector3(349.1798f, 86.03563f, 189.2694f), new Vector3(0.30619f, 0.30619f, 0.30619f)));
            rules.Add("mdlRailGunner", CreateDisplayRuleArray(display, "CalfL", new Vector3(0.01771f, 0.35205f, 0.0558f), new Vector3(355.6657f, 83.68776f, 196.9867f), new Vector3(0.39992f, 0.39992f, 0.39992f)));
            return rules;
        }
    }
}
