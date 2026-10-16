using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static Cloudburst.Modules.ItemDisplays;

namespace Cloudburst.Items.Green
{
    internal class MechanicalTrinket
    {
        public static ItemDef mechanicalTrinketItem;

        public static float RadiusPerStack = 5;
        public static int MaxDrones = 4;
        public static float DroneLifetime = 120;

        public static GameObject droneMasterPrefab;

        public static void Setup()
        {
            mechanicalTrinketItem = ScriptableObject.CreateInstance<ItemDef>();
            mechanicalTrinketItem.deprecatedTier = ItemTier.Tier2;
            mechanicalTrinketItem.name = "itemmechanicaltrinket";
            mechanicalTrinketItem.nameToken = "ITEM_MECHANICALTRINKET_NAME";
            mechanicalTrinketItem.pickupToken = "ITEM_MECHANICALTRINKET_PICKUP";
            mechanicalTrinketItem.descriptionToken = "ITEM_MECHANICALTRINKET_DESCRIPTION";
            mechanicalTrinketItem.loreToken = "ITEM_MECHANICALTRINKET_LORE";
            mechanicalTrinketItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            mechanicalTrinketItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("Assets/Cloudburst/Items/MechanicalTrinket/IMDLMechanicalTrinket.prefab");
            mechanicalTrinketItem.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Assets/Cloudburst/Items/MechanicalTrinket/icon.png");
            mechanicalTrinketItem.tags = new ItemTag[]
            {
                ItemTag.CanBeTemporary,
                ItemTag.Utility,
                ItemTag.HoldoutZoneRelated,
                ItemTag.AIBlacklist
            };

            ItemAPI.Add(new CustomItem(mechanicalTrinketItem, CreateItemDisplayRules()));

            Modules.Language.Add("ITEM_MECHANICALTRINKET_NAME", "Mechanical Trinket");
            Modules.Language.Add("ITEM_MECHANICALTRINKET_PICKUP", "Increase the radius of any holdout zones and summon drones when the teleporter is activated.");
            Modules.Language.Add("ITEM_MECHANICALTRINKET_DESCRIPTION", "Increase the radius of all holdout zones by <style=cIsUtility>" + RadiusPerStack + "m</style> <style=cStack>(+" + RadiusPerStack + "m per stack)</style>. When the teleporter is activated, summon <style=cIsUtility>1</style> <style=cStack>(+1 per stack, up to " + MaxDrones + ")</style> drone for " + DroneLifetime + " seconds with <style=cIsDamage>100% <style=cStack>(+50% per stack)</style> damage</style> and <style=cIsHealing>100% <style=cStack>(+100% per stack)</style> health</style>.");
            Modules.Language.Add("ITEM_MECHANICALTRINKET_LORE", "");

            droneMasterPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/DroneBackupMaster");

            On.RoR2.HoldoutZoneController.Start += HoldoutZoneController_Start;
            TeleporterInteraction.onTeleporterBeginChargingGlobal += TeleporterInteraction_onTeleporterBeginChargingGlobal;
        }

        private static void HoldoutZoneController_Start(On.RoR2.HoldoutZoneController.orig_Start orig, HoldoutZoneController self)
        {
            self.gameObject.AddComponent<MechanicalTrinketRadiusController>();
            orig(self);
        }

        private static void TeleporterInteraction_onTeleporterBeginChargingGlobal(TeleporterInteraction teleporter)
        {
            if (!NetworkServer.active || !droneMasterPrefab || !Run.instance)
                return;

            int itemCount = Util.GetItemCountForTeam(TeamIndex.Player, mechanicalTrinketItem.itemIndex, false, true);
            if (itemCount <= 0)
                return;

            float healthMult = (1f + Run.instance.difficultyCoefficient / 2f) * itemCount;
            float damageMult = (1f + Run.instance.difficultyCoefficient / 8f) * Mathf.Sqrt(itemCount);

            float baseYaw = Quaternion.LookRotation(teleporter.transform.forward).eulerAngles.y;
            int droneCount = Mathf.Min(itemCount, MaxDrones);
            for (int i = 0; i < droneCount; i++)
            {
                float yaw = baseYaw + 10f + i * (360f / droneCount);
                Vector3 position = teleporter.transform.position + Quaternion.Euler(0f, yaw, 0f) * (Vector3.forward * 3f) + Vector3.up * 2f;

                CharacterMaster droneMaster = new MasterSummon
                {
                    masterPrefab = droneMasterPrefab,
                    position = position,
                    rotation = Quaternion.Euler(0f, yaw + 180f, 0f),
                    teamIndexOverride = TeamIndex.Player,
                    ignoreTeamMemberLimit = false,
                    useAmbientLevel = true
                }.Perform();

                if (droneMaster)
                {
                    droneMaster.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = DroneLifetime + UnityEngine.Random.Range(0f, 3f);
                    droneMaster.inventory.GiveItemPermanent(RoR2Content.Items.BoostHp, Mathf.RoundToInt((healthMult - 1f) * 10f));
                    droneMaster.inventory.GiveItemPermanent(RoR2Content.Items.BoostDamage, Mathf.RoundToInt((damageMult - 1f) * 10f));
                }
            }
        }

