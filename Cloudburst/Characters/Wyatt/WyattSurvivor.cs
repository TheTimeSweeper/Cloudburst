using BepInEx.Configuration;
using Cloudburst.Wyatt.Components;
using Cloudburst.GlobalComponents;
using Cloudburst.Modules;
using Cloudburst.Modules.Characters;
using Cloudburst.Modules.Survivors;
using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using Cloudburst.CEntityStates.Wyatt;

namespace Cloudburst.Characters
{
    internal class WyattSurvivor : SurvivorBase<WyattSurvivor>
    {
        public override string prefabBodyName => "Wyatt";

        public const string WYATT_PREFIX = "CLOUDBURST_WYATT_";
        public override string survivorTokenPrefix => WYATT_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "WyattBody",
            bodyNameToken = WYATT_PREFIX + "NAME",
            subtitleNameToken = WYATT_PREFIX + "SUBTITLE",
            
            characterPortrait = Assets.LoadAsset<Texture>("texIconWyatt"),
            bodyColor = Color.white,

            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[] 
        {
                new CustomRendererInfo
                {
                    childName = "WyattMesh",
                },
                new CustomRendererInfo
                {
                    childName = "WyattBroom",
                }
        };

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new WyattItemDisplays();

        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public BuffDef wyattCombatBuffDef;
        public BuffDef wyattFlowBuffDef;

        public static SkillDef throwPrimarySkillDef;

        public static SerializableEntityStateType RetrieveMaidState = new SerializableEntityStateType(typeof(RetrieveMaid));
        public static SerializableEntityStateType DeployMaidState = new SerializableEntityStateType(typeof(DeployMaid));


        public override void Initialize()
        {
            base.Initialize();

            CreateBuffs();

            AddBodyLanguageTokens();

            MAIDManager janniePower = bodyPrefab.AddComponent<MAIDManager>();

            SfxLocator sfxLocator = bodyPrefab.GetComponent<SfxLocator>();
            //sfx
            sfxLocator.fallDamageSound = "Play_MULT_shift_hit";
            sfxLocator.landingSound = "play_char_land";

            //CharacterDeathBehavior characterDeathBehavior = wyattBody.GetComponent<CharacterDeathBehavior>();
            bodyPrefab.AddComponent<CustodianWalkmanBehavior>();
            //kil
            //Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(DeathState));
            //characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(DeathState));

            GenericCursorEnemyTracker tracker = bodyPrefab.AddComponent<GenericCursorEnemyTracker>();

            GameObject indicatorPrefab = R2API.PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/EngiShieldRetractIndicator"), "WyattTrackerIndicator", false);
            SpriteRenderer indicator = indicatorPrefab.transform.Find("Holder").GetComponent<SpriteRenderer>(); //
            indicator.sprite = Modules.Assets.LoadAsset<Sprite>("texWyattIndicator");
            indicator.color = CCUtilities.HexToColor("00A86B");

            tracker.maxTrackingAngle = 20;
            tracker.maxTrackingDistance = 55;
            tracker.trackerUpdateFrequency = 5;
            tracker.indicatorPrefab = indicatorPrefab;

            AlterStatemachines();

        }

