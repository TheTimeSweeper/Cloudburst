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

        public static GameObject FabProc;
        public static Material matFabCripple;
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
            fabinhorusBuff.iconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texBuffBleedCripple");
            fabinhorusBuff.buffColor = Color.white;
            fabinhorusDaggerItem.requiredExpansion = Cloudburst.cloudburstExpansion;

            ContentAddition.AddBuffDef(fabinhorusBuff);

            LanguageAPI.Add("ITEM_BLEEDCRIPPLE_NAME", "Fabinhoru's Dagger");

            FabProc = Cloudburst.CloudburstAssets.LoadAsset<GameObject>("FabDaggerIndicator");
            FabProc.AddComponent<DestroyOnParticleEnd>().ps = FabProc.transform.GetChild(0).GetComponent<ParticleSystem>();
            EffectComponent effect = FabProc.AddComponent<EffectComponent>();
            ContentAddition.AddEffect(FabProc);

            matFabCripple = Cloudburst.CloudburstAssets.LoadAsset<Material>("matFabCripple");

            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
        }

        private static void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self)
        {
            orig(self);
            if (self.body)
            {
                AddOverlay(self, matFabCripple, self.body.HasBuff(fabinhorusBuff));
            }
        }

        private static void AddOverlay(CharacterModel model, Material overlayMaterial, bool condition)
        {
            if (model.activeOverlayCount >= CharacterModel.maxOverlays)

            {
                return;
            }
            if (condition)
            {
                Material[] array = model.currentOverlays;
                int num = model.activeOverlayCount;
                model.activeOverlayCount = num + 1;
                array[num] = overlayMaterial;
            }
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
                    EffectManager.SpawnEffect(FabProc, new EffectData()
                    {
                        origin = damageInfo.position
                    }, false);
                }
            }
        }
    }
}
