using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Green
{
    internal class BismuthEarrings
    {
        public static ItemDef bismuthEarringsItem;
        public static float BaseBarrier = 15;
        public static float StackingBarrier = 10;

        private static List<BuffDef> _bleedBuffs = null;
        public static List<BuffDef> bleedBuffs
        {
            get
            {
                if(_bleedBuffs == null)
                {
                    _bleedBuffs = new List<BuffDef>
                    {
                        RoR2Content.Buffs.Bleeding,
                        RoR2Content.Buffs.SuperBleed
                   };
                }

                return _bleedBuffs;
            }
        } 

        //public static Material
        public static void Setup()
        {
            bismuthEarringsItem = ScriptableObject.CreateInstance<ItemDef>();
            bismuthEarringsItem.deprecatedTier = ItemTier.Tier2;
            bismuthEarringsItem.name = "itembarriercrit";
            bismuthEarringsItem.nameToken = "ITEM_BARRIERONCRIT_NAME";
            bismuthEarringsItem.descriptionToken = "ITEM_BARRIERONCRIT_DESCRIPTION";
            bismuthEarringsItem.pickupToken = "ITEM_BARRIERONCRIT_PICKUP";
            bismuthEarringsItem.loreToken = "ITEM_BARRIERONCRIT_LORE";
            bismuthEarringsItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            bismuthEarringsItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("IMDLBismuthRings");
            bismuthEarringsItem.pickupIconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texBismuthEarring");
            bismuthEarringsItem.tags = new ItemTag[]
            {
                ItemTag.Healing
            };

            ContentAddition.AddItemDef(bismuthEarringsItem);

            Modules.Language.Add("ITEM_BARRIERONCRIT_NAME", "Bismuth Earrings");
            Modules.Language.Add("ITEM_BARRIERONCRIT_DESCRIPTION", "Gain <style=cIsDamage>5% bleed chance</style>. Gain a <style=cIsHealing>temporary barrier</style> on applying bleed for <style=cIsHealing>" + BaseBarrier + " health</style> <style=cStack>(+" + StackingBarrier + " per stack)</style>.");
            Modules.Language.Add("ITEM_BARRIERONCRIT_PICKUP", "Gain barrier on applying bleed");
            Modules.Language.Add("ITEM_BARRIERONCRIT_LORE", "The Earrings are Bismuth or something idk.");

            //On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            //On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;

            On.RoR2.DotController.AddDot += DotController_AddDot;
        }

        private static void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotController.DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotController.DotIndex? preUpgradeDotIndex)
        {
            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);

            if (attackerObject == null) return;
            CharacterBody attackerBody = attackerObject.GetComponent<CharacterBody>();
            if (attackerBody == null) return;
            if (attackerBody.inventory == null) return;
            
            int itemCount = attackerBody.inventory.GetItemCount(bismuthEarringsItem);
            if (itemCount <= 0)
                return;
            if (bleedBuffs.Contains(DotController.dotDefs[(int)dotIndex].associatedBuff))
            {
                attackerBody.healthComponent?.AddBarrier(BaseBarrier + StackingBarrier * (itemCount - 1));
            }
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);
            if (damageInfo.attacker == null) return;

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            if(attackerBody == null) return;
            
            Inventory inventory = attackerBody.inventory;
            if(inventory)
            {
                int itemCount = inventory.GetItemCount(bismuthEarringsItem) - 1;
                if (victim.GetComponent<CharacterBody>().HasBuff(RoR2Content.Buffs.Bleeding))
                {
                    attackerBody.healthComponent?.AddBarrier(BaseBarrier + StackingBarrier * itemCount);
                }
                //if(damageInfo.damageType.HasFlag(DamageType.BleedOnHit))
                //{
                //    attackerBody.healthComponent?.AddBarrier(BaseBarrier + StackingBarrier * itemCount);
                //}
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self.inventory)
            {
                if (self.inventory.GetItemCount(bismuthEarringsItem) > 0)
                {
                    self.bleedChance += 5;
                }
            }
        }
            
        private static void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo damageInfo, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
            orig(self, body, damageInfo, master, procCoefficient, procChainMask);
            if (body.inventory)
            {
                int earringCount = body.inventory.GetItemCount(bismuthEarringsItem);
                if (earringCount > 0)
                {
                    body.healthComponent.AddBarrier(BaseBarrier + (earringCount - 1) * StackingBarrier);
                }
            }
        }
    }
}
