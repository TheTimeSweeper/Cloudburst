using System;
using System.Collections.Generic;
using Cloudburst.Characters.Engineer.EntityStates;
using EntityStates;
using EntityStates.Engi.EngiWeapon;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Characters.Engineer
{
    //old cloudburst engineer alts: Impact Grenades primary and TR27 Immolator Turret special
    internal static class EngineerContent
    {
        public static GameObject flameTurretBody;
        public static GameObject flameTurretMaster;
        public static GameObject impactGrenadeProjectile;

        public static SkillDef impactGrenadesSkillDef;
        public static SkillDef flameTurretSkillDef;

        public static int BonusTurretLimit = 2;

        private static GameObject engiBody;
        private static GameObject walkerTurretBody;
        private static GameObject vanillaGrenadeProjectile;

        public static void Setup()
        {
            engiBody = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiBody.prefab").WaitForCompletion();
            walkerTurretBody = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiWalkerTurretBody.prefab").WaitForCompletion();
            GameObject walkerTurretMaster = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiWalkerTurretMaster.prefab").WaitForCompletion();
            vanillaGrenadeProjectile = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiGrenadeProjectile.prefab").WaitForCompletion();

            if (!engiBody || !walkerTurretBody || !walkerTurretMaster || !vanillaGrenadeProjectile)
            {
                Log.Error("Could not load engineer prefabs, skipping engineer skills");
                return;
            }

            ContentAddition.AddEntityState<FireVolley>(out _);
            ContentAddition.AddEntityState<PlaceFlameTurret>(out _);
            ContentAddition.AddEntityState<FireFlameThrower>(out _);
            ContentAddition.AddEntityState<FlameTurretDeathState>(out _);

            CreateFlameTurret(walkerTurretMaster);
            CreateImpactGrenadeProjectile();
            CreateSkills();

            try
            {
                AddSkinReplacements();
            }
            catch (Exception e)
            {
                Log.Warning("Could not add engineer skin replacements for flame turrets and impact grenades. They will use default visuals on alternate skins.\n" + e);
            }

            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
        }

        #region flame turret
        private static void CreateFlameTurret(GameObject walkerTurretMaster)
        {
            flameTurretBody = PrefabAPI.InstantiateClone(walkerTurretBody, "EngiFlameTurretBody", true);
            flameTurretMaster = PrefabAPI.InstantiateClone(walkerTurretMaster, "EngiFlameTurretMaster", true);

            flameTurretMaster.GetComponent<CharacterMaster>().bodyPrefab = flameTurretBody;

            CharacterBody body = flameTurretBody.GetComponent<CharacterBody>();
            body.baseMaxHealth = 70f;
            body.levelMaxHealth = 21f;
            body.baseDamage = 8f;
            body.levelDamage = 1.6f;

            flameTurretBody.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(FlameTurretDeathState));

            SkillLocator skillLocator = flameTurretBody.GetComponent<SkillLocator>();
            SkillDef beamSkillDef = skillLocator.primary.skillFamily.variants[0].skillDef;

            SkillDef flameSkillDef = UnityEngine.Object.Instantiate(beamSkillDef);
            (flameSkillDef as ScriptableObject).name = "EngiFlameTurretFireFlamethrower";
            flameSkillDef.activationState = new SerializableEntityStateType(typeof(FireFlameThrower));
            ContentAddition.AddSkillDef(flameSkillDef);

            SkillFamily flameFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (flameFamily as ScriptableObject).name = "EngiFlameTurretPrimaryFamily";
            flameFamily.variants = new SkillFamily.Variant[0];
            Modules.Skills.AddSkillToFamily(flameFamily, flameSkillDef);
            ContentAddition.AddSkillFamily(flameFamily);

            skillLocator.primary._skillFamily = flameFamily;

            ContentAddition.AddBody(flameTurretBody);
            ContentAddition.AddMaster(flameTurretMaster);
        }

        private static int CharacterMaster_GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot)
        {
            int limit = orig(self, slot);

            if (slot == DeployableSlot.EngiTurret)
            {
                CharacterBody body = self.GetBody();
                if (body && body.skillLocator && body.skillLocator.special && body.skillLocator.special.skillDef == flameTurretSkillDef)
                {
                    limit += BonusTurretLimit;
                }
            }

            return limit;
        }
        #endregion

        private static void CreateImpactGrenadeProjectile()
        {
            impactGrenadeProjectile = PrefabAPI.InstantiateClone(vanillaGrenadeProjectile, "EngiGrenadeImpactProjectile", true);
            impactGrenadeProjectile.GetComponent<ProjectileImpactExplosion>().destroyOnWorld = true;

            ProjectileStickOnImpact stick = impactGrenadeProjectile.GetComponent<ProjectileStickOnImpact>();
            if (stick)
            {
                UnityEngine.Object.DestroyImmediate(stick);
            }

            ContentAddition.AddProjectile(impactGrenadeProjectile);
        }

        private static void CreateSkills()
        {
            SkillLocator engiSkillLocator = engiBody.GetComponent<SkillLocator>();

            #region primary
            SkillDef grenadeSkillDef = engiSkillLocator.primary.skillFamily.variants[0].skillDef;

            impactGrenadesSkillDef = UnityEngine.Object.Instantiate(grenadeSkillDef);
            (impactGrenadesSkillDef as ScriptableObject).name = "EngiBodyFireImpactGrenades";
            impactGrenadesSkillDef.skillName = "EngiImpactGrenades";
            impactGrenadesSkillDef.skillNameToken = "ENGI_PRIMARY_CLOUDBURST_IMPACTGRENADES_NAME";
            impactGrenadesSkillDef.skillDescriptionToken = "ENGI_PRIMARY_CLOUDBURST_IMPACTGRENADES_DESCRIPTION";
            impactGrenadesSkillDef.icon = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("Impact_Grenades");
            impactGrenadesSkillDef.activationState = new SerializableEntityStateType(typeof(FireVolley));
            impactGrenadesSkillDef.canceledFromSprinting = false;
            impactGrenadesSkillDef.keywordTokens = Array.Empty<string>();
            ContentAddition.AddSkillDef(impactGrenadesSkillDef);

            Modules.Skills.AddSkillToFamily(engiSkillLocator.primary.skillFamily, impactGrenadesSkillDef);

            Modules.Language.Add("ENGI_PRIMARY_CLOUDBURST_IMPACTGRENADES_NAME", "Impact Grenades");
            Modules.Language.Add("ENGI_PRIMARY_CLOUDBURST_IMPACTGRENADES_DESCRIPTION", "Fire an impact grenade from each cannon that explodes on contact for <style=cIsDamage>100% damage</style>.");
            #endregion

            #region special
            SkillDef walkerSkillDef = null;
            foreach (SkillFamily.Variant variant in engiSkillLocator.special.skillFamily.variants)
            {
                if (variant.skillDef && variant.skillDef.activationState.stateType == typeof(PlaceWalkerTurret))
                {
                    walkerSkillDef = variant.skillDef;
                    break;
                }
            }
            if (walkerSkillDef == null)
            {
                walkerSkillDef = engiSkillLocator.special.skillFamily.variants[engiSkillLocator.special.skillFamily.variants.Length - 1].skillDef;
            }

            flameTurretSkillDef = UnityEngine.Object.Instantiate(walkerSkillDef);
            (flameTurretSkillDef as ScriptableObject).name = "EngiBodyPlaceFlameTurret";
            flameTurretSkillDef.skillName = "EngiPlaceFlameTurret";
            flameTurretSkillDef.skillNameToken = "ENGI_SPECIAL_CLOUDBURST_FLAMETURRET_NAME";
            flameTurretSkillDef.skillDescriptionToken = "ENGI_SPECIAL_CLOUDBURST_FLAMETURRET_DESCRIPTION";
            flameTurretSkillDef.icon = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("TR75_Immolator_Turret_2");
            flameTurretSkillDef.activationState = new SerializableEntityStateType(typeof(PlaceFlameTurret));
            flameTurretSkillDef.baseRechargeInterval = 15;
            flameTurretSkillDef.baseMaxStock = 2 + BonusTurretLimit;
            ContentAddition.AddSkillDef(flameTurretSkillDef);

            Modules.Skills.AddSkillToFamily(engiSkillLocator.special.skillFamily, flameTurretSkillDef);

            Modules.Language.Add("ENGI_SPECIAL_CLOUDBURST_FLAMETURRET_NAME", "TR27 Immolator Turret");
            Modules.Language.Add("ENGI_SPECIAL_CLOUDBURST_FLAMETURRET_DESCRIPTION", "Place a <style=cIsUtility>mobile</style> turret that <style=cIsUtility>inherits all your items.</style> Fires a plume of flame for <style=cIsDamage>" + (FireFlameThrower.damageCoefficient * 100) + "% damage per second</style> that <style=cIsDamage>ignites enemies</style>. <style=cIsDamage>Explodes on death</style> for <style=cIsDamage>" + (FlameTurretDeathState.healthDamageCoefficient * 100) + "%</style> of its maximum health. Can place up to " + (2 + BonusTurretLimit) + ".");
            #endregion
        }

        #region skins
        //make flame turrets and impact grenades follow engi's skins the same way the vanilla walker turret and grenades do
        private static void AddSkinReplacements()
        {
            ModelSkinController skinController = engiBody.GetComponentInChildren<ModelSkinController>();
            if (!skinController)
                return;

            foreach (SkinDef skin in skinController.skins)
            {
                if (!skin)
                    continue;

                HashSet<SkinDefParams> patchedParams = new HashSet<SkinDefParams>();
                PatchSkinParams(LoadSkinParams(skin.skinDefParams, skin.skinDefParamsAddress), patchedParams);
                PatchSkinParams(LoadSkinParams(skin.optimizedSkinDefParams, skin.optimizedSkinDefParamsAddress), patchedParams);
            }
        }

        private static SkinDefParams LoadSkinParams(SkinDefParams directReference, AssetReferenceT<SkinDefParams> address)
        {
            if (directReference)
                return directReference;

            if (address != null && address.RuntimeKeyIsValid())
                return Addressables.LoadAssetAsync<SkinDefParams>(address.RuntimeKey).WaitForCompletion();

            return null;
        }

        private static void PatchSkinParams(SkinDefParams skinParams, HashSet<SkinDefParams> patchedParams)
        {
            if (!skinParams || patchedParams.Contains(skinParams))
                return;
            patchedParams.Add(skinParams);

            if (skinParams.projectileGhostReplacements != null)
            {
                for (int i = 0; i < skinParams.projectileGhostReplacements.Length; i++)
                {
                    SkinDefParams.ProjectileGhostReplacement replacement = skinParams.projectileGhostReplacements[i];
                    if (replacement.projectilePrefab == vanillaGrenadeProjectile)
                    {
                        replacement.projectilePrefab = impactGrenadeProjectile;
                        HG.ArrayUtils.ArrayAppend(ref skinParams.projectileGhostReplacements, in replacement);
                        break;
                    }
                }
            }

            if (skinParams.minionSkinReplacements != null)
            {
                for (int i = 0; i < skinParams.minionSkinReplacements.Length; i++)
                {
                    SkinDefParams.MinionSkinReplacement replacement = skinParams.minionSkinReplacements[i];
                    if (replacement.minionBodyPrefab == walkerTurretBody)
                    {
                        replacement.minionBodyPrefab = flameTurretBody;
                        HG.ArrayUtils.ArrayAppend(ref skinParams.minionSkinReplacements, in replacement);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
