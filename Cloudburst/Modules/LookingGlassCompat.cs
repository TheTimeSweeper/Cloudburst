using Cloudburst.Items.Equipment;
using Cloudburst.Items.Gray;
using Cloudburst.Items.Gray.BlastBoot;
using Cloudburst.Items.Gray.RiftBubble;
using Cloudburst.Items.Green;
using Cloudburst.Items.Green.RestlessRings;
using Cloudburst.Items.Red;
using LookingGlass.ItemStatsNameSpace;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using StatType = LookingGlass.ItemStatsNameSpace.ItemStatsDef.ValueType;
using Units = LookingGlass.ItemStatsNameSpace.ItemStatsDef.MeasurementUnits;

namespace Cloudburst.Modules
{
    //adds the stat lines LookingGlass shows under item descriptions
    //every method touching LookingGlass types is NoInlining so nothing loads unless LookingGlass is installed
    internal static class LookingGlassCompat
    {
        public const string GUID = "droppod.lookingglass";
        public static bool IsLookingGlassInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Init()
        {
            ItemCatalog.availability.CallWhenAvailable(RegisterItemStats);
            EquipmentCatalog.availability.CallWhenAvailable(RegisterEquipmentStats);
        }

        private struct Line
        {
            public string description;
            public StatType valueType;
            public Units units;