        private class MechanicalTrinketRadiusController : MonoBehaviour
        {
            private HoldoutZoneController holdoutZoneController;
            private int stackCount;

            private void Awake()
            {
                holdoutZoneController = GetComponent<HoldoutZoneController>();
            }

            private void OnEnable()
            {
                holdoutZoneController.calcRadius += ApplyRadius;
            }

            private void OnDisable()
            {
                holdoutZoneController.calcRadius -= ApplyRadius;
            }

            private void FixedUpdate()
            {
                stackCount = Util.GetItemCountForTeam(holdoutZoneController.chargingTeam, mechanicalTrinketItem.itemIndex, true, false);
            }

            private void ApplyRadius(ref float radius)
            {
                if (stackCount > 0)
                {
                    radius += stackCount * RadiusPerStack;
                }
            }
        }

        private static ItemDisplayRuleDict CreateItemDisplayRules()
        {
            GameObject display = CreateItemDisplayPrefab(mechanicalTrinketItem.pickupModelPrefab, "DisplayMechanicalTrinket");

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", CreateDisplayRuleArray(display, "Chest", new Vector3(0.1122f, 0.3168f, 0.212f), new Vector3(350.1537f, 0.0057f, 359.9348f), new Vector3(4f, 4f, 4f)));
            rules.Add("mdlHuntress", CreateDisplayRuleArray(display, "Chest", new Vector3(0.1122f, 0.2168f, 0.152f), new Vector3(350.1537f, 0.0057f, 359.9348f), new Vector3(3f, 3f, 3f)));
            rules.Add("mdlToolbot", CreateDisplayRuleArray(display, "Head", new Vector3(0.4493f, 3.1995f, -1.1082f), new Vector3(54.7524f, 359.9821f, 0.1173f), new Vector3(25f, 25f, 25f)));
            rules.Add("mdlEngi", CreateDisplayRuleArray(display, "CannonHeadR", new Vector3(-0.2175f, 0.2798f, -0.0528f), new Vector3(0.4107f, 271.4308f, 238.6879f), new Vector3(7f, 7f, 7f)));
            rules.Add("mdlMage", CreateDisplayRuleArray(display, "Chest", new Vector3(0.1244f, 0.1703f, 0.1248f), new Vector3(331.7253f, 358.0778f, 4.2768f), new Vector3(3f, 3f, 3f)));
            rules.Add("mdlMerc", CreateDisplayRuleArray(display, "Chest", new Vector3(0.0034f, 0.1911f, -0.2936f), new Vector3(13.9531f, 359.9117f, 359.09f), new Vector3(5f, 5f, 5f)));
            rules.Add("mdlLoader", CreateDisplayRuleArray(display, "MechLowerArmL", new Vector3(0.0018f, 0.6337f, 0.0048f), new Vector3(281.7989f, 159.6738f, 152.303f), new Vector3(10f, 10f, 10f)));
            rules.Add("mdlCroco", CreateDisplayRuleArray(display, "Chest", new Vector3(0f, 0.2f, 0.3f), new Vector3(-180f, 0f, 0f), new Vector3(3f, 2f, 3f)));
            rules.Add("mdlTreebot", CreateDisplayRuleArray(display, "Eye", new Vector3(0.0203f, 0.7649f, 0.0108f), new Vector3(270.5436f, 0f, 0f), new Vector3(12f, 12f, 12f)));
            rules.Add("mdlCaptain", CreateDisplayRuleArray(display, "Pelvis", new Vector3(-0.0545f, -0.1244f, -0.205f), new Vector3(345.5598f, 194.8679f, 174.2104f), new Vector3(4f, 4f, 4f)));
            return rules;
        }
    }
}
