using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Green.RiftBubble
{
    internal class RiftBubble
    {
        public static ItemDef riftBubbleItem;
        public static BuffDef riftBuff;

        public static float Radius = 10;
        public static float BaseInterval = 15;
        public static float IntervalReductionPerStack = 0.1f;
        public static float BaseDuration = 3;
        public static float StackingDuration = 1;

        public static GameObject riftBubbleIndicator;
        public static Material riftBubbleSecondaryMaterial;

        //-10% per stack compounds so the interval never hits zero
        public static float GetInterval(int stack) => BaseInterval * Mathf.Pow(1 - IntervalReductionPerStack, stack - 1);
        public static float GetDuration(int stack) => BaseDuration + StackingDuration * (stack - 1);

        public static void Setup()
        {
            riftBubbleItem = ScriptableObject.CreateInstance<ItemDef>();
            riftBubbleItem.deprecatedTier = ItemTier.Tier2;
            riftBubbleItem.name = "itemslowradius";
            riftBubbleItem.nameToken = "ITEM_SLOWRADIUS_NAME";
            riftBubbleItem.pickupToken = "ITEM_SLOWRADIUS_PICKUP";
            riftBubbleItem.descriptionToken = "ITEM_SLOWRADIUS_DESCRIPTION";
            riftBubbleItem.loreToken = "ITEM_SLOWRADIUS_LORE";
            riftBubbleItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            riftBubbleItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("Assets/Cloudburst/Items/RiftBubble/IMDLRiftBubble.prefab");
            riftBubbleItem.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Assets/Cloudburst/Items/RiftBubble/icon.png");
            riftBubbleItem.tags = new ItemTag[]
            {
                ItemTag.Utility,
                ItemTag.CanBeTemporary,
            };

            ItemAPI.Add(new CustomItem(riftBubbleItem, new ItemDisplayRuleDict()));

            riftBuff = ScriptableObject.CreateInstance<BuffDef>();
            riftBuff.name = "bdRiftBubbleIsolated";
            riftBuff.canStack = false;
            riftBuff.isDebuff = true;
            riftBuff.iconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texBuffSlowRadius");
            riftBuff.buffColor = Color.white;

            ContentAddition.AddBuffDef(riftBuff);

            Modules.Language.Add("ITEM_SLOWRADIUS_NAME", "Rift Bubble");
            Modules.Language.Add("ITEM_SLOWRADIUS_PICKUP", "Periodically spawn a bubble that stops enemies inside from attacking.");
            Modules.Language.Add("ITEM_SLOWRADIUS_DESCRIPTION", "Every <style=cIsUtility>" + BaseInterval + "s</style> <style=cStack>(-" + (IntervalReductionPerStack * 100) + "% per stack)</style>, spawn an <style=cIsUtility>isolating bubble</style> with a <style=cIsUtility>" + Radius + "m</style> radius. Enemies inside <style=cIsUtility>cannot attack</style>. The bubble lasts <style=cIsUtility>" + BaseDuration + "s</style> <style=cStack>(+" + StackingDuration + "s per stack)</style>.");
            Modules.Language.Add("ITEM_SLOWRADIUS_LORE", "");

            CreateIndicator();

            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.GenericSkill.ExecuteIfReady += GenericSkill_ExecuteIfReady;
        }

        private static void CreateIndicator()
        {
            riftBubbleIndicator = Cloudburst.CloudburstAssets.LoadAsset<GameObject>("RiftBubbleIndicator");
            Transform indicator = riftBubbleIndicator.transform.Find("Indicator");

            //BuffWard checks distance, not colliders, so strip them to let pings pass through the bubble
            foreach (Collider collider in riftBubbleIndicator.GetComponentsInChildren<Collider>(true))
            {
                UnityEngine.Object.DestroyImmediate(collider, true);
            }

            riftBubbleIndicator.AddComponent<TeamFilter>();
            BuffWard buffWard = riftBubbleIndicator.AddComponent<BuffWard>();
            buffWard.rangeIndicator = indicator;
            buffWard.buffDef = riftBuff;
            //short so enemies get their attacks back soon after leaving the bubble
            buffWard.buffDuration = 0.5f;
            buffWard.interval = 0.2f;
            buffWard.floorWard = false;
            buffWard.expires = false;
            buffWard.invertTeamFilter = true;
            buffWard.shape = BuffWard.BuffWardShape.Sphere;

            //BuffWard scales the indicator to the radius, so its meshes need a radius of 1. both shipped at 0.5, which drew the bubble at half size
            indicator.Find("Sphere").localScale *= 2f;
            indicator.Find("Outline").localScale *= 2f;

            //the oddly shaped opal bubble
            indicator.Find("Sphere").GetComponent<Renderer>().sharedMaterial = Addressables.LoadAssetAsync<Material>("RoR2/DLC1/OutOfCombatArmor/matOpalShield.mat").WaitForCompletion();

            riftBubbleSecondaryMaterial = UnityEngine.Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/NearbyDamageBonus/matNearbyDamageBonusRangeIndicator.mat").WaitForCompletion());
            riftBubbleSecondaryMaterial.SetColor("_TintColor", new Color(0.02f, 0.01f, 0.3f, 1f));
            indicator.Find("Outline").GetComponent<Renderer>().sharedMaterial = riftBubbleSecondaryMaterial;

            //matRiftStar and matRiftFlare were bundled with no shader or textures, so they render pink. swap in vanilla void stars instead
            Material starMaterial = Addressables.LoadAssetAsync<Material>("RoR2/Base/Nullifier/matNullifierStarParticle.mat").WaitForCompletion();
            Material flareMaterial = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matGenericStarburst.mat").WaitForCompletion();
            Transform particles = indicator.Find("Particles");
            particles.Find("Stars, Big").GetComponent<ParticleSystemRenderer>().sharedMaterial = starMaterial;
            particles.Find("Stars, Small").GetComponent<ParticleSystemRenderer>().sharedMaterial = starMaterial;
            particles.Find("Stars, Radiant").GetComponent<ParticleSystemRenderer>().sharedMaterial = flareMaterial;
        }

        private static bool GenericSkill_ExecuteIfReady(On.RoR2.GenericSkill.orig_ExecuteIfReady orig, GenericSkill self)
        {
            if (self.characterBody && self.characterBody.HasBuff(riftBuff))
            {
                return false;
            }
            return orig(self);
        }

        private static void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            self.AddItemBehavior<RiftBehaviour>(self.inventory.GetItemCountEffective(riftBubbleItem));
        }
    }
}
