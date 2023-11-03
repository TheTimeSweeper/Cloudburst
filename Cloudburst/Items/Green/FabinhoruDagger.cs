using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Green
{
    internal class FabinhoruDagger
    {
        public static ItemDef fabinhorusDaggerItem;
        public static BuffDef fabinhorusBuff;

        public static GameObject FabProc;
        public static Material matFabCripple;

        public static Shader standard = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGStandard.shader").WaitForCompletion();
        public static void Setup()
        {
            fabinhorusDaggerItem = ScriptableObject.CreateInstance<ItemDef>();
            fabinhorusDaggerItem.deprecatedTier = ItemTier.Tier2;
            fabinhorusDaggerItem.name = "itembleedcripple";
            fabinhorusDaggerItem.nameToken = "ITEM_BLEEDCRIPPLE_NAME";
            fabinhorusDaggerItem.pickupToken = "ITEM_BLEEDCRIPPLE_PICKUP";
            fabinhorusDaggerItem.descriptionToken = "ITEM_BLEEDCRIPPLE_DESCRIPTION";
            fabinhorusDaggerItem.loreToken = "ITEM_BLEEDCRIPPLE_LORE";
            fabinhorusDaggerItem.pickupIconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texFabDagger");

            GameObject PickupPrefab = Cloudburst.CloudburstAssets.LoadAsset<GameObject>("FabDaggerPickup");
            Material mat = PickupPrefab.GetComponentInChildren<Renderer>().material;
            mat.shader = standard;
            mat.SetTexture("_NormalTex", Cloudburst.CloudburstAssets.LoadAsset<Texture>("fabdagger_normal"));
            mat.SetFloat("_NormalStrength", 1.6f);
            mat.SetFloat("_Smoothness", 0.25f);
            mat.SetFloat("_ForceSpecOn", 1);
            mat.SetFloat("_SpecularStrength", 1);
            mat.SetFloat("_SpecularExponent", 9);
            mat.EnableKeyword("FORCE_SPEC");

            fabinhorusDaggerItem.pickupModelPrefab = PickupPrefab;

            ContentAddition.AddItemDef(fabinhorusDaggerItem);

            fabinhorusBuff = ScriptableObject.CreateInstance<BuffDef>();
            fabinhorusBuff.canStack = true;
            fabinhorusBuff.isDebuff = true;
            fabinhorusBuff.iconSprite = Cloudburst.CloudburstAssets.LoadAsset<Sprite>("texBuffBleedCripple");
            fabinhorusBuff.buffColor = Color.white;
            fabinhorusDaggerItem.requiredExpansion = Cloudburst.cloudburstExpansion;

            ContentAddition.AddBuffDef(fabinhorusBuff);

            LanguageAPI.Add("ITEM_BLEEDCRIPPLE_NAME", "Fabinhoru's Dagger");
            LanguageAPI.Add("ITEM_BLEEDCRIPPLE_PICKUP", "Striking bleeding enemies reduces their armor.");
            LanguageAPI.Add("ITEM_BLEEDCRIPPLE_DESCRIPTION",
                "<style=cIsDamage>Striking</style> enemies while they are <style=cIsDamage>bleeding</style> reduces their <style=cIsDamage>armor</style> by <style=cIsDamage>30</style> <style=cStack>(+15 per stack)</style>. Also gain <style=cIsDamage>5%</style> chance to <style=cIsDamage>bleed</style> an enemy on hit.");
            LanguageAPI.Add("ITEM_BLEEDCRIPPLE_LORE", "");

           FabProc = Cloudburst.CloudburstAssets.LoadAsset<GameObject>("FabDaggerIndicator");
            LightIntensityCurve curve = FabProc.transform.GetChild(1).gameObject.AddComponent<LightIntensityCurve>();
            curve.curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
            curve.timeMax = 0.75f;
            
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
            int num = model.activeOverlayCount;
            if (num >= CharacterModel.maxOverlays)
            {
                return;
            }
            if (condition)
            {
                Material[] array = model.currentOverlays;
                num += 1;
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

            if (damageInfo.attacker == null) return;

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();

            if(!attackerBody || !victimBody) return;

            Inventory inventory = attackerBody.inventory;
            int itemCount = inventory.GetItemCount(fabinhorusDaggerItem);

            if (inventory && inventory.GetItemCount(fabinhorusDaggerItem) > 0)
            {
                if(victimBody.HasBuff(RoR2Content.Buffs.Bleeding) || victimBody.HasBuff(RoR2Content.Buffs.SuperBleed))
                {
                    victimBody.AddTimedBuff(fabinhorusBuff, 2.5f + (2.5f * itemCount));
                    /*
                    EffectManager.SpawnEffect(FabProc, new EffectData()
                    {
                        origin = damageInfo.position
                    }, false);
                    */
                }
            }
        }
    }
}
