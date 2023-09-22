using Cloudburst.Modules;
using Cloudburst.Modules.Characters;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace Cloudburst.Characters
{
    internal class WyattItemDisplays : ItemDisplaysBase
    {
/* for custom copy format in KEB's IDPH
                    {childName},
                    {localPos}, 
                    {localAngles},
                    {localScale})
*/
        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules)
        {
            #region displyas

            #region items
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.AlienHead,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAlienHead"),
                    "Pelvis",
                    new Vector3(-0.20154F, 0.00001F, -0.01504F),
                    new Vector3(80.01421F, 64.6179F, 11.08477F),
                    new Vector3(0.91884F, 0.91884F, 0.91884F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorPlate,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                    "ThighR",
                    new Vector3(0.05387F, 0.50659F, -0.02144F),
                    new Vector3(84.51842F, 137.1935F, 231.9463F),
                    new Vector3(0.31405F, 0.24756F, 0.26171F)
            )));
            //hide broom bristles?
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorReductionOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarhammer"),
                    "BroomHead",
                    new Vector3(-0.00001F, 0.07884F, 0.05286F),
                    new Vector3(270F, 0.00001F, 0F),
                    new Vector3(0.46663F, 0.46663F, 0.46663F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.AttackSpeedAndMoveSpeed,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCoffee"),
                    "Pelvis",
                    new Vector3(0.20537F, -0.00842F, -0.03923F),
                    new Vector3(0.08944F, 90.4156F, 167.8554F),
                    new Vector3(0.16623F, 0.16623F, 0.16623F)
            )));
            //hide hat?
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.AttackSpeedOnCrit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWolfPelt"),
                    "Head",
                    new Vector3(0.00245F, 0.12131F, 0.14615F),
                    new Vector3(285.1029F, 4.31383F, 175.262F),
                    new Vector3(0.37554F, 0.37554F, 0.37554F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.AutoCastEquipment,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFossil"),
                    "Pelvis",
                    new Vector3(-0.08822F, 0.18287F, 0.19673F),
                    new Vector3(284.0815F, 274.2758F, 352.4556F),
                    new Vector3(0.45317F, 0.45317F, 0.45317F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Bandolier,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBandolier"),
                    "Chest",
                    new Vector3(0.03929F, 0.1082F, 0.00438F),
                    new Vector3(319.1221F, 259.9634F, 105.1327F),
                    new Vector3(0.64966F, 0.76812F, 0.76812F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BarrierOnKill,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrooch"),
                    "Chest",
                    new Vector3(-0.16788F, 0.17222F, 0.19055F),
                    new Vector3(77.29682F, 94.19226F, 107.1558F),
                    new Vector3(0.58362F, 0.58362F, 0.58362F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BarrierOnOverHeal,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAegis"),
                    "LowerArmL",
                    new Vector3(0.05252F, 0.13307F, 0.06527F),
                    new Vector3(5.53522F, 316.651F, 93.15971F),
                    new Vector3(0.21542F, 0.21542F, 0.21542F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Bear,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBear"),
                    "Chest",
                    new Vector3(-0.13076F, 0.32361F, -0.12823F),
                    new Vector3(332.5732F, 203.4498F, 341.4774F),
                    new Vector3(0.19511F, 0.19511F, 0.19511F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BearVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBearVoid"),
                    "Chest",
                    new Vector3(-0.13076F, 0.32361F, -0.12823F),
                    new Vector3(332.5732F, 203.4498F, 341.4774F),
                    new Vector3(0.19511F, 0.19511F, 0.19511F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BeetleGland,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeetleGland"),
                    "Pelvis",
                    new Vector3(-0.16358F, 0.07017F, -0.14638F),
                    new Vector3(21.36368F, 207.6693F, 140.8712F),
                    new Vector3(0.07988F, 0.07988F, 0.07988F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Behemoth,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBehemoth"),
                    "BroomHead",
                    new Vector3(-0.20712F, 0.64541F, 0F),
                    new Vector3(270F, 90F, 0F),
                    new Vector3(0.17072F, 0.11016F, 0.24258F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTip"),
                    "Chest",
                    new Vector3(0.21187F, 0.39631F, -0.0829F),
                    new Vector3(60.14054F, 291.3688F, 294.283F),
                    new Vector3(0.50974F, 0.50974F, 0.50974F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BleedOnHitVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
                    "Chest",
                    new Vector3(0.21187F, 0.39631F, -0.0829F),
                    new Vector3(60.14054F, 291.3688F, 294.283F),
                    new Vector3(0.50974F, 0.50974F, 0.50974F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHitAndExplode,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                    "Stomach",
                    new Vector3(0.07236F, 0.11178F, 0.19605F),
                    new Vector3(0F, 0F, 295.5077F),
                    new Vector3(0.07089F, 0.07089F, 0.07089F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BonusGoldPackOnKill,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTome"),
                    "ThighR",
                    new Vector3(-0.00001F, 0.18149F, 0.09468F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.06862F, 0.06862F, 0.06862F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BossDamageBonus,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAPRound"),
                    "UpperArmR",
                    new Vector3(0.01693F, 0.1186F, 0.12737F),
                    new Vector3(86.32347F, 180F, 180F),
                    new Vector3(0.71834F, 0.71834F, 0.71834F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BounceNearby,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHook"),
                    "Chest",
                    new Vector3(-0.00002F, 0.41579F, -0.10431F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.50781F, 0.50781F, 0.50781F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ChainLightning,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkulele"),
                    "Head",
                    new Vector3(-0.05813F, -0.25565F, -0.17511F),
                    new Vector3(76.72027F, 0.00011F, 221.1832F),
                    new Vector3(0.432F, 0.432F, 0.50583F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ChainLightningVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
                    "Head",
                    new Vector3(-0.05813F, -0.25565F, -0.17511F),
                    new Vector3(76.72027F, 0.00011F, 221.1832F),
                    new Vector3(0.432F, 0.432F, 0.50583F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Clover,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayClover"),
                    "Head",
                    new Vector3(-0.11375F, 0.03093F, 0.25329F),
                    new Vector3(76.33544F, 8.73817F, 41.75109F),
                    new Vector3(0.37257F, 0.37257F, 0.37257F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.CloverVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCloverVoid"),
                    "Head",
                    new Vector3(-0.11375F, 0.03093F, 0.25329F),
                    new Vector3(76.33544F, 8.73817F, 41.75109F),
                    new Vector3(0.37257F, 0.37257F, 0.37257F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.CooldownOnCrit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkull"),
                    "HandR",
                    new Vector3(0.03834F, 0.21836F, -0.00093F),
                    new Vector3(272.6805F, 180F, 180F),
                    new Vector3(0.17954F, 0.17954F, 0.17954F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.CritDamage,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserSight"),
                    "Broom",
                    new Vector3(-0.00152F, 1.34531F, -0.13639F),
                    new Vector3(0F, 90F, 270F),
                    new Vector3(0.15625F, 0.15625F, 0.15625F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.CritGlasses,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlasses"),
                    "Head",
                    new Vector3(-0.0018F, 0.16379F, 0.12983F),
                    new Vector3(270F, 180F, 0F),
                    new Vector3(0.22942F, 0.23315F, 0.1432F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.CritGlassesVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
                    "Head",
                    new Vector3(-0.0018F, 0.16379F, 0.12983F),
                    new Vector3(270F, 180F, 0F),
                    new Vector3(0.22942F, 0.23315F, 0.1432F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Crowbar,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCrowbar"),
                    "Chest",
                    new Vector3(-0.15975F, 0.21073F, -0.1427F),
                    new Vector3(355.9364F, 172.5989F, 331.3856F),
                    new Vector3(0.36462F, 0.36462F, 0.36462F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Dagger,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDagger"),
                    "UpperArmL",
                    new Vector3(-0.16277F, -0.10518F, -0.14042F),
                    new Vector3(30.58225F, 341.8326F, 352.5203F),
                    new Vector3(1F, -1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.DeathMark,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathMark"),
                    "HandL",
                    new Vector3(-0.03457F, 0.16648F, 0.04621F),
                    new Vector3(284.3827F, 272.9047F, 86.9721F),
                    new Vector3(0.03101F, 0.03101F, 0.03101F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.EnergizedOnEquipmentUse,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarHorn"),
                    "Pelvis",
                    new Vector3(0.16546F, -0.0389F, 0.02428F),
                    new Vector3(14.22322F, 94.23827F, 176.3027F),
                    new Vector3(0.43337F, 0.43337F, 0.43337F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.EquipmentMagazine,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBattery"),
                    "Pelvis",
                    new Vector3(-0.20866F, -0.01259F, 0.129F),
                    new Vector3(276.9902F, 85.97079F, 180.0001F),
                    new Vector3(0.12318F, 0.12318F, 0.12318F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.EquipmentMagazineVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
                    "Pelvis",
                    new Vector3(-0.21409F, -0.01299F, 0.13009F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.13053F, 0.13053F, 0.13053F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ExecuteLowHealthElite,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGuillotine"),
                    "BroomHead",
                    new Vector3(-0.35323F, -0.46365F, 0.0862F),
                    new Vector3(0.51557F, 90.00626F, 270.6978F),
                    new Vector3(0.52721F, 0.52721F, 0.39759F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ExplodeOnDeath,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWilloWisp"),
                    "Pelvis",
                    new Vector3(0.08902F, 0.02843F, 0.15348F),
                    new Vector3(19.21306F, 10.83366F, 181.6862F),
                    new Vector3(0.05041F, 0.05041F, 0.05338F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ExplodeOnDeathVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
                    "Pelvis",
                    new Vector3(0.08902F, 0.02843F, 0.15348F),
                    new Vector3(19.21306F, 10.83366F, 181.6862F),
                    new Vector3(0.05041F, 0.05041F, 0.05338F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ExtraLife,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippo"),
                    "Chest",
                    new Vector3(0.13634F, 0.35199F, -0.13666F),
                    new Vector3(336.8877F, 153.4926F, 23.49166F),
                    new Vector3(0.18383F, 0.18383F, 0.18383F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ExtraLifeVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippoVoid"),
                    "Chest",
                    new Vector3(0.13634F, 0.35199F, -0.13666F),
                    new Vector3(336.8877F, 153.4926F, 23.49166F),
                    new Vector3(0.18383F, 0.18383F, 0.18383F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FallBoots,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "CalfR",
                    new Vector3(0.00494F, 0.47807F, 0.03648F),
                    new Vector3(352.6691F, 277.6574F, 180.5115F),
                    new Vector3(0.27764F, 0.27764F, 0.27764F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
                    "CalfL",
                    new Vector3(0.00216F, 0.47789F, 0.01594F),
                    new Vector3(12.86568F, 277.8386F, 180.5204F),
                    new Vector3(0.27764F, 0.27764F, 0.27764F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Feather,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFeather"),
                    "UpperArmR",
                    new Vector3(-0.0831F, -0.04298F, -0.0149F),
                    new Vector3(21.82889F, 83.77492F, 131.5974F),
                    new Vector3(0.03217F, 0.03583F, 0.03217F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FireRing,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireRing"),
                    "Broom",
                    new Vector3(-0.00006F, 1.06193F, -0.04561F),
                    new Vector3(272.4608F, 180F, 180F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IceRing,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIceRing"),
                    "Broom",
                    new Vector3(-0.00004F, 0.74125F, -0.03185F),
                    new Vector3(272.4608F, 180F, 180F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ElementalRingVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVoidRing"),
                    "Broom",
                    new Vector3(-0.00004F, 0.88611F, -0.03807F),
                    new Vector3(272.4608F, 180F, 180F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FireballsOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
                    "Head",
                    new Vector3(0.06089F, 0.1606F, -0.06539F),
                    new Vector3(352.1397F, 178.4638F, 10.2204F),
                    new Vector3(0.0524F, 0.0524F, 0.0524F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Firework,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFirework"),
                    "ThighL",
                    new Vector3(0.14753F, 0.09413F, 0.08981F),
                    new Vector3(38.1708F, 321.5965F, 333.9003F),
                    new Vector3(0.31985F, 0.31985F, 0.31985F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FlatHealth,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySteakCurved"),
                    "ThighR",
                    new Vector3(-0.18491F, 0.22293F, -0.00001F),
                    new Vector3(15.65886F, 263.9458F, 268.3603F),
                    new Vector3(0.08897F, 0.08897F, 0.08897F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FocusConvergence,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
                    "Root",
                    new Vector3(1.99996F, -0.19543F, -1.14534F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.09215F, 0.09215F, 0.09215F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.FragileDamageBonus,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
                    "HandR",
                    new Vector3(-0.00001F, 0.00001F, 0.01459F),
                    new Vector3(90F, 0F, 0F),
                    new Vector3(0.56689F, 0.83863F, 0.74009F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.FreeChest,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
                    "CalfL",
                    new Vector3(0.08315F, 0.19383F, 0.10719F),
                    new Vector3(274.4316F, 29.32907F, 197.5813F),
                    new Vector3(0.48946F, 0.48946F, 0.48946F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.GhostOnKill,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMask"),
                    "Head",
                    new Vector3(0.0044F, 0.08999F, 0.12347F),
                    new Vector3(270F, 182.0347F, 0F),
                    new Vector3(0.61528F, 0.61528F, 0.64387F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.GoldOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBoneCrown"),
                    "Head",
                    new Vector3(0.00001F, 0.06154F, 0.13573F),
                    new Vector3(270F, 180F, 0F),
                    new Vector3(0.93485F, 0.93485F, 0.93485F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.GoldOnHurt,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRollOfPennies"),
                    "UpperArmL",
                    new Vector3(0.04F, 0.10752F, 0.09408F),
                    new Vector3(0F, 0F, 94.06913F),
                    new Vector3(0.81096F, 0.81096F, 0.81096F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.HalfAttackSpeedHalfCooldowns,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
                    "UpperArmL",
                    new Vector3(0.04511F, -0.02964F, 0.02338F),
                    new Vector3(340.6534F, 316.6692F, 202.6659F),
                    new Vector3(0.63118F, 0.63118F, 0.63118F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.HalfSpeedDoubleHealth,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
                    "UpperArmR",
                    new Vector3(-0.02584F, -0.04672F, 0.07165F),
                    new Vector3(352.3888F, 273.3138F, 182.7653F),
                    new Vector3(0.59777F, 0.59777F, 0.59777F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.HeadHunter,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkullcrown"),
                    "Pelvis",
                    new Vector3(-0.00001F, 0.07416F, -0.00666F),
                    new Vector3(358.7889F, 180F, 180F),
                    new Vector3(0.58443F, 0.19108F, 0.10261F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.HealOnCrit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScythe"),
                    "Stomach",
                    new Vector3(0.07371F, 0.1661F, -0.12949F),
                    new Vector3(344.0432F, 57.1394F, 114.1822F),
                    new Vector3(0.17555F, 0.17555F, 0.17555F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.HealWhileSafe,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySnail"),
                    "UpperArmL",
                    new Vector3(0.02825F, -0.09555F, -0.06408F),
                    new Vector3(22.81644F, 9.55254F, 230.6241F),
                    new Vector3(0.05938F, 0.05938F, 0.05938F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.HealingPotion,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHealingPotion"),
                    "ThighL",
                    new Vector3(-0.02005F, 0.14272F, 0.15574F),
                    new Vector3(4.66952F, 4.00842F, 220.7212F),
                    new Vector3(0.06508F, 0.06508F, 0.06508F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Hoof,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHoof"),
                    "CalfR",
                    new Vector3(0.04735F, 0.42559F, 0.02582F),
                    new Vector3(75.3112F, 259.5815F, 349.9231F),
                    new Vector3(0.08727F, 0.08727F, 0.08727F)
                ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Icicle,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFrostRelic"),
                    "Root",
                    new Vector3(1.61642F, -0.00694F, 0.79327F),
                    new Vector3(0F, 0F, 358.8099F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IgniteOnKill,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasoline"),
                    "Pelvis",
                    new Vector3(0.22068F, 0.20755F, 0.06157F),
                    new Vector3(40.60497F, 346.4113F, 346.7165F),
                    new Vector3(0.41568F, 0.41568F, 0.41568F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ImmuneToDebuff,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
                    "CalfL",
                    new Vector3(0.04703F, 0F, 0.00777F),
                    new Vector3(0F, 80.61617F, 178.7244F),
                    new Vector3(0.62443F, 0.62443F, 0.62443F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IncreaseHealing,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
                    "Head",
                    new Vector3(0.09052F, 0.01703F, 0.26225F),
                    new Vector3(344.4369F, 82.16043F, 92.39706F),
                    new Vector3(0.33777F, 0.33777F, 0.33777F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
                    "Head",
                    new Vector3(-0.09932F, 0.01265F, 0.25901F),
                    new Vector3(357.3574F, 280.9073F, 262.5785F),
                    new Vector3(0.33777F, 0.33777F, 0.33777F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.Incubator,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
                    "Chest",
                    new Vector3(0.17116F, -0.00001F, 0.10811F),
                    new Vector3(13.96628F, 348.7766F, 320.5743F),
                    new Vector3(0.04152F, 0.04152F, 0.04152F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Infusion,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInfusion"),
                    "Pelvis",
                    new Vector3(-0.12747F, 0.00857F, 0.12164F),
                    new Vector3(359.3219F, 335.1808F, 178.5341F),
                    new Vector3(0.43812F, 0.43812F, 0.43812F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.JumpBoost,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaxBird"),
                    "Head",
                    new Vector3(0.00003F, 0.10221F, -0.42427F),
                    new Vector3(270F, 180F, 0F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.KillEliteFrenzy,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrainstalk"),
                    "Head",
                    new Vector3(-0.0198F, 0.03411F, 0.12136F),
                    new Vector3(26.48516F, 269.3174F, 268.4695F),
                    new Vector3(0.29681F, 0.61382F, 0.27041F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Knurl,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKnurl"),
                    "UpperArmR",
                    new Vector3(-0.14687F, 0.01833F, 0.00337F),
                    new Vector3(58.45601F, 259.836F, 73.73438F),
                    new Vector3(0.06083F, 0.06083F, 0.06083F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LaserTurbine,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
                    "CalfR",
                    new Vector3(-0.00851F, 0.17115F, 0.09662F),
                    new Vector3(2.08681F, 353.6625F, 93.45658F),
                    new Vector3(0.41106F, 0.41106F, 0.41106F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LightningStrikeOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayChargedPerforator"),
                    "Head",
                    new Vector3(-0.0596F, 0.15819F, -0.0192F),
                    new Vector3(278.6371F, 266.8278F, 276.675F),
                    new Vector3(0.84341F, 0.84341F, 0.84341F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarDagger,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarDagger"),
                    "Chest",
                    new Vector3(0.01065F, 0.00577F, -0.18144F),
                    new Vector3(289.3188F, 168.0754F, 207.5459F),
                    new Vector3(0.46517F, 0.46623F, 0.46517F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarPrimaryReplacement,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdEye"),
                    "Head",
                    new Vector3(0.00001F, 0.14476F, 0.12731F),
                    new Vector3(3.2258F, 180F, 180F),
                    new Vector3(0.31032F, 0.31032F, 0.31032F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarSecondaryReplacement,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdClaw"),
                    "LowerArmR",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.7565F, 0.7565F, 0.7565F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarSpecialReplacement,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdHeart"),
                    "Head",
                    new Vector3(0.7769F, 0.23891F, -0.54071F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.29988F, 0.29988F, 0.29988F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.LunarSun,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
                    "Head",
                    new Vector3(0F, -0.00991F, -0.02314F),
                    new Vector3(56.39571F, 310.1387F, 315.3539F),
                    new Vector3(1F, 1F, 1F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHead"),
                    "Head",
                    new Vector3(-0.00198F, 0.03743F, 0.11514F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.62733F, 0.62733F, 0.9104F)
                ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("EmpowerAlways",
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
                    "Head",
                    new Vector3(0F, -0.00991F, -0.02314F),
                    new Vector3(56.39571F, 310.1387F, 315.3539F),
                    new Vector3(1F, 1F, 1F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHead"),
                    "Head",
                    new Vector3(-0.00198F, 0.03743F, 0.11514F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.62733F, 0.62733F, 0.9104F)
                ),
                ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)
            ));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarTrinket,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeads"),
                    "Broom",
                    new Vector3(0.00695F, 0.45849F, -0.02904F),
                    new Vector3(0F, 235.204F, 0F),
                    new Vector3(1.28296F, 1.28296F, 1.28296F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarUtilityReplacement,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdFoot"),
                    "CalfR",
                    new Vector3(-0.13896F, 0.03199F, 0.17776F),
                    new Vector3(356.756F, 67.44904F, 29.60561F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Medkit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMedkit"),
                    "Pelvis",
                    new Vector3(-0.19544F, 0.00825F, 0.03893F),
                    new Vector3(87.94176F, 355.7326F, 265.7327F),
                    new Vector3(0.50978F, 0.52164F, 0.52164F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MinorConstructOnKill,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDefenseNucleus"),
                    "Root",
                    new Vector3(1.99993F, 0.82129F, -0.8974F),
                    new Vector3(294.0437F, 349.8369F, 290.4543F),
                    new Vector3(0.47643F, 0.47643F, 0.47643F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Missile,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
                    "Chest",
                    new Vector3(-0.26358F, 0.47969F, -0.26926F),
                    new Vector3(317.3102F, 15.75007F, 3.22816F),
                    new Vector3(0.07726F, 0.07726F, 0.07726F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MissileVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
                    "Chest",
                    new Vector3(-0.26358F, 0.47969F, -0.26926F),
                    new Vector3(317.3102F, 15.75007F, 3.22816F),
                    new Vector3(0.07726F, 0.07726F, 0.07726F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.MonstersOnShrineUse,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
                    "ThighR",
                    new Vector3(-0.17858F, 0.11648F, 0.03787F),
                    new Vector3(322.0793F, 192.1933F, 351.2322F),
                    new Vector3(0.08622F, 0.08893F, 0.08893F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MoreMissile,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayICBM"),
                    "BroomHead",
                    new Vector3(-0.49252F, 0.21969F, 0.07607F),
                    new Vector3(0F, 0F, 89.97874F),
                    new Vector3(0.39158F, 0.22442F, 0.27931F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MoveSpeedOnKill,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGrappleHook"),
                    "ThighR",
                    new Vector3(0.12261F, 0.05947F, -0.11274F),
                    new Vector3(351.046F, 308.3646F, 82.97639F),
                    new Vector3(0.1749F, 0.1749F, 0.1749F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Mushroom,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroom"),
                    "Pelvis",
                    new Vector3(0.0146F, 0.16794F, -0.10031F),
                    new Vector3(305.5628F, 285.9746F, 70.68364F),
                    new Vector3(0.10679F, 0.10679F, 0.10679F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MushroomVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
                    "Pelvis",
                    new Vector3(0.0146F, 0.16794F, -0.10031F),
                    new Vector3(305.5628F, 285.9746F, 70.68364F),
                    new Vector3(0.10679F, 0.10679F, 0.10679F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NearbyDamageBonus,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDiamond"),
                    "Broom",
                    new Vector3(0.01401F, 0.46014F, -0.03051F),
                    new Vector3(90F, 272.8817F, 0F),
                    new Vector3(0.11883F, 0.11883F, 0.11883F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NovaOnHeal,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "Head",
                    new Vector3(-0.07433F, -0.02411F, 0.06451F),
                    new Vector3(284.5699F, 212.2244F, 4.92912F),
                    new Vector3(0.59631F, 0.59631F, 0.59631F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                    "Head",
                    new Vector3(0.07433F, -0.02411F, 0.06451F),
                    new Vector3(284.5699F, 212.2243F, 289.8446F),
                    new Vector3(-0.59631F, 0.59631F, 0.59631F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NovaOnLowHealth,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayJellyGuts"),
                    "ThighL",
                    new Vector3(0.01891F, 0.30845F, 0.0503F),
                    new Vector3(297.5815F, 99.2698F, 341.6528F),
                    new Vector3(0.10863F, 0.10863F, 0.10863F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.OutOfCombatArmor,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
                    "Chest",
                    new Vector3(0.14999F, 0.15522F, 0.19338F),
                    new Vector3(0F, 0F, 347.1633F),
                    new Vector3(0.32982F, 0.32982F, 0.32982F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ParentEgg,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayParentEgg"),
                    "Stomach",
                    new Vector3(-0.07103F, 0.07411F, 0.22956F),
                    new Vector3(4.83315F, 348.4719F, 22.44489F),
                    new Vector3(0.08782F, 0.08782F, 0.08782F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Pearl,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPearl"),
                    "Broom",
                    new Vector3(0.01712F, 0.4673F, -0.03643F),
                    new Vector3(270F, 0.00001F, 0F),
                    new Vector3(0.12724F, 0.12724F, 0.12724F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShinyPearl,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShinyPearl"),
                    "Broom",
                    new Vector3(0.01703F, 0.46031F, -0.03641F),
                    new Vector3(270F, 0F, 0F),
                    new Vector3(0.12724F, 0.12724F, -0.12724F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.PermanentDebuffOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScorpion"),
                    "Chest",
                    new Vector3(-0.01068F, 0.36411F, -0.09385F),
                    new Vector3(43.69953F, 358.8581F, 356.0534F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.PersonalShield,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                    "Chest",
                    new Vector3(0.00027F, 0.20721F, 0.19144F),
                    new Vector3(275.313F, 205.627F, 159.6195F),
                    new Vector3(0.144F, 0.144F, 0.144F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Phasing,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStealthkit"),
                    "ThighR",
                    new Vector3(0.14291F, 0.19063F, -0.05163F),
                    new Vector3(85.00601F, 103.7042F, 180.9358F),
                    new Vector3(0.30297F, 0.30297F, 0.30297F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Plant,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
                    "UpperArmL",
                    new Vector3(0.12632F, 0.0575F, -0.08125F),
                    new Vector3(7.09771F, 121.4352F, 0F),
                    new Vector3(0.12575F, 0.12575F, 0.12575F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.PrimarySkillShuriken,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShuriken"),
                    "Broom",
                    new Vector3(-0.00002F, 1.11483F, 0.00001F),
                    new Vector3(270F, 45.8909F, 0F),
                    new Vector3(1.54663F, 1.54663F, 1.54663F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.RandomDamageZone,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
                    "Chest",
                    new Vector3(-0.00004F, 0.19772F, -0.19889F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.09418F, 0.09418F, 0.09418F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.RandomEquipmentTrigger,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBottledChaos"),
                    "Pelvis",
                    new Vector3(-0.03793F, 0.03793F, 0.14032F),
                    new Vector3(350.5406F, 4.86953F, 179.1978F),
                    new Vector3(0.14028F, 0.14028F, 0.14028F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.RandomlyLunar,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDomino"),
                    "Root",
                    new Vector3(2.05117F, -0.2474F, 0.98529F),
                    new Vector3(0F, 76.92225F, 0F),
                    new Vector3(0.71049F, 0.71049F, 0.71049F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.RegeneratingScrap,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
                    "Pelvis",
                    new Vector3(0.1386F, 0.2019F, 0.19457F),
                    new Vector3(357.0432F, 200.9825F, 190.9932F),
                    new Vector3(0.17205F, 0.17205F, 0.17205F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.RepeatHeal,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCorpseflower"),
                    "Head",
                    new Vector3(0.1314F, 0.02372F, 0.26576F),
                    new Vector3(67.94113F, 204.2303F, 151.5391F),
                    new Vector3(0.16994F, 0.16994F, 0.16994F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SecondarySkillMagazine,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                    "Broom",
                    new Vector3(-0.17931F, 0.27665F, -0.01528F),
                    new Vector3(275.0581F, 90.19373F, 179.9999F),
                    new Vector3(0.07712F, 0.07712F, 0.07712F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Seed,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySeed"),
                    "UpperArmR",
                    new Vector3(0.132F, -0.1001F, -0.00003F),
                    new Vector3(0F, 0F, 256.3376F),
                    new Vector3(0.05783F, 0.05783F, 0.05783F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShieldOnly,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Head",
                    new Vector3(-0.06624F, 0.10327F, 0.27561F),
                    new Vector3(340.9475F, 88.1622F, 90.44755F),
                    new Vector3(0.23328F, 0.22403F, 0.22403F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
                    "Head",
                    new Vector3(0.07739F, 0.10946F, 0.2742F),
                    new Vector3(16.99376F, 134.3086F, 93.97624F),
                    new Vector3(0.24331F, 0.24331F, 0.24331F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShockNearby,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
                    "BroomHead",
                    new Vector3(-0.00001F, 0.1575F, -0.09331F),
                    new Vector3(270F, 0.00001F, 0F),
                    new Vector3(0.47186F, 0.47186F, 0.66674F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SiphonOnLowHealth,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
                    "Stomach",
                    new Vector3(0.16126F, 0.09006F, -0.11254F),
                    new Vector3(39.45484F, 168.968F, 30.07182F),
                    new Vector3(0.06434F, 0.06434F, 0.06434F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SlowOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBauble"),
                    "UpperArmR",
                    new Vector3(0.11415F, 0.46891F, -0.0451F),
                    new Vector3(0F, 0F, 103.319F),
                    new Vector3(0.33023F, 0.33023F, 0.33023F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.SlowOnHitVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
                    "UpperArmR",
                    new Vector3(0.11415F, 0.46891F, -0.0451F),
                    new Vector3(0F, 0F, 103.319F),
                    new Vector3(0.33023F, 0.33023F, 0.33023F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SprintArmor,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBuckler"),
                    "LowerArmR",
                    new Vector3(-0.02923F, 0.25486F, 0.06305F),
                    new Vector3(353.9946F, 0F, 0F),
                    new Vector3(0.17866F, 0.17866F, 0.17866F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SprintBonus,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySoda"),
                    "Pelvis",
                    new Vector3(0.15182F, 0.04126F, -0.09585F),
                    new Vector3(87.32702F, 180F, 180F),
                    new Vector3(0.2605F, 0.2605F, 0.2605F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SprintOutOfCombat,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWhip"),
                    "Pelvis",
                    new Vector3(-0.2169F, 0.15665F, 0.05913F),
                    new Vector3(0F, 0F, 199.4656F),
                    new Vector3(0.31693F, 0.31693F, 0.31693F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SprintWisp,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrokenMask"),
                    "Stomach",
                    new Vector3(0.09213F, 0.14248F, -0.11422F),
                    new Vector3(353.9642F, 151.6872F, 6.07737F),
                    new Vector3(0.11667F, 0.11667F, 0.11667F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Squid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySquidTurret"),
                    "Pelvis",
                    new Vector3(0.11284F, 0.18206F, 0.16612F),
                    new Vector3(81.77419F, 180F, 180F),
                    new Vector3(0.05462F, 0.05462F, 0.05462F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.StickyBomb,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStickyBomb"),
                    "ThighL",
                    new Vector3(-0.10021F, 0.04921F, 0.11207F),
                    new Vector3(10.25249F, 289.7552F, 247.1061F),
                    new Vector3(0.27528F, 0.27528F, 0.27528F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.StrengthenBurn,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasTank"),
                    "CalfL",
                    new Vector3(-0.02418F, 0.16118F, -0.08983F),
                    new Vector3(341.115F, 180F, 180F),
                    new Vector3(0.15782F, 0.15782F, 0.15782F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.StunChanceOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStunGrenade"),
                    "ThighL",
                    new Vector3(0.15001F, 0.20611F, -0.03908F),
                    new Vector3(66.84044F, 180F, 180F),
                    new Vector3(0.65301F, 0.65301F, 0.65301F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Syringe,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                    "UpperArmL",
                    new Vector3(0.11701F, 0.26922F, -0.01178F),
                    new Vector3(70.41078F, 85.54021F, 326.2664F),
                    new Vector3(0.15582F, 0.15582F, 0.15582F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.TPHealingNova,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlowFlower"),
                    "CalfL",
                    new Vector3(0.12653F, 0.126F, 0.03576F),
                    new Vector3(0F, 74.22023F, 0F),
                    new Vector3(0.39651F, 0.39651F, 0.39651F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Talisman,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTalisman"),
                    "Root",
                    new Vector3(2.26674F, 0.29053F, 0.62209F),
                    new Vector3(288.364F, 37.92486F, 230.6145F),
                    new Vector3(0.6458F, 0.6458F, 0.6458F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Thorns,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
                    "UpperArmL",
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.68366F, 0.68366F, 0.68366F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.TitanGoldDuringTP,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldHeart"),
                    "UpperArmR",
                    new Vector3(-0.11906F, 0.1794F, -0.07018F),
                    new Vector3(359.6279F, 228.5979F, 179.578F),
                    new Vector3(0.21508F, 0.21508F, 0.21508F)
            )));

            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Tooth,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
                    "Chest",
                    new Vector3(-0.00007F, 0.34448F, 0.14543F),
                    new Vector3(346.1043F, 0F, 0F),
                    new Vector3(2.09205F, 2.09205F, 2.09205F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Chest",
                    new Vector3(-0.03857F, 0.34411F, 0.14835F),
                    new Vector3(345.4234F, 2.94222F, 345.9512F),
                    new Vector3(1.5648F, 1.5648F, 1.55569F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Chest",
                    new Vector3(-0.06238F, 0.35424F, 0.14976F),
                    new Vector3(345.919F, 8.08656F, 321.8032F),
                    new Vector3(1.24714F, 1.24714F, 1.24714F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
                    "Chest",
                    new Vector3(0.03342F, 0.3448F, 0.14289F),
                    new Vector3(347.3368F, 357.5063F, 12.94002F),
                    new Vector3(1.34406F, 1.34406F, 1.34406F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
                    "Chest",
                    new Vector3(0.05534F, 0.35045F, 0.14102F),
                    new Vector3(348.7161F, 355.7397F, 23.34462F),
                    new Vector3(1.30419F, 1.30419F, 1.30419F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.TreasureCache,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKey"),
                    "ThighR",
                    new Vector3(-0.12128F, 0.26072F, 0.06454F),
                    new Vector3(328.2108F, 19.78612F, 273.502F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.TreasureCacheVoid,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKeyVoid"),
                    "ThighR",
                    new Vector3(-0.12128F, 0.26072F, 0.06454F),
                    new Vector3(328.2108F, 19.78612F, 273.502F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.UtilitySkillMagazine,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "UpperArmL",
                    new Vector3(-0.1653F, 0.08384F, -0.09514F),
                    new Vector3(339.6255F, 339.4724F, 47.08277F),
                    new Vector3(0.81766F, 0.81766F, 0.81766F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                    "UpperArmR",
                    new Vector3(0.00714F, 0.03423F, -0.18293F),
                    new Vector3(298.644F, 1.5185F, 337.9953F),
                    new Vector3(0.81766F, 0.81766F, 0.81766F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.VoidMegaCrabItem,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
                    "Stomach",
                    new Vector3(0.13426F, 0.11444F, -0.1233F),
                    new Vector3(0F, 164.1894F, 65.97357F),
                    new Vector3(0.11307F, 0.11307F, 0.11307F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.WarCryOnMultiKill,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPauldron"),
                    "UpperArmR",
                    new Vector3(-0.01802F, -0.02058F, 0.00428F),
                    new Vector3(78.1953F, 0F, 0F),
                    new Vector3(0.80723F, 0.80723F, 0.80723F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.WardOnLevel,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarbanner"),
                    "Stomach",
                    new Vector3(-0.04271F, 0.09973F, -0.1414F),
                    new Vector3(270.9304F, 291.9171F, 338.0854F),
                    new Vector3(0.25751F, 0.25751F, 0.25751F)
            )));

            #endregion items
            #region equips
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.BFG,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBFG"),
                    "Chest",
                    new Vector3(-0.00001F, 0.37669F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.52039F, 0.52039F, 0.52039F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Blackhole,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravCube"),
                    "Root",
                    new Vector3(1.99994F, -0.63136F, -0.54605F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.52026F, 0.52026F, 0.52026F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.BossHunter,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornGhost"),
                    "Head",
                    new Vector3(0.00875F, 0.03021F, 0.24766F),
                    new Vector3(300.3105F, 180F, 0F),
                    new Vector3(0.63131F, 0.63131F, 0.63131F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
                    "Root",
                    new Vector3(1.94225F, -0.54331F, -0.66767F),
                    new Vector3(0.00002F, 269.0287F, 0.00001F),
                    new Vector3(0.74956F, 0.74956F, 0.74956F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.BossHunterConsumed,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornUsed"),
                    "Head",
                    new Vector3(0.00875F, 0.03021F, 0.24766F),
                    new Vector3(300.3105F, 180F, 0F),
                    new Vector3(0.63131F, 0.63131F, 0.63131F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.BurnNearby,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPotion"),
                    "Chest",
                    new Vector3(0.1323F, 0.02892F, -0.10836F),
                    new Vector3(348.7431F, 26.52866F, 280.2495F),
                    new Vector3(0.03976F, 0.03976F, 0.03976F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Cleanse,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaterPack"),
                    "Chest",
                    new Vector3(0.10548F, 0.01498F, -0.10909F),
                    new Vector3(40.83774F, 158.5257F, 23.65814F),
                    new Vector3(0.04497F, 0.04497F, 0.04497F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.CommandMissile,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileRack"),
                    "Chest",
                    new Vector3(0.1091F, 0.02046F, -0.11699F),
                    new Vector3(89.55106F, 350.1162F, 180.0006F),
                    new Vector3(0.27717F, 0.27717F, 0.27717F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.CrippleWard,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEffigy"),
                    "Chest",
                    new Vector3(0.11994F, -0.06803F, -0.1597F),
                    new Vector3(339.5139F, 0F, 0F),
                    new Vector3(0.38542F, 0.38542F, 0.38542F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.CritOnUse,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
                    "Head",
                    new Vector3(0.00002F, 0.26479F, 0.10875F),
                    new Vector3(270F, 0.00001F, 0F),
                    new Vector3(0.20115F, 0.25357F, 0.19761F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.DeathProjectile,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
                    "Chest",
                    new Vector3(0.11974F, -0.00003F, -0.14193F),
                    new Vector3(15.7537F, 167.7942F, 0F),
                    new Vector3(0.07944F, 0.07944F, 0.07937F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.DroneBackup,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRadio"),
                    "Chest",
                    new Vector3(0.12752F, 0.04956F, -0.1663F),
                    new Vector3(17.61459F, 183.4723F, 20.96004F),
                    new Vector3(0.46518F, 0.46518F, 0.46518F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Elites.Earth.eliteEquipmentDef,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteMendingAntlers"),
                    "Head",
                    new Vector3(-0.00213F, 0.04639F, 0.16906F),
                    new Vector3(87.92765F, 0.92184F, 0.54948F),
                    new Vector3(0.73633F, 0.73633F, 0.78274F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixRed,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                    "Head",
                    new Vector3(-0.12868F, 0.00998F, 0.26474F),
                    new Vector3(287.0104F, 167.3562F, 10.61231F),
                    new Vector3(0.09232F, 0.09232F, 0.09232F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                    "Head",
                    new Vector3(0.12868F, 0.00998F, 0.26474F),
                    new Vector3(287.0104F, 167.3562F, 10.61231F),
                    new Vector3(-0.09232F, 0.09232F, 0.09232F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixHaunted,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
                    "Head",
                    new Vector3(-0.00237F, 0.07086F, 0.36655F),
                    new Vector3(5.2256F, 0.1614F, 183.1073F),
                    new Vector3(0.04637F, 0.04637F, 0.04637F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixWhite,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
                    "Head",
                    new Vector3(-0.00165F, 0.05198F, 0.33983F),
                    new Vector3(353.4686F, 359.9142F, 179.2197F),
                    new Vector3(0.0289F, 0.0289F, 0.0289F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixBlue,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                    "Head",
                    new Vector3(-0.00628F, 0.16786F, 0.2781F),
                    new Vector3(352.4962F, 359.535F, 179.4846F),
                    new Vector3(0.25121F, 0.25121F, 0.25121F)
                ),
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                    "Head",
                    new Vector3(-0.00168F, 0.05306F, 0.28465F),
                    new Vector3(3.73845F, 0.42939F, 177.4046F),
                    new Vector3(0.1942F, 0.1942F, 0.1942F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixLunar,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteLunar,Eye"),
                    "Head",
                    new Vector3(-0.01038F, 0.26318F, 0.09345F),
                    new Vector3(85.8074F, 148.1605F, 145.7384F),
                    new Vector3(0.2363F, 0.2363F, 0.2363F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixPoison,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
                    "Head",
                    new Vector3(-0.0003F, 0.01643F, 0.2879F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.06374F, 0.0636F, 0.06374F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.EliteVoidEquipment,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAffixVoid"),
                    "Head",
                    new Vector3(0.00001F, 0.13062F, 0.11479F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.14321F, 0.14321F, 0.14321F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.FireBallDash,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEgg"),
                    "Chest",
                    new Vector3(0.11773F, -0.00847F, -0.10399F),
                    new Vector3(299.9879F, 153.9736F, 202.9252F),
                    new Vector3(0.33774F, 0.33774F, 0.33774F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Fruit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFruit"),
                    "Chest",
                    new Vector3(-0.03726F, -0.35429F, 0.05235F),
                    new Vector3(0F, 54.58615F, 0F),
                    new Vector3(0.30196F, 0.30196F, 0.30196F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.GainArmor,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayElephantFigure"),
                    "Chest",
                    new Vector3(0.12226F, 0.01342F, -0.17727F),
                    new Vector3(304.836F, 138.6736F, 215.82F),
                    new Vector3(0.5874F, 0.5874F, 0.5874F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Gateway,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVase"),
                    "Chest",
                    new Vector3(0.13303F, 0.06204F, -0.19283F),
                    new Vector3(19.53514F, 74.28463F, 304.0134F),
                    new Vector3(0.22963F, 0.22963F, 0.22963F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.GoldGat,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldGat"),
                    "Head",
                    new Vector3(0F, -0.00078F, 0.47795F),
                    new Vector3(354.3331F, 270.0092F, 269.9072F),
                    new Vector3(0.1111F, 0.1111F, 0.1111F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.GummyClone,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGummyClone"),
                    "Chest",
                    new Vector3(0.15443F, 0.09473F, -0.16544F),
                    new Vector3(332.808F, 0F, 329.4202F),
                    new Vector3(0.21179F, 0.21179F, 0.21179F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("IrradiatingLaser",
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIrradiatingLaser"),
                    "Chest",
                    new Vector3(0.13469F, -0.01743F, -0.05397F),
                    new Vector3(307.7972F, 180F, 180F),
                    new Vector3(0.09339F, 0.09339F, 0.09339F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Jetpack,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBugWings"),
                    "Chest",
                    new Vector3(-0.00003F, 0.00053F, 0F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(0.24973F, 0.24973F, 0.24973F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.LifestealOnHit,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
                    "Chest",
                    new Vector3(0.18192F, -0.03024F, -0.15592F),
                    new Vector3(331.5204F, 326.8986F, 279.2948F),
                    new Vector3(0.14493F, 0.14493F, 0.14493F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Lightning,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
                    "Pelvis",
                    new Vector3(2, 2, 2),
                    new Vector3(0, 0, 0),
                    new Vector3(1, 1, 1)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.LunarPortalOnUse,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarPortalOnUse"),
                    "Root",
                    new Vector3(1.45257F, -0.35842F, -0.75593F),
                    new Vector3(289.6129F, 129.251F, 142.416F),
                    new Vector3(0.75746F, 0.75746F, 0.75746F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Meteor,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMeteor"),
                    "Root",
                    new Vector3(1.78329F, -0.54538F, -0.62443F),
                    new Vector3(0F, 0F, 0F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.Molotov,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMolotov"),
                    "Pelvis",
                    new Vector3(0.11025F, -0.2139F, 0.14409F),
                    new Vector3(33.96423F, 180F, 160.2393F),
                    new Vector3(0.26171F, 0.26171F, 0.26171F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.MultiShopCard,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
                    "Chest",
                    new Vector3(0.12094F, 0.00102F, -0.14436F),
                    new Vector3(335.157F, 304.0425F, 86.40652F),
                    new Vector3(1F, 1F, 1F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.QuestVolatileBattery,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBatteryArray"),
                    "Chest",
                    new Vector3(0.00112F, 0.10935F, -0.27917F),
                    new Vector3(0F, 180F, 0F),
                    new Vector3(0.35013F, 0.35013F, 0.35013F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Recycle,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRecycler"),
                    "Pelvis",
                    new Vector3(0.10315F, -0.26396F, 0.17597F),
                    new Vector3(346.0809F, 112.6943F, 160.7941F),
                    new Vector3(0.06046F, 0.06046F, 0.06046F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Saw,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySawmerangFollower"),
                    "Root",
                    new Vector3(1.67458F, -0.68721F, -0.5356F),
                    new Vector3(0F, 89.80949F, 0F),
                    new Vector3(0.13793F, 0.13793F, 0.13793F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Scanner,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScanner"),
                    "Chest",
                    new Vector3(0.06319F, 0.05348F, -0.17455F),
                    new Vector3(342.9559F, 180F, 180F),
                    new Vector3(0.21953F, 0.21953F, 0.21953F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.TeamWarCry,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
                    "Chest",
                    new Vector3(0.12316F, 0.01343F, -0.17398F),
                    new Vector3(31.60163F, 174.9196F, 3.72611F),
                    new Vector3(0.05398F, 0.05398F, 0.05398F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Tonic,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTonic"),
                    "Chest",
                    new Vector3(0.11659F, 0.0284F, -0.15739F),
                    new Vector3(311.9842F, 342.7171F, 9.33097F),
                    new Vector3(0.1942F, 0.1942F, 0.1942F)
            )));
            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.VendingMachine,
                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVendingMachine"),
                    "Pelvis",
                    new Vector3(0.08008F, -0.26609F, 0.10498F),
                    new Vector3(325.7831F, 25.30642F, 359.2435F),
                    new Vector3(0.21792F, 0.21792F, 0.21792F)
            )));
            #endregion quips

            #endregion displyas
        }
    }
}