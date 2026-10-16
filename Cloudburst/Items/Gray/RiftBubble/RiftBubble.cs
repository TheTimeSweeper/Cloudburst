using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Gray.RiftBubble
{
    internal class RiftBubble
    {
        public static ItemDef riftBubbleItem;
        public static BuffDef riftBuff;

        public static float BaseRadius = 5;
        public static float StackingRadius = 5;
        public static float AttackSpeedReduction = 0.5f;
        public static float MoveSpeedReduction = 0.33f;
        public static float KnockbackMultiplier = 0.5f;

        public static GameObject riftBubbleIndicator;
        public static Material riftBubbleMaterial;
        public static Material riftBubbleSecondaryMaterial;

        public static void Setup()
        {
            riftBubbleItem = ScriptableObject.CreateInstance<ItemDef>();
            riftBubbleItem.deprecatedTier = ItemTier.Tier1;
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
            riftBuff.name = "bdRiftBubbleSlow";
            riftBuff.canStack = false;
            riftBuff.isDebuff = true;
            riftBuff.iconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texBuffSlowRadius");
            riftBuff.buffColor = Color.white;

            ContentAddition.AddBuffDef(riftBuff);

            Modules.Language.Add("ITEM_SLOWRADIUS_NAME", "Rift Bubble");
            Modules.Language.Add("ITEM_SLOWRADIUS_PICKUP", "Reduce incoming knockback and slow nearby enemies.");
            Modules.Language.Add("ITEM_SLOWRADIUS_DESCRIPTION", "Enemies within <style=cIsUtility>" + BaseRadius + "m</style> <style=cStack>(+" + StackingRadius + "m per stack)</style> have their <style=cIsUtility>attack speed</style> reduced by <style=cIsUtility>" + (AttackSpeedReduction * 100) + "%</style> and <style=cIsUtility>movement speed</style> reduced by <style=cIsUtility>" + (MoveSpeedReduction * 100) + "%</style>. Reduce incoming <style=cIsUtility>knockback</style> by <style=cIsUtility>" + ((1 - KnockbackMultiplier) * 100) + "%</style>.");
            Modules.Language.Add("ITEM_SLOWRADIUS_LORE", "");

            CreateIndicator();

            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.CharacterMotor.ApplyForceImpulse += CharacterMotor_ApplyForceImpulse;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private static void CreateIndicator()
        {
            riftBubbleIndicator = Cloudburst.CloudburstAssets.LoadAsset<GameObject>("RiftBubbleIndicator");
            Transform indicator = riftBubbleIndicator.transform.Find("Indicator");

            riftBubbleIndicator.AddComponent<TeamFilter>();
            BuffWard buffWard = riftBubbleIndicator.AddComponent<BuffWard>();
            buffWard.rangeIndicator = indicator;
            buffWard.buffDef = riftBuff;
            buffWard.buffDuration = 2;
            buffWard.interval = 0.2f;
            buffWard.floorWard = false;
            buffWard.expires = false;
            buffWard.invertTeamFilter = true;
            buffWard.shape = BuffWard.BuffWardShape.Sphere;

            riftBubbleMaterial = UnityEngine.Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/CritOnUse/matFullCrit.mat").WaitForCompletion());
            riftBubbleMaterial.SetColor("_TintColor", new Color(1f, 1f, 1f, 1f));
            riftBubbleMaterial.SetTexture("_RemapTex", Cloudburst.CloudburstAssets.LoadAsset<Texture>("texRampRiftBubble2"));
            riftBubbleMaterial.SetFloat("_Boost", 0.1f);
            riftBubbleMaterial.SetFloat("_RimPower", 1);
            riftBubbleMaterial.SetFloat("_RimStrength", 1f);
            riftBubbleMaterial.SetFloat("_IntersectionStrength", 1.2f);
            riftBubbleMaterial.SetFloat("_Cull", 1f);
            indicator.Find("Sphere").GetComponent<Renderer>().sharedMaterial = riftBubbleMaterial;

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

        private static void CharacterMotor_ApplyForceImpulse(On.RoR2.CharacterMotor.orig_ApplyForceImpulse orig, CharacterMotor self, ref PhysForceInfo forceInfo)
        {
            //the server forwards forces to the owning client, so only scale where the force actually gets applied or clients get it twice
            if (self.hasEffectiveAuthority && self.body && self.body.inventory && self.body.inventory.GetItemCountEffective(riftBubbleItem) > 0)
            {
                PhysForceInfo reduced = forceInfo;
                reduced.force *= KnockbackMultiplier;
                orig(self, ref reduced);
                return;
            }
            orig(self, ref forceInfo);
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(riftBuff))
            {
                args.attackSpeedMultAdd -= AttackSpeedReduction;
                args.moveSpeedMultAdd -= MoveSpeedReduction;
            }
        }

        private static void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);
            self.AddItemBehavior<RiftBehaviour>(self.inventory.GetItemCountEffective(riftBubbleItem));
        }
    }
}
