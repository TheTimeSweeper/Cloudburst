using BepInEx.Configuration;
using Cloudburst.Modules;

namespace Cloudburst.Characters.Wyatt
{
    public static class WyattConfig
    {
        public static ConfigEntry<float> M1Damage;
        public static ConfigEntry<float> M1DamageFinisher;
        public static ConfigEntry<float> M1AttackDuration;
        public static ConfigEntry<float> M1AttackDurationFinisher;
        public static ConfigEntry<float> M1UpwardsLiftForce;
        public static ConfigEntry<float> M1KnockbackForce;
        public static ConfigEntry<float> M1AntiGravDuration;

        public static ConfigEntry<float> M2Damage;

        public static ConfigEntry<float> M3GrooveSpeedMultiplierPerStack;
        
        public static ConfigEntry<float> M3FlowDurationBase;
        public static ConfigEntry<float> M3FlowDurationPerStack;

        public static ConfigEntry<float> M3FlowArmorBase;
        public static ConfigEntry<float> M3FlowArmorPerStack;

        public static ConfigEntry<float> M3FlowCDR;
        public static ConfigEntry<int> M3FlowExtraJumps;

        public static ConfigEntry<bool> M3FlowPlayMusic;
        public static ConfigEntry<float> M3FlowMusicVolume;

        public static ConfigEntry<float> M4MaidProjectileDamage;
        public static ConfigEntry<float> M4MaidImpactDamageMultiplier;
        public static ConfigEntry<float> M4MaidBeamsDamageMultiplier;
        public static ConfigEntry<int> M4MaidBeamsAmount;

        public static ConfigEntry<float> M4SlamDamage;
        public static ConfigEntry<float> M4SlamLiftForce;
        public static ConfigEntry<float> M4SlamAntiGravDuration;

        public static ConfigEntry<float> SpikeDamage;
        public static ConfigEntry<float> SpikeDamagePerMeterFell;
        public static ConfigEntry<float> SpikeImpactLiftForce;
        public static ConfigEntry<float> SpikeInitialSpeed;
        public static ConfigEntry<float> SpikeSpeedGrowth;

        public static void Init()
        {

            string sectionWyatt = "Custodian";

            #region m1
            M1Damage = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M1 - M1Damage",
                1f,
                "value",
                0f,
                10,
                false);

            M1DamageFinisher = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M1 - M1DamageFinisher",
                2f,
                "value",
                0f,
                10,
                false);

            M1AttackDuration = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M1 - M1AttackDuration",
                0.5f,
                "value",
                0.01f,
                10,
                false);

            M1AttackDurationFinisher = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M1 - M1AttackDurationFinisher",
                0.8f,
                "value",
                0.01f,
                10,
                false);

            M1UpwardsLiftForce = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M1 - M1UpwardsLiftForce",
                7f,
                "value",
                0,
                50,
                false);
            M1KnockbackForce = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M1 - M1KnockbackForce",
                3f,
                "value",
                0,
                50,
                false);
            M1AntiGravDuration = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M1 - M1AntiGravDuration",
                1.5f,
                "value",
                0,
                10,
                false);
            #endregion m1

            #region m2
            M2Damage = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M2 - M2Damage",
                3f,
                "value",
                0f,
                10,
                false);
            #endregion m2

            #region m3
            M3GrooveSpeedMultiplierPerStack = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M3 - M3GrooveSpeedGainedPerStack",
                0.2f,
                "value",
                0f,
                5,
                false);
            M3FlowDurationBase = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M3 - M3FlowDurationBase",
                4f,
                "value",
                0f,
                10,
                false);
            M3FlowDurationPerStack = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M3 - M3FlowDurationPerStack",
                0.4f,
                "value",
                0f,
                10,
                false);
            M3FlowArmorBase = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M3 - M3FlowArmorBase",
                30f,
                "value",
                10f,
                100,
                false);
            M3FlowArmorPerStack = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M3 - M3FlowArmorPerStack",
                5f,
                "value",
                0f,
                50,
                false);

            M3FlowCDR = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M3 - M3FlowCDR",
                0.3f,
                "value",
                0f,
                10,
                false);
            M3FlowExtraJumps = Config.BindAndOptions<int>(
                sectionWyatt,
                "M3 - M3FlowExtraJumps",
                1,
                "value", 
                false);

            M3FlowPlayMusic = Config.BindAndOptions<bool>(
                sectionWyatt,
                "M3 - M3FlowPlayMusic",
                true,
                "set false to disable jingle during flow",
                false);
            M3FlowMusicVolume = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M3 - M3FlowMusicVolume",
                50,
                "value",
                0f,
                100,
                false);
            #endregion m3

            #region m4
            M4MaidProjectileDamage = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M4 - M4MaidProjectileDamage",
                3f,
                "value",
                0f,
                10,
                false);
            M4MaidImpactDamageMultiplier = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M4 - M4MaidImpactDamageMultiplier",
                1f,
                "value",
                0f,
                10,
                true);
            M4MaidBeamsDamageMultiplier = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M4 - M4MaidBeamsDamageMultiplier",
                0.5f,
                "value",
                0f,
                10,
                true);
            M4MaidBeamsAmount = Config.BindAndOptions<int>(
                sectionWyatt,
                "M4 - M4MaidBeamsAmount",
                0,
                "value",
                true);

            M4SlamDamage = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M4 - M4SlamDamage",
                8f,
                "value",
                0f,
                50,
                false);
            M4SlamLiftForce = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M4 - M4SlamLiftForce",
                10f,
                "value",
                0f,
                100,
                false);
            M4SlamAntiGravDuration = Config.BindAndOptionsSlider(
                sectionWyatt,
                "M4 - M4SlamAntiGravDuration",
                2f,
                "value",
                0,
                10,
                false);
            #endregion m4

            #region misc
            SpikeDamage = Config.BindAndOptionsSlider(
                sectionWyatt,
                "Misc - SpikeDamage",
                2f,
                "value",
                0,
                10,
                false);
            SpikeDamagePerMeterFell = Config.BindAndOptionsSlider(
                sectionWyatt,
                "Misc - SpikeDamagePerMeterFell",
                0.32f,
                "value",
                0,
                10,
                false);
            SpikeImpactLiftForce = Config.BindAndOptionsSlider(
                sectionWyatt,
                "Misc - SpikeImpactLiftForce",
                0f,
                "value",
                0,
                100,
                false);

            SpikeInitialSpeed = Config.BindAndOptionsSlider(
                sectionWyatt,
                "Misc - SpikeInitialSpeed",
                20f,
                "value",
                0,
                200,
                false);

            SpikeSpeedGrowth = Config.BindAndOptionsSlider(
                sectionWyatt,
                "Misc - SpikeSpeedGrowth",
                50f,
                "value",
                0,
                100,
                false);

            #endregion

        }
    }
}