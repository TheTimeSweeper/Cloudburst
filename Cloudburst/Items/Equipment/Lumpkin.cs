using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using static Cloudburst.Modules.ItemDisplays;

namespace Cloudburst.Items.Equipment
{
    internal class Lumpkin
    {
        public static EquipmentDef lumpkinEquipment;
        public static GameObject screamEffect;

        public static float DamageCoefficient = 5;
        public static float MaxHealthCoefficient = 2;
        public static float Radius = 15;
        public static float Cooldown = 60;

        public static void Setup()
        {
            lumpkinEquipment = ScriptableObject.CreateInstance<EquipmentDef>();
            lumpkinEquipment.name = "equipmentlumpkin";
            lumpkinEquipment.nameToken = "EQUIPMENT_LUMPKIN_NAME";
            lumpkinEquipment.pickupToken = "EQUIPMENT_LUMPKIN_PICKUP";
            lumpkinEquipment.descriptionToken = "EQUIPMENT_LUMPKIN_DESCRIPTION";
            lumpkinEquipment.loreToken = "EQUIPMENT_LUMPKIN_LORE";
            lumpkinEquipment.requiredExpansion = Cloudburst.cloudburstExpansion;
            lumpkinEquipment.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("Assets/Cloudburst/Equipment/Lumpkin/IMDLLumpkin.prefab");
            lumpkinEquipment.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Assets/Cloudburst/Equipment/Lumpkin/icon.png");
            lumpkinEquipment.appearsInSinglePlayer = true;
            lumpkinEquipment.appearsInMultiPlayer = true;
            lumpkinEquipment.canDrop = true;
            lumpkinEquipment.enigmaCompatible = true;
            lumpkinEquipment.canBeRandomlyTriggered = true;
            lumpkinEquipment.cooldown = Cooldown;

            ItemAPI.Add(new CustomEquipment(lumpkinEquipment, CreateItemDisplayRules()));

            Modules.Language.Add("EQUIPMENT_LUMPKIN_NAME", "The Lumpkin");
            Modules.Language.Add("EQUIPMENT_LUMPKIN_PICKUP", "And his screams were Brazilian...");
            Modules.Language.Add("EQUIPMENT_LUMPKIN_DESCRIPTION", "Release a Brazilian scream that deals <style=cIsDamage>" + (DamageCoefficient * 100) + "% damage</style> plus <style=cIsDamage>" + (MaxHealthCoefficient * 100) + "% of your maximum health</style> to enemies within <style=cIsDamage>" + Radius + "m</style>.");
            Modules.Language.Add("EQUIPMENT_LUMPKIN_LORE", "\"Lumpkin, one of the many rebel commanders of WW19, possessed a scream that could deafen his oppenents. In many battles and skrimishes, he would often use this scream to ambush squads and slaughter them. He continued this practice until the final battle of WW19, when he had his left lung sforcibly ripped from his chest and eaten. \n\nHis lungs, pictured above, allowed him to scream loudly without injuring himself.\"\n\n-Exhibit at The National WW19 Museum");

            CreateEffect();

            On.RoR2.EquipmentSlot.PerformEquipmentAction += EquipmentSlot_PerformEquipmentAction;
        }

        private static void CreateEffect()
        {
            screamEffect = PrefabAPI.InstantiateClone(Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("LumpkinScreamEffect"), "LumpkinScreamEffect", false);

            EffectComponent effectComponent = screamEffect.AddComponent<EffectComponent>();
            effectComponent.applyScale = true;
            effectComponent.positionAtReferencedTransform = false;
            effectComponent.parentToReferencedTransform = false;

            VFXAttributes vfxAttributes = screamEffect.AddComponent<VFXAttributes>();
            vfxAttributes.vfxIntensity = VFXAttributes.VFXIntensity.Medium;
            vfxAttributes.vfxPriority = VFXAttributes.VFXPriority.Always;

            ShakeEmitter shakeEmitter = screamEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.wave = new Wave
            {
                amplitude = 0.7f,
                cycleOffset = 0f,
                frequency = 100f
            };
            shakeEmitter.duration = 2f;
            shakeEmitter.radius = Radius;
            shakeEmitter.amplitudeTimeDecay = true;

            screamEffect.AddComponent<DestroyOnTimer>().duration = 2f;

            ContentAddition.AddEffect(screamEffect);
        }

