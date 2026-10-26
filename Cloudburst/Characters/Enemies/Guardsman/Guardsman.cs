using System;
using System.Collections.Generic;
using Cloudburst.Enemies.Guardsman.EntityStates;
using EntityStates;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Navigation;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Enemies.Guardsman
{
    internal class Guardsman
    {
        public static GameObject bodyPrefab;
        public static CharacterBody bodyBody;
        public static GameObject masterPrefab;
        public static CharacterSpawnCard spawnCard;

        public static GameObject stompEffect;

        public static float ModelScale = 0.35f;
        public static float CapsuleHeight = 3.8f;
        public static float CapsuleRadius = 1.2f;

        public static void Setup()
        {
            ContentAddition.AddEntityState<GuardsmanSpawnState>(out _);
            ContentAddition.AddEntityState<Swing>(out _);
            ContentAddition.AddEntityState<Stomp>(out _);

            stompEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Parent/ParentSlamEffect.prefab").WaitForCompletion();

            CreateBody();
            Modules.Config.ConfigureBody(bodyBody, "Clay Guardsman");
            CreateSkills();
            CreateMaster();
            CreateSpawnCard();

            Modules.Language.Add("CLOUDBURST_GUARDSMAN_BODY_NAME", "Clay Guardsman");
            Modules.Language.Add("CLOUDBURST_GUARDSMAN_BODY_SUBTITLE", "");
            Modules.Language.Add("CLOUDBURST_GUARDSMAN_BODY_LORE", "");
            Modules.Language.Add("CLOUDBURST_GUARDSMAN_PRIMARY_NAME", "Halberd Swing");
            Modules.Language.Add("CLOUDBURST_GUARDSMAN_SECONDARY_NAME", "Stomp");
        }

        private static void CreateBody()
        {
            bodyPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Golem/GolemBody.prefab").WaitForCompletion(), "CloudburstGuardsmanBody", true);

            ModelLocator modelLocator = bodyPrefab.GetComponent<ModelLocator>();
            Transform modelBase = modelLocator.modelBaseTransform ? modelLocator.modelBaseTransform : modelLocator.modelTransform.parent;
            UnityEngine.Object.DestroyImmediate(modelLocator.modelTransform.gameObject);

            GameObject model = PrefabAPI.InstantiateClone(Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("mdlClayGuardsman"), "mdlCloudburstGuardsman", false);
            model.transform.SetParent(modelBase, false);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one * ModelScale;
            modelLocator.modelTransform = model.transform;
            modelLocator.modelBaseTransform = modelBase;

            KinematicCharacterController.KinematicCharacterMotor kinematicMotor = bodyPrefab.GetComponent<KinematicCharacterController.KinematicCharacterMotor>();
            kinematicMotor.Capsule = bodyPrefab.GetComponent<CapsuleCollider>();
            kinematicMotor.SetCapsuleDimensions(CapsuleRadius, CapsuleHeight, 0f);

            modelBase.localPosition = new Vector3(0f, -CapsuleHeight * 0.5f, 0f);
            Transform aimOrigin = bodyPrefab.GetComponent<CharacterBody>().aimOriginTransform;
            if (aimOrigin)
            {
                aimOrigin.localPosition = new Vector3(0f, CapsuleHeight * 0.3f, 0f);
            }

            Animator animator = model.GetComponent<Animator>();
            CharacterDirection direction = bodyPrefab.GetComponent<CharacterDirection>();
            direction.targetTransform = modelBase;
            direction.modelAnimator = animator;
            direction.rootMotionAccumulator = null;
            direction.driveFromRootRotation = false;

            bodyBody = bodyPrefab.GetComponent<CharacterBody>();
            bodyBody.baseNameToken = "CLOUDBURST_GUARDSMAN_BODY_NAME";
            bodyBody.subtitleNameToken = "CLOUDBURST_GUARDSMAN_BODY_SUBTITLE";
            bodyBody.baseMaxHealth = 400;
            bodyBody.levelMaxHealth = 120;
            bodyBody.baseRegen = 0.6f;
            bodyBody.levelRegen = 0.12f;
            bodyBody.baseArmor = 20;
            bodyBody.baseDamage = 15;
            bodyBody.levelDamage = 3;
            bodyBody.baseMoveSpeed = 10.5f;
            bodyBody.baseAcceleration *= 1.5f;
            bodyBody.baseAttackSpeed = 1.5f;
            bodyBody.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyBody.hullClassification = HullClassification.Golem;
            bodyBody.rootMotionInMainState = false;

            UnlockableDef logUnlockable = Modules.Unlockables.CreateUnlockableDef("Logs.CloudburstGuardsmanBody.0", "CLOUDBURST_GUARDSMAN_BODY_NAME", null);
            bodyPrefab.GetComponent<DeathRewards>().logUnlockableDef = logUnlockable;

            bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            EntityStateMachine bodyMachine = EntityStateMachine.FindByCustomName(bodyPrefab, "Body");
            bodyMachine.initialStateType = new SerializableEntityStateType(typeof(GuardsmanSpawnState));
            bodyMachine.mainStateType = new SerializableEntityStateType(typeof(GenericCharacterMain));

            bodyPrefab.AddComponent<RoR2.ExpansionManagement.ExpansionRequirementComponent>().requiredExpansion = Cloudburst.cloudburstExpansion;

            SetupModel(model, bodyBody);

            ContentAddition.AddBody(bodyPrefab);
        }

        private static void SetupModel(GameObject model, CharacterBody body)
        {
            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = body;
            characterModel.autoPopulateLightInfos = true;
            List<CharacterModel.RendererInfo> rendererInfos = new List<CharacterModel.RendererInfo>();
            foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>())
            {
                rendererInfos.Add(new CharacterModel.RendererInfo
                {
                    renderer = renderer,
                    defaultMaterial = renderer.sharedMaterial,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                });
            }
            characterModel.baseRendererInfos = rendererInfos.ToArray();

            HealthComponent healthComponent = bodyPrefab.GetComponent<HealthComponent>();
            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();
            HurtBox hurtBox = model.transform.Find("GameObject").gameObject.AddComponent<HurtBox>();
            hurtBox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            hurtBox.healthComponent = healthComponent;
            hurtBox.isBullseye = true;
            hurtBox.isSniperTarget = true;
            hurtBox.damageModifier = HurtBox.DamageModifier.Normal;
            hurtBox.hurtBoxGroup = hurtBoxGroup;
            hurtBox.indexInGroup = 0;
            hurtBoxGroup.hurtBoxes = new HurtBox[] { hurtBox };
            hurtBoxGroup.mainHurtBox = hurtBox;
            hurtBoxGroup.bullseyeCount = 1;

            Transform swingHitbox = model.transform.Find("HitboxSwing");
            UnityEngine.Object.DestroyImmediate(swingHitbox.GetComponent<Collider>());
            Modules.Prefabs.SetupHitbox(model, swingHitbox, "HitboxSwing");

            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.inputBank = bodyPrefab.GetComponent<InputBankTest>();
            aimAnimator.directionComponent = bodyPrefab.GetComponent<CharacterDirection>();
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.yawRangeMax = 90f;
            aimAnimator.yawRangeMin = -90f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;

            Transform logbookFocus = new GameObject("LogbookFocus").transform;
            logbookFocus.SetParent(model.transform, false);
            logbookFocus.localPosition = new Vector3(0f, 5.3f, 0f);
            Transform logbookCamera = new GameObject("LogbookCamera").transform;
            logbookCamera.SetParent(model.transform, false);
            logbookCamera.localPosition = new Vector3(0f, 6.5f, 22f);
            ModelPanelParameters modelPanelParameters = model.AddComponent<ModelPanelParameters>();
            modelPanelParameters.focusPointTransform = logbookFocus;
            modelPanelParameters.cameraPositionTransform = logbookCamera;
            modelPanelParameters.minDistance = 3f;
            modelPanelParameters.maxDistance = 12f;
        }

        private static void CreateSkills()
        {
            foreach (GenericSkill existing in bodyPrefab.GetComponents<GenericSkill>())
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }
            SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();
            skillLocator.primary = Modules.Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "Primary");
            skillLocator.secondary = Modules.Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "Secondary");
            skillLocator.utility = null;
            skillLocator.special = null;

            SkillDef swing = Modules.Skills.CreateSkillDef(new Modules.SkillDefInfo
            {
                skillName = "CloudburstGuardsmanSwing",
                skillNameToken = "CLOUDBURST_GUARDSMAN_PRIMARY_NAME",
                skillDescriptionToken = "",
                activationState = new SerializableEntityStateType(typeof(Swing)),
                activationStateMachineName = "Body",
                baseRechargeInterval = 2f,
                interruptPriority = InterruptPriority.Skill,
                cancelSprintingOnActivation = true,
            });
            Modules.Skills.AddPrimarySkills(bodyPrefab, swing);

            SkillDef stomp = Modules.Skills.CreateSkillDef(new Modules.SkillDefInfo
            {
                skillName = "CloudburstGuardsmanStomp",
                skillNameToken = "CLOUDBURST_GUARDSMAN_SECONDARY_NAME",
                skillDescriptionToken = "",
                activationState = new SerializableEntityStateType(typeof(Stomp)),
                activationStateMachineName = "Body",
                baseRechargeInterval = 4f,
                interruptPriority = InterruptPriority.Skill,
                cancelSprintingOnActivation = true,
            });
            Modules.Skills.AddSecondarySkills(bodyPrefab, stomp);
        }

        private static void CreateMaster()
        {
            masterPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Golem/GolemMaster.prefab").WaitForCompletion(), "CloudburstGuardsmanMaster", true);
            masterPrefab.GetComponent<CharacterMaster>().bodyPrefab = bodyPrefab;

            foreach (AISkillDriver driver in masterPrefab.GetComponents<AISkillDriver>())
            {
                UnityEngine.Object.DestroyImmediate(driver);
            }

            AISkillDriver stomp = masterPrefab.AddComponent<AISkillDriver>();
            stomp.customName = "Stomp";
            stomp.skillSlot = SkillSlot.Secondary;
            stomp.requireSkillReady = true;
            stomp.minDistance = 0f;
            stomp.maxDistance = Stomp.blastRadius * 0.6f;
            stomp.selectionRequiresTargetLoS = false;
            stomp.activationRequiresAimConfirmation = false;
            stomp.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            stomp.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            stomp.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            stomp.moveInputScale = 0f;

            AISkillDriver swing = masterPrefab.AddComponent<AISkillDriver>();
            swing.customName = "Swing";
            swing.skillSlot = SkillSlot.Primary;
            swing.requireSkillReady = true;
            swing.minDistance = 0f;
            swing.maxDistance = 9f;
            swing.selectionRequiresTargetLoS = true;
            swing.activationRequiresTargetLoS = true;
            swing.activationRequiresAimConfirmation = true;
            swing.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            swing.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            swing.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            swing.moveInputScale = 0.5f;

            AISkillDriver chase = masterPrefab.AddComponent<AISkillDriver>();
            chase.customName = "Chase";
            chase.skillSlot = SkillSlot.None;
            chase.minDistance = 0f;
            chase.maxDistance = float.PositiveInfinity;
            chase.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chase.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chase.aimType = AISkillDriver.AimType.AtMoveTarget;
            chase.moveInputScale = 1f;
            chase.shouldSprint = false;

            ContentAddition.AddMaster(masterPrefab);
        }

        private static void CreateSpawnCard()
        {
            spawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            spawnCard.name = "cscCloudburstGuardsman";
            spawnCard.prefab = masterPrefab;
            spawnCard.sendOverNetwork = true;
            spawnCard.hullSize = HullClassification.Golem;
            spawnCard.nodeGraphType = MapNodeGroup.GraphType.Ground;
            spawnCard.requiredFlags = NodeFlags.None;
            spawnCard.forbiddenFlags = NodeFlags.NoCharacterSpawn;
            spawnCard.directorCreditCost = 35;
            spawnCard.occupyPosition = false;
            spawnCard.loadout = new SerializableLoadout();
            spawnCard.noElites = false;
            spawnCard.forbiddenAsBoss = false;

            //same format and defaults as moff's clay men: "stagename" or "stagename - loop" for post-loop only
            string stageList = Modules.Config.BindAndOptions<string>("Clay Guardsman", "Spawn Stages",
                "goolake, ancientloft, wispgraveyard, sulfurpools, arena, itgoolake, itancientloft, village - loop, villagenight, habitat, habitatfall",
                "Comma separated list of internal stage names the Clay Guardsman can spawn on. Add \" - loop\" after a stage name to only spawn there after looping.", true).Value;

            HashSet<string> stages = new HashSet<string>();
            HashSet<string> loopStages = new HashSet<string>();
            foreach (string entry in stageList.Split(','))
            {
                string[] parts = entry.Split('-');
                string stageName = parts[0].Trim().ToLowerInvariant();
                if (stageName.Length == 0)
                    continue;

                if (parts.Length > 1 && parts[1].Trim().ToLowerInvariant() == "loop")
                {
                    loopStages.Add(stageName);
                }
                else
                {
                    stages.Add(stageName);
                }
            }

            AddToStages(stages, 0);
            //vanilla post-loop monsters use 5 stage completions too
            AddToStages(loopStages, 5);
        }

        private static void AddToStages(HashSet<string> stageNames, int minimumStageCompletions)
        {
            if (stageNames.Count == 0)
                return;

            DirectorAPI.DirectorCardHolder cardHolder = new DirectorAPI.DirectorCardHolder
            {
                Card = new DirectorCard
                {
                    spawnCard = spawnCard,
                    selectionWeight = 1,
                    spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                    preventOverhead = false,
                    minimumStageCompletions = minimumStageCompletions,
                },
                MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters,
            };

            //gives the vanilla internal name, or the scene name for modded and newer stages
            DirectorAPI.Helpers.AddNewMonsterToStagesWhere(cardHolder, false, stageInfo =>
                stageNames.Contains((stageInfo.ToInternalStageName() ?? "").ToLowerInvariant()));
        }
    }
}
