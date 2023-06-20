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

namespace Cloudburst.Characters.Wyatt
{
    public class WyattSurvivor : SurvivorBase<WyattSurvivor>
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
            
            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 120f,
            healthRegen = 2.5f,
            armor = 20f,

            jumpCount = 1,

            cameraParamsDepth = -10,
            cameraPivotPosition = new Vector3(0, 1, 0)
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

        public override Type characterMainState => typeof(GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new WyattItemDisplays();

        private static UnlockableDef masterySkinUnlockableDef;

        public static SerializableEntityStateType DeployMaidState = new SerializableEntityStateType(typeof(DeployMaid));

        public override void Initialize()
        {
            characterEnabledConfig =
                Config.BindAndOptions(
                    "Survivors",
                    "Enable Custodian",
                    true,
                    "Set false to disable Custodian and as much of his content/code as possible",
                    true);

            base.Initialize();
        }
        
        protected override void OnCharacterInitialized()
        {
            WyattConfig.Init();
            WyattEffects.OnLoaded();
            WyattAssets.InitAss();
            WyattDamageTypes.InitDamageTypes();
            WyattLanguageTokens.AddLanguageTokens(WYATT_PREFIX);
            WyattBuffs.Init();

            WyattEntityStates.AddEntityStates();

            bodyPrefab.AddComponent<WyattMAIDManager>();
            bodyPrefab.AddComponent<WyattNetworkCombat>();

            SfxLocator sfxLocator = bodyPrefab.GetComponent<SfxLocator>();
            //sfx
            sfxLocator.fallDamageSound = "Play_MULT_shift_hit";
            sfxLocator.landingSound = "play_char_land";

            //CharacterDeathBehavior characterDeathBehavior = wyattBody.GetComponent<CharacterDeathBehavior>();
            bodyPrefab.AddComponent<WyattWalkmanBehavior>();
            //kil
            //Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(DeathState));
            //characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(DeathState));

            GenericCursorEnemyTracker tracker = bodyPrefab.AddComponent<GenericCursorEnemyTracker>();

            GameObject indicatorPrefab = R2API.PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/EngiShieldRetractIndicator"), "WyattTrackerIndicator", false);
            SpriteRenderer indicator = indicatorPrefab.transform.Find("Holder").GetComponent<SpriteRenderer>(); //
            indicator.sprite = Assets.LoadAsset<Sprite>("texWyattIndicator");
            indicator.color = CCUtilities.HexToColor("00A86B");

            tracker.maxTrackingAngle = 20;
            tracker.maxTrackingDistance = 55;
            tracker.trackerUpdateFrequency = 5;
            tracker.indicatorPrefab = indicatorPrefab;

            AlterStatemachines();

            SetHooks();
        }

        #region hooks
        private void SetHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;

            On.RoR2.CharacterBody.OnBuffFinalStackLost += CharacterBody_OnBuffFinalStackLost;
            On.RoR2.CharacterBody.OnBuffFirstStackGained += CharacterBody_OnBuffFirstStackGained;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

            On.RoR2.Projectile.BoomerangProjectile.FixedUpdate += BoomerangProjectile_FixedUpdate;
        }

        private void BoomerangProjectile_FixedUpdate(On.RoR2.Projectile.BoomerangProjectile.orig_FixedUpdate orig, RoR2.Projectile.BoomerangProjectile self)
        {
            orig(self);
            //Log.Warning($"state: {self.boomerangState} newtork: {self.NetworkboomerangState}");
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            if(R2API.DamageAPI.HasModdedDamageType(damageInfo, WyattDamageTypes.antiGravDamage))
            {
                self.body.AddTimedBuff(WyattBuffs.wyattAntiGravBuffDef, WyattConfig.M1AntiGravDuration.Value);
            }
            if (R2API.DamageAPI.HasModdedDamageType(damageInfo, WyattDamageTypes.antiGravDamage2))
            {
                self.body.AddTimedBuff(WyattBuffs.wyattAntiGravBuffDef, WyattConfig.M4SlamAntiGravDuration.Value);
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(WyattBuffs.wyattGrooveBuffDef))
            {
                args.moveSpeedMultAdd += WyattConfig.M3GrooveSpeedMultiplierPerStack.Value * sender.GetBuffCount(WyattBuffs.wyattGrooveBuffDef);
            }

            if (sender.HasBuff(WyattBuffs.wyattFlowBuffDef))
            {
                args.armorAdd += WyattConfig.M3FlowArmorBase.Value + WyattConfig.M3FlowArmorPerStack.Value * sender.GetBuffCount(WyattBuffs.wyattGrooveBuffDef);
            }

            if (sender.HasBuff(WyattBuffs.wyattAntiGravBuffDef))
            {
                args.attackSpeedMultAdd -= 0.5f;
                args.moveSpeedMultAdd -= 0.5f;
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self && self.HasBuff(WyattBuffs.wyattFlowBuffDef))
            {
                self.maxJumpCount+= WyattConfig.M3FlowExtraJumps.Value;
            }

            //if (self & self.HasBuff(WyattBuffs.wyattAntiGravBuffDef))
            //{
            //    if (self.characterMotor)
            //    {
            //        self.characterMotor.useGravity = false;
            //    }
            //}
        }