        private static void AddBodyLanguageTokens()
        {
            R2API.LanguageAPI.Add(WYATT_PREFIX + "NAME", "Custodian");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "SUBTITLE", "Lean, Mean, Cleaning Machines");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "OUTRO_FLAVOR", "...and so they left, a job well done");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "OUTRO_FAILURE", "...and so they vanished, leaving a bigger mess than when they started");

            R2API.LanguageAPI.Add(WYATT_PREFIX + "DESCRIPTION", 
                "The Custodian is a master of janitorial warfare who uses his MAID to control the battlefield<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
                + "< ! > Send enemies upwards with the MAID, and spike them downwads with Trash Out for major damage." + Environment.NewLine + Environment.NewLine
                + "< ! > The MAID slows projectiles within her radius, use this to your advantage in combat!" + Environment.NewLine + Environment.NewLine
                + "< ! > this skill no longer exists" + Environment.NewLine + Environment.NewLine
                + "< ! > The key to success is realizing that staying away from the ground helps you stay alive longer." + Environment.NewLine + Environment.NewLine);

            R2API.LanguageAPI.Add(WYATT_PREFIX + "LORE", @"Can't stop now. Can't stop now. Every step I take is a step I can't take back. Come hell or high water I will find it.

It's all a rhythm, just a rhythm. Every time I step out of line is a punishment. I will obey the groove. Nothing can stop me now.

Every scar is worth it. I can feel it, I am coming closer to it. I will have it, and it will be mine.

No matter the blood, it's worth it. I will find what I want, and I will come home.

No matter how many I slaughter, it will be mine... It can't hide from me from me forever.

It's here, I can feel it. This security chest, it has it. I crack it open, and I find it.

It's... finally mine. I hold it in my bruised hands. Has it really been years? 

She'll love this, I know.

");
        }

        private void CreateBuffs()
        {
            wyattCombatBuffDef = Modules.Buffs.AddNewBuff(
                "CloudburstWyattCombatBuff",
                Modules.Assets.LoadAsset<Sprite>("WyattVelocity"),
                new Color(1f, 0.7882353f, 0.05490196f),
                true,
                false);

            wyattFlowBuffDef = Modules.Buffs.AddNewBuff(
                "CloudburstWyattFlowBuff",
                Modules.Assets.LoadAsset<Sprite>("WyattVelocity"),
                CCUtilities.HexToColor("#37323e"),
                false,
                false);
        }

        public void AlterStatemachines()
        {
            SetStateOnHurt setStateOnHurt = bodyPrefab.GetComponent<SetStateOnHurt>();
            NetworkStateMachine networkStateMachine = bodyPrefab.GetComponent<NetworkStateMachine>();

            EntityStateMachine maidMachine = bodyPrefab.AddComponent<EntityStateMachine>();
            maidMachine.customName = "MAID";
            maidMachine.initialStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));
            maidMachine.mainStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));

            int idleLength = setStateOnHurt.idleStateMachine.Length;
            Array.Resize<EntityStateMachine>(ref setStateOnHurt.idleStateMachine, idleLength + 1);
            setStateOnHurt.idleStateMachine[idleLength] = maidMachine;

            int networkStateMachinesLength = networkStateMachine.stateMachines.Length;
            Array.Resize<EntityStateMachine>(ref networkStateMachine.stateMachines, networkStateMachinesLength + 1);
            networkStateMachine.stateMachines[networkStateMachinesLength] = maidMachine;


            EntityStateMachine marioJumpMachine = bodyPrefab.AddComponent<EntityStateMachine>();
            marioJumpMachine.customName = "SuperMarioJump";
            marioJumpMachine.initialStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));
            marioJumpMachine.mainStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));

            idleLength = setStateOnHurt.idleStateMachine.Length;
            Array.Resize<EntityStateMachine>(ref setStateOnHurt.idleStateMachine, idleLength + 1);
            setStateOnHurt.idleStateMachine[idleLength] = marioJumpMachine;

            networkStateMachinesLength = networkStateMachine.stateMachines.Length;
            Array.Resize<EntityStateMachine>(ref networkStateMachine.stateMachines, networkStateMachinesLength + 1);
            networkStateMachine.stateMachines[networkStateMachinesLength] = marioJumpMachine;
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();

            Modules.Prefabs.SetupHitbox(prefabCharacterModel.gameObject, childLocator.FindChild("TempHitbox"), "TempHitbox");
            Modules.Prefabs.SetupHitbox(prefabCharacterModel.gameObject, childLocator.FindChild("TempHitboxLarge"), "TempHitboxLarge");
            Modules.Prefabs.SetupHitbox(prefabCharacterModel.gameObject, childLocator.FindChild("TempHitboxSquish"), "TempHitboxSquish");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            InitializePassive();

            InitializePrimarySkills();

            InitializeSecondarySkills();

            InitializeUtilitySkills();

            InitializeSpecialSkills();
        }

        private void InitializePassive()
        {
            SkillLocator.PassiveSkill passive = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = "WYATT_PASSIVE_NAME",
                skillDescriptionToken = "WYATT_PASSIVE_DESCRIPTION",
                keywordToken = "KEYWORD_VELOCITY",
                icon = Modules.Assets.LoadAsset<Sprite>("texIconWyattPassive")
            };

            R2API.LanguageAPI.Add(passive.keywordToken, "<style=cKeywordName>Groove</style><style=cSub>Increases movement speed by X%.</style>");
            R2API.LanguageAPI.Add(passive.skillNameToken, "Walkman");
            R2API.LanguageAPI.Add(passive.skillDescriptionToken, "On hit, gain a stack Groove. Lose 2 stacks of Groove every 0.5 seconds after being out of combat for 3 seconds. Groove grants 30% move speed and 25% damage.");

            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = passive;
        }

        private void InitializePrimarySkills()
        {
            //Creates a skilldef for a typical primary 
            SteppedSkillDef primarySkillDef = Modules.Skills.CreateSkillDef<SteppedSkillDef>(
                new SkillDefInfo(
                    "wyatt_primary_combo",
                    WYATT_PREFIX + "PRIMARY_COMBO_NAME",
                    WYATT_PREFIX + "PRIMARY_COMBO_DESCRIPTION",
                    Modules.Assets.LoadAsset<Sprite>("texIconWyattPrimary"),
                    new EntityStates.SerializableEntityStateType(typeof(WyattBaseMeleeAttack)),
                    "Weapon",
                    true));
            primarySkillDef.keywordTokens = new string[] {
                 "KEYWORD_AGILE",
                 "KEYWORD_WEIGHTLESS",
                 "KEYWORD_SPIKED",
            };
            primarySkillDef.stepCount = 2;
            primarySkillDef.stepGraceDuration = 1;

            R2API.LanguageAPI.Add(primarySkillDef.skillNameToken, "G22 Grav-Broom");
            R2API.LanguageAPI.Add(primarySkillDef.skillDescriptionToken, "<style=cIsUtility>Agile</style>. Swing in front for X% damage. [NOT IMPLEMENTED] Every 4th hit <style=cIsDamage>Spikes</style>.");
            //R2API.LanguageAPI.Add(primarySkillDef.keywordTokens[1], "<style=cKeywordName>Weightless</style><style=cSub>Slows and removes gravity from target.</style>");
            R2API.LanguageAPI.Add(primarySkillDef.keywordTokens[2], "<style=cKeywordName>Spiking</style><style=cSub>Forces an enemy to travel downwards, causing a shockwave if they impact terrain.</style>");

            Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
        }

        private void InitializeSecondarySkills()
        {
            GCETSkillDef secondarySkillDef = Modules.Skills.CreateSkillDef<GCETSkillDef>(new SkillDefInfo
            {
                skillName = "wyatt_secondary_trashout",
                skillNameToken = WYATT_PREFIX + "SECONDARY_TRASHOUT_NAME",
                skillDescriptionToken = WYATT_PREFIX + "SECONDARY_TRASHOUT_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texIconWyattSecondary"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(TrashOut)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 3f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_SPIKED" }
            });

            R2API.LanguageAPI.Add(secondarySkillDef.skillNameToken, "Trash Out");
            R2API.LanguageAPI.Add(secondarySkillDef.skillDescriptionToken, "Deploy a winch that reels you towards an enemy, and <style=cIsDamage>Spike</style> for <style=cIsDamage>X%</style>.");
            
            Modules.Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef);
        }

        private void InitializeUtilitySkills()
        {
            SkillDef utilitySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "wyatt_utility_flow",
                skillNameToken = WYATT_PREFIX + "UTILITY_FLOW_NAME",
                skillDescriptionToken = WYATT_PREFIX + "UTILITY_FLOW_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texIconWyattUtility"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ActivateFlow)),
                activationStateMachineName = "SuperMarioJump",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] {
                 "KEYWORD_RUPTURE",
                }
            });

            R2API.LanguageAPI.Add(utilitySkillDef.skillNameToken, "Flow");
            R2API.LanguageAPI.Add(utilitySkillDef.skillDescriptionToken, "Idk if this even works rn tbh.\nActivate Flow for 4 seconds (0.4s for each stack of Groove, max 8 seconds). During flow, you are unable to lose or gain Groove. After Flow ends, lose all stacks groove.");
            R2API.LanguageAPI.Add("KEYWORD_RUPTURE", "<style=cKeywordName>Flow</style><style=cSub> Gives you a double jump. +30% cooldown reduction.</style>");

            Modules.Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef);
        }

        private void InitializeSpecialSkills()
        {
            WyattMAIDSkillDef specialSkillDef = Modules.Skills.CreateSkillDef<WyattMAIDSkillDef>(new SkillDefInfo
            {
                skillName = "wyatt_special_maid",
                skillNameToken = WYATT_PREFIX + "SPECIAL_MAID_NAME",
                skillDescriptionToken = WYATT_PREFIX + "SPECIAL_MAID_DESCRIPTION",
                skillIcon = Modules.Assets.LoadAsset<Sprite>("texIconWyattSpecial"),
                activationState = DeployMaidState,
                activationStateMachineName = "MAID",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0,
                //keywordTokens = new string[]
                //{
                //    "KEYWORD_WEIGHTLESS"
                //}
            });

            R2API.LanguageAPI.Add(specialSkillDef.skillNameToken, "M88 MAID");
            R2API.LanguageAPI.Add(specialSkillDef.skillDescriptionToken, "Send your MAID unit barreling through enemies for 500% damage before stopping briefly and returning to you, able to hit enemies on the way back. Using this skill again while MAID is deployed reels you to the MAID and rebounds you off of her, bashing into an enemy for X% damage.");

            throwPrimarySkillDef = specialSkillDef;

            Modules.Skills.AddSpecialSkills(bodyPrefab, specialSkillDef);
        }

        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(WYATT_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.LoadAsset<Sprite>("texIconWyattSkinDefault"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion
            
            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            //creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(HenryPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshHenrySwordAlt",
                null,//no gun mesh replacement. use same gun mesh
                "meshHenryAlt");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GunModel"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}