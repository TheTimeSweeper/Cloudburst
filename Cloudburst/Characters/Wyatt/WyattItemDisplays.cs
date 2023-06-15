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

            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.AlienHead,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAlienHead"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));            
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorPlate,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ArmorReductionOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarhammer"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.AttackSpeedAndMoveSpeed,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCoffee"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.AttackSpeedOnCrit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWolfPelt"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.AutoCastEquipment,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFossil"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Bandolier,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBandolier"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BarrierOnKill,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrooch"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BarrierOnOverHeal,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAegis"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Bear,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBear"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BearVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBearVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BeetleGland,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeetleGland"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Behemoth,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBehemoth"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTip"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BleedOnHitAndExplode,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.BleedOnHitVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BonusGoldPackOnKill,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTome"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BossDamageBonus,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAPRound"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.BounceNearby,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHook"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ChainLightning,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkulele"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ChainLightningVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Clover,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayClover"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.CloverVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCloverVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.CooldownOnCrit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkull"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.CritDamage,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserSight"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.CritGlasses,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlasses"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.CritGlassesVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Crowbar,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCrowbar"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Dagger,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDagger"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.DeathMark,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathMark"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ElementalRingVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVoidRing"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.LunarSun,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySunHead"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.Head)));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.EnergizedOnEquipmentUse,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarHorn"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.EquipmentMagazine,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBattery"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.EquipmentMagazineVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ExecuteLowHealthElite,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGuillotine"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ExplodeOnDeath,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWilloWisp"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ExplodeOnDeathVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ExtraLife,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippo"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ExtraLifeVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHippoVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FallBoots,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravBoots"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Feather,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFeather"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FireballsOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FireRing,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFireRing"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Firework,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFirework"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FlatHealth,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySteakCurved"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.FocusConvergence,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.FragileDamageBonus,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.FreeChest,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.GhostOnKill,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMask"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.GoldOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBoneCrown"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.GoldOnHurt,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRollOfPennies"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.HalfAttackSpeedHalfCooldowns,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.HalfSpeedDoubleHealth,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.HeadHunter,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySkullcrown"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.HealingPotion,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHealingPotion"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.HealOnCrit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScythe"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.HealWhileSafe,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySnail"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Hoof,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayHoof"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightCalf)));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IceRing,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIceRing"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Icicle,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFrostRelic"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IgniteOnKill,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasoline"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.ImmuneToDebuff,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.IncreaseHealing,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAntler"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(JunkContent.Items.Incubator,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Infusion,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInfusion"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.JumpBoost,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaxBird"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.KillEliteFrenzy,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrainstalk"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Knurl,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKnurl"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LaserTurbine,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LightningStrikeOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayChargedPerforator"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarDagger,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarDagger"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarPrimaryReplacement,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdEye"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarSecondaryReplacement,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdClaw"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarSpecialReplacement,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdHeart"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarTrinket,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBeads"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.LunarUtilityReplacement,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBirdFoot"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Medkit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMedkit"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MinorConstructOnKill,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDefenseNucleus"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Missile,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MissileVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.MonstersOnShrineUse,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MoreMissile,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayICBM"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MoveSpeedOnKill,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGrappleHook"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Mushroom,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroom"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.MushroomVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NearbyDamageBonus,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDiamond"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NovaOnHeal,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDevilHorns"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.NovaOnLowHealth,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayJellyGuts"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.OutOfCombatArmor,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ParentEgg,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayParentEgg"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Pearl,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPearl"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.PermanentDebuffOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScorpion"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.PersonalShield,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Phasing,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStealthkit"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Plant,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.PrimarySkillShuriken,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShuriken"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.RandomDamageZone,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.RandomEquipmentTrigger,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBottledChaos"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.RandomlyLunar,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDomino"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.RegeneratingScrap,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.RepeatHeal,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayCorpseflower"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SecondarySkillMagazine,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDoubleMag"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Seed,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySeed"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShieldOnly,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShieldBug"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShinyPearl,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayShinyPearl"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.ShockNearby,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SiphonOnLowHealth,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SlowOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBauble"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.SlowOnHitVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SprintArmor,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBuckler"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SprintBonus,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySoda"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SprintOutOfCombat,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWhip"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.SprintWisp,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBrokenMask"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Squid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySquidTurret"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.StickyBomb,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStickyBomb"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.StrengthenBurn,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGasTank"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.StunChanceOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayStunGrenade"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Syringe,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Talisman,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTalisman"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Thorns,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.TitanGoldDuringTP,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldHeart"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.Tooth,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothNecklaceDecal"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall2"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayToothMeshSmall1"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.TPHealingNova,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGlowFlower"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.TreasureCache,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKey"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.TreasureCacheVoid,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayKeyVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.UtilitySkillMagazine,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Items.VoidMegaCrabItem,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.WarCryOnMultiKill,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPauldron"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Items.WardOnLevel,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWarbanner"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixHaunted,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.MultiShopCard,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixBlue,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.BurnNearby,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayPotion"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.GoldGat,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGoldGat"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.BFG,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBFG"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.DeathProjectile,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Fruit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayFruit"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.EliteVoidEquipment,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayAffixVoid"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.CommandMissile,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMissileRack"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Cleanse,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayWaterPack"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.FireBallDash,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEgg"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Meteor,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMeteor"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixPoison,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.Molotov,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayMolotov"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixLunar,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteLunar,Eye"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.DroneBackup,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRadio"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Elites.Earth.eliteEquipmentDef,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteMendingAntlers"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.VendingMachine,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVendingMachine"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.QuestVolatileBattery,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBatteryArray"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixWhite,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Blackhole,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGravCube"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.TeamWarCry,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Gateway,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayVase"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Scanner,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayScanner"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Recycle,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayRecycler"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Jetpack,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBugWings"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.CrippleWard,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEffigy"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.BossHunter,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornGhost"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.BossHunterConsumed,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTricornUsed"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.LunarPortalOnUse,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLunarPortalOnUse"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Saw,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplaySawmerangFollower"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.LifestealOnHit,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Tonic,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayTonic"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(DLC1Content.Equipment.GummyClone,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayGummyClone"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules("IrradiatingLaser",
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayIrradiatingLaser"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.GainArmor,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayElephantFigure"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.CritOnUse,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.Lightning,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateLimbMaskDisplayRule(LimbFlags.RightArm)));
            //    itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules(RoR2Content.Equipment.AffixRed,
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //        ),
            //        ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay("DisplayEliteHorn"),
            //            "Head",
            //            new Vector3(2, 2, 2),
            //            new Vector3(0, 0, 0),
            //            new Vector3(1, 1, 1)
            //    )));

            #endregion displyas
        }
    }
}