        private void CharacterBody_OnBuffFinalStackLost(On.RoR2.CharacterBody.orig_OnBuffFinalStackLost orig, CharacterBody self, BuffDef buffDef)
        {
            orig(self, buffDef);
            if (buffDef == WyattBuffs.wyattAntiGravBuffDef)
            {
                if (self.characterMotor)
                {
                    self.characterMotor.useGravity = true;
                }
            }
        }

        private void CharacterBody_OnBuffFirstStackGained(On.RoR2.CharacterBody.orig_OnBuffFirstStackGained orig, CharacterBody self, BuffDef buffDef)
        {
            orig(self, buffDef);
            if (buffDef == WyattBuffs.wyattAntiGravBuffDef)
            {
                if (self.characterMotor)
                {
                    self.characterMotor.useGravity = false;
                }
            }
        }
#endregion hooks
        public void AlterStatemachines()
        {
            SetStateOnHurt setStateOnHurt = bodyPrefab.GetComponent<SetStateOnHurt>();
            NetworkStateMachine networkStateMachine = bodyPrefab.GetComponent<NetworkStateMachine>();

            EntityStateMachine maidMachine = bodyPrefab.AddComponent<EntityStateMachine>();
            maidMachine.customName = "MAID";
            maidMachine.initialStateType = new SerializableEntityStateType(typeof(Idle));
            maidMachine.mainStateType = new SerializableEntityStateType(typeof(Idle));

            int idleLength = setStateOnHurt.idleStateMachine.Length;
            Array.Resize(ref setStateOnHurt.idleStateMachine, idleLength + 1);
            setStateOnHurt.idleStateMachine[idleLength] = maidMachine;

            int networkStateMachinesLength = networkStateMachine.stateMachines.Length;
            Array.Resize(ref networkStateMachine.stateMachines, networkStateMachinesLength + 1);
            networkStateMachine.stateMachines[networkStateMachinesLength] = maidMachine;


            EntityStateMachine marioJumpMachine = bodyPrefab.AddComponent<EntityStateMachine>();
            marioJumpMachine.customName = "SuperMarioJump";
            marioJumpMachine.initialStateType = new SerializableEntityStateType(typeof(Idle));
            marioJumpMachine.mainStateType = new SerializableEntityStateType(typeof(Idle));

            idleLength = setStateOnHurt.idleStateMachine.Length;
            Array.Resize(ref setStateOnHurt.idleStateMachine, idleLength + 1);
            setStateOnHurt.idleStateMachine[idleLength] = marioJumpMachine;

            networkStateMachinesLength = networkStateMachine.stateMachines.Length;
            Array.Resize(ref networkStateMachine.stateMachines, networkStateMachinesLength + 1);
            networkStateMachine.stateMachines[networkStateMachinesLength] = marioJumpMachine;
        }

        public override void InitializeUnlockables()
        {
            WyattUnlockables.Init();
        }

        public override void InitializeHitboxes()
        {
            //set up on model

            //ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();

            //Prefabs.SetupHitbox(prefabCharacterModel.gameObject, childLocator.FindChild("TempHitbox"), "TempHitbox");
            //Prefabs.SetupHitbox(prefabCharacterModel.gameObject, childLocator.FindChild("TempHitboxLarge"), "TempHitboxLarge");
            //Prefabs.SetupHitbox(prefabCharacterModel.gameObject, childLocator.FindChild("TempHitboxSquish"), "TempHitboxSquish");
            //Prefabs.SetupHitbox(prefabCharacterModel.gameObject, childLocator.FindChild("TempHitboxLunge"), "TempHitboxLunge");
        }