        private static bool EquipmentSlot_PerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentDef equipmentDef)
        {
            if (equipmentDef == lumpkinEquipment)
            {
                return Scream(self);
            }
            return orig(self, equipmentDef);
        }

        private static bool Scream(EquipmentSlot slot)
        {
            CharacterBody body = slot.characterBody;
            if (!body)
                return false;

            new BlastAttack
            {
                attacker = body.gameObject,
                inflictor = body.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = DamageCoefficient * body.damage + body.maxHealth * MaxHealthCoefficient,
                baseForce = 5000f,
                bonusForce = new Vector3(0f, 5000f, 0f),
                crit = false,
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.AOE,
                falloffModel = BlastAttack.FalloffModel.None,
                losType = BlastAttack.LoSType.NearestHit,
                position = body.transform.position,
                procChainMask = default(ProcChainMask),
                procCoefficient = 2f,
                radius = Radius,
                teamIndex = body.teamComponent.teamIndex
            }.Fire();

            EffectManager.SpawnEffect(screamEffect, new EffectData
            {
                origin = body.footPosition,
                scale = Radius
            }, true);

            return true;
        }

        private static ItemDisplayRuleDict CreateItemDisplayRules()
        {
            GameObject display = CreateItemDisplayPrefab(lumpkinEquipment.pickupModelPrefab, "DisplayLumpkin");

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", CreateDisplayRuleArray(display, "Chest", new Vector3(-0.1546f, 0.2269f, 0.3945f), new Vector3(2.1125f, 264.9913f, 92.1811f), Vector3.one));
            rules.Add("mdlHuntress", CreateDisplayRuleArray(display, "Chest", new Vector3(-0.06059f, 0.24714f, 0.32719f), new Vector3(355.0741f, 264.7103f, 99.27936f), Vector3.one));
            rules.Add("mdlToolbot", CreateDisplayRuleArray(display, "Chest", new Vector3(-0.4381f, 1.8153f, 4.3646f), new Vector3(3.4262f, 271.8089f, 104.9852f), new Vector3(8f, 8f, 8f)));
            rules.Add("mdlEngi", CreateDisplayRuleArray(display, "Chest", new Vector3(0.03356f, 0.36964f, 0.37491f), new Vector3(323.1482f, 238.5304f, 117.516f), Vector3.one));
            rules.Add("mdlMage", CreateDisplayRuleArray(display, "Chest", new Vector3(0.0531f, 0.0726f, 0.3056f), new Vector3(3.4891f, 290.111f, 61.6155f), Vector3.one));
            rules.Add("mdlMerc", CreateDisplayRuleArray(display, "Chest", new Vector3(0.0439f, 0.2073f, 0.3381f), new Vector3(359.9271f, 289.9793f, 85.8721f), Vector3.one));
            rules.Add("mdlLoader", CreateDisplayRuleArray(display, "MechHandR", new Vector3(-0.03233f, 0.27857f, -0.02345f), new Vector3(298.3183f, 204.5738f, 95.84402f), Vector3.one));
            rules.Add("mdlCroco", CreateDisplayRuleArray(display, "HandL", new Vector3(0.9815f, 1.4904f, -0.0826f), new Vector3(359.7866f, 89.432f, 70.9637f), new Vector3(9f, 9f, 9f)));
            rules.Add("mdlTreebot", CreateDisplayRuleArray(display, "MuzzleSyringe", new Vector3(-0.1103f, -0.2115f, -1.274f), new Vector3(357.233f, 271.4698f, 277.0092f), new Vector3(2f, 2f, 2f)));
            rules.Add("mdlCaptain", CreateDisplayRuleArray(display, "Head", new Vector3(0.3209f, 0.2037f, -0.2083f), new Vector3(331.4015f, 25.9899f, 115.6633f), Vector3.one));
            return rules;
        }
    }
}