            public Line(string description, StatType valueType, Units units)
            {
                this.description = description;
                this.valueType = valueType;
                this.units = units;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ItemStatsDef Create(Line[] lines)
        {
            var def = new ItemStatsDef();
            foreach (var line in lines)
            {
                def.descriptions.Add(line.description);
                def.valueTypes.Add(line.valueType);
                def.measurementUnits.Add(line.units);
            }
            return def;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void AddItem(ItemDef item, Func<int, List<float>> values, params Line[] lines)
        {
            var def = Create(lines);
            def.calculateValuesFlat = values;
            Register(item, def);
        }

        //chances LookingGlass should show luck adjusted
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void AddChanceItem(ItemDef item, Func<float, int, List<float>> values, params Line[] lines)
        {
            var def = Create(lines);
            def.hasChance = true;
            def.calculateValuesNew = (luck, count, procChance) => values(luck, count);
            Register(item, def);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void AddEquipment(EquipmentDef equipment, Func<List<float>> values, params Line[] lines)
        {
            if (!equipment || equipment.equipmentIndex == EquipmentIndex.None)
                return;
            var def = Create(lines);
            def.calculateValuesFlat = count => values();
            ItemDefinitions.allEquipmentDefinitions[(int)equipment.equipmentIndex] = def;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Register(ItemDef item, ItemStatsDef def)
        {
            //disabled items never get created
            if (!item || item.itemIndex == ItemIndex.None)
                return;
            ItemDefinitions.allItemDefinitions[(int)item.itemIndex] = def;
        }

        private static float Stack(float initial, float additional, int count) => initial + additional * (count - 1);

        //same rerolls as Util.CheckRoll. chance is 0..1
        private static float ChanceWithLuck(float chance, float luck)
        {
            chance = Math.Min(chance, 1f);
            int rolls = (int)Math.Ceiling(luck);
            if (rolls > 0)
                return 1f - (float)Math.Pow(1f - chance, rolls + 1);
            if (rolls < 0)
                return (float)Math.Pow(chance, -rolls + 1);
            return chance;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RegisterItemStats()
        {
            try
            {
                RegisterWhite();
                RegisterGreen();
                RegisterRed();
            }
            catch (Exception e)
            {
                Log.Error("Failed to register LookingGlass item stats: " + e);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RegisterWhite()
        {
            //crit chance doesn't stack, crit damage is 0.10 + 0.30 per stack
            AddItem(GlassHarvester.glassHarvesterItem, count => new List<float> { 0.05f, 0.1f + 0.3f * count },
                new Line("Crit Chance: ", StatType.Damage, Units.Percentage),
                new Line("Bonus Crit Damage: ", StatType.Damage, Units.Percentage));

            AddItem(BlastBoot.blastBootItem, count => new List<float>
                {
                    BlastBoot.BaseFireworks + count,
                    Stack(BlastBoot.BaseDamage, BlastBoot.StackingDamage, count),
                    BlastBoot.Cooldown
                },
                new Line("Fireworks: ", StatType.Damage, Units.Number),
                new Line("Firework Damage: ", StatType.Damage, Units.Percentage),
                new Line("Cooldown: ", StatType.Utility, Units.Seconds));

            //the ward also adds the holder's body radius on top of this
            AddItem(RiftBubble.riftBubbleItem, count => new List<float>
                {
                    Stack(RiftBubble.BaseRadius, RiftBubble.StackingRadius, count),
                    1 - RiftBubble.KnockbackMultiplier
                },
                new Line("Radius: ", StatType.Utility, Units.Meters),
                new Line("Knockback Reduction: ", StatType.Utility, Units.Percentage));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RegisterGreen()
        {
            AddItem(BismuthEarrings.bismuthEarringsItem, count => new List<float>
                {
                    0.05f,
                    Stack(BismuthEarrings.BaseBarrier, BismuthEarrings.StackingBarrier, count)
                },
                new Line("Bleed Chance: ", StatType.Damage, Units.Percentage),
                new Line("Barrier on Bleed: ", StatType.Healing, Units.FlatHealth));

            AddChanceItem(EnigmaticKeycard.enigmaticKeycardItem, (luck, count) => new List<float>
                {
                    ChanceWithLuck(EnigmaticKeycard.Chance * .01f, luck),
                    EnigmaticKeycard.BaseDamage * count
                },
                new Line("Orb Chance: ", StatType.Damage, Units.Percentage),
                new Line("Orb Damage: ", StatType.Damage, Units.Percentage));

            //one buff per stack, each buff takes 15 armor plus a flat 15
            AddItem(FabinhoruDagger.fabinhorusDaggerItem, count => new List<float> { 0.05f, 15 * count + 15 },
                new Line("Bleed Chance: ", StatType.Damage, Units.Percentage),
                new Line("Armor Reduction: ", StatType.Damage, Units.Number));

            //each buff gives 5 armor and 30% healing
            AddItem(JapesCloak.japesCloakItem, count => new List<float> { count * 2 + 1, 5 * (count * 2 + 1), 0.3f * (count * 2 + 1) },
                new Line("Max Buffs: ", StatType.Utility, Units.Number),
                new Line("Max Armor: ", StatType.Armor, Units.Number),
                new Line("Max Healing Bonus: ", StatType.Healing, Units.Percentage));

            AddItem(MechanicalTrinket.mechanicalTrinketItem, count => new List<float>
                {
                    MechanicalTrinket.RadiusPerStack * count,
                    Math.Min(count, MechanicalTrinket.MaxDrones)
                },
                new Line("Holdout Radius: ", StatType.Utility, Units.Meters),
                new Line("Drones: ", StatType.Utility, Units.Number));

            AddItem(RestlessRings.restlessRingsItem, count => new List<float>
                {
                    RestlessRings.ArmorPerStack * count,
                    RestlessRings.RegenPerStack * count,
                    RestlessRings.AttackSpeedPerStack * count
                },
                new Line("Armor: ", StatType.Armor, Units.Number),
                new Line("Regeneration: ", StatType.Healing, Units.Percentage),
                new Line("Attack Speed: ", StatType.Damage, Units.Percentage));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RegisterRed()
        {
            AddChanceItem(Lemdog.lemdogItem, (luck, count) => new List<float>
                {
                    ChanceWithLuck((Lemdog.BaseConvertChance + Lemdog.StackingConvertChance * count) * .01f, luck),
                    Stack(Lemdog.BaseDurationBonus, Lemdog.StackingDurationBonus, count)
                },
                new Line("Debuff Conversion Chance: ", StatType.Utility, Units.Percentage),
                new Line("Buff Duration: ", StatType.Utility, Units.Seconds));

            AddChanceItem(Extractor.extractorItem, (luck, count) => new List<float>
                {
                    ChanceWithLuck((Extractor.BaseChance + Extractor.StackingChance * count) * .01f, luck),
                    Extractor.BarrierPerProjectile * count
                },
                new Line("Boss Item Chance: ", StatType.Utility, Units.Percentage),
                new Line("Barrier per Projectile: ", StatType.Healing, Units.FlatHealth));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RegisterEquipmentStats()
        {
            try
            {
                AddEquipment(Lumpkin.lumpkinEquipment, () => new List<float>
                    {
                        Lumpkin.DamageCoefficient,
                        Lumpkin.MaxHealthCoefficient,
                        Lumpkin.Radius
                    },
                    new Line("Damage: ", StatType.Damage, Units.Percentage),
                    new Line("Max Health Damage: ", StatType.Damage, Units.Percentage),
                    new Line("Radius: ", StatType.Utility, Units.Meters));
            }
            catch (Exception e)
            {
                Log.Error("Failed to register LookingGlass equipment stats: " + e);
            }
        }
    }
}