        #region skills
        public override void InitializeSkills()
        {
            Skills.CreateSkillFamilies(bodyPrefab);

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
                //keywordToken = "KEYWORD_VELOCITY",
                icon = Assets.LoadAsset<Sprite>("texIconWyattPassive")
            };

            //R2API.LanguageAPI.Add(passive.keywordToken, "<style=cKeywordName>Groove</style><style=cSub>Increases movement speed by X%.</style>");
            R2API.LanguageAPI.Add(passive.skillNameToken, "Walkman");
            R2API.LanguageAPI.Add(passive.skillDescriptionToken, "On hit, gain a stack of <style=cIsUtility>Groove</style>, granting <style=cIsUtility>20% move speed</style> per stack. Diminishes out of combat.");

            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = passive;
        }

        private void InitializePrimarySkills()
        {
            //Creates a skilldef for a typical primary 
            SteppedSkillDef primarySkillDef = Skills.CreateSkillDef<SteppedSkillDef>(
                new SkillDefInfo(
                    "wyatt_primary_combo",
                    WYATT_PREFIX + "PRIMARY_COMBO_NAME",
                    WYATT_PREFIX + "PRIMARY_COMBO_DESCRIPTION",
                    Assets.LoadAsset<Sprite>("texIconWyattPrimary"),
                    new SerializableEntityStateType(typeof(WyattBaseMeleeAttack)),
                    "Weapon",
                    true));
            primarySkillDef.keywordTokens = new string[] {
                 "KEYWORD_AGILE",
                 "KEYWORD_WEIGHTLESS",
            };
            primarySkillDef.stepCount = 3;
            primarySkillDef.stepGraceDuration = 0.2f;

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
        }

        private void InitializeSecondarySkills()
        {
            GCETSkillDef secondarySkillDef = Skills.CreateSkillDef<GCETSkillDef>(new SkillDefInfo
            {
                skillName = "wyatt_secondary_trashout",
                skillNameToken = WYATT_PREFIX + "SECONDARY_TRASHOUT_NAME",
                skillDescriptionToken = WYATT_PREFIX + "SECONDARY_TRASHOUT_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texIconWyattSecondary"),
                activationState = new SerializableEntityStateType(typeof(TrashOut)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_SPIKED" }
            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef);
        }

        private void InitializeUtilitySkills()
        {
            SkillDef utilitySkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "wyatt_utility_flow",
                skillNameToken = WYATT_PREFIX + "UTILITY_FLOW_NAME",
                skillDescriptionToken = WYATT_PREFIX + "UTILITY_FLOW_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texIconWyattUtility"),
                activationState = new SerializableEntityStateType(typeof(ActivateFlow)),
                activationStateMachineName = "SuperMarioJump",
                baseMaxStock = 1,
                baseRechargeInterval = 14f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_FLOW" }
            });
            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef);
        }

        private void InitializeSpecialSkills()
        {
            WyattMAIDSkillDef specialSkillDef = Skills.CreateSkillDef<WyattMAIDSkillDef>(new SkillDefInfo
            {
                skillName = "wyatt_special_maid",
                skillNameToken = WYATT_PREFIX + "SPECIAL_MAID_NAME",
                skillDescriptionToken = WYATT_PREFIX + "SPECIAL_MAID_DESCRIPTION",
                skillIcon = Assets.LoadAsset<Sprite>("texIconWyattSpecial"),
                activationState = DeployMaidState,
                activationStateMachineName = "MAID",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 0, 
                keywordTokens = new string[] { "KEYWORD_WEIGHTLESS" }
            });
            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef);
        }
        #endregion skills
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                Assets.LoadAsset<Sprite>("texIconWyattSkinDefault"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "WyattMesh",
                "WyattBroom");

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin
            
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(WYATT_PREFIX + "CLASSIC_SKIN",
                Assets.LoadAsset<Sprite>("texIconWyattSkinClassic"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                WyattUnlockables.masteryUnlockable);

            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matWyattClassic");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matWyattClassic_Broom");

            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};

            skins.Add(masterySkin);
            
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}