using System;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattLanguageTokens
    {
        public static void AddLanguageTokens()
        {
            string WYATT_PREFIX = WyattSurvivor.WYATT_PREFIX;

            #region body
            R2API.LanguageAPI.Add(WYATT_PREFIX + "NAME", "Custodian");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "SUBTITLE", "Lean, Mean, Cleaning Machines");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "OUTRO_FLAVOR", "...and so they left, a job well done");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "OUTRO_FAILURE", "...and so they vanished, leaving a bigger mess than when they started");

            R2API.LanguageAPI.Add(WYATT_PREFIX + "DESCRIPTION",
                "The Custodian is a master of janitorial warfare who manipulates his foes' gravity to his advantage<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
                + "< ! > Chain combos of Grav-Broom to lift enemies higher and higher." + Environment.NewLine + Environment.NewLine
                + "< ! > Spike enemies with Trash Out after they've been lifted in the air with Grav-Broom and MAID for major damage!" + Environment.NewLine + Environment.NewLine
                + "< ! > Use the double jump from Flow to combo enemies in the air, or use the armor to get out of a sticky situation." + Environment.NewLine + Environment.NewLine
                + "< ! > Use MAID to lift enemies into the air, and to reposition yourself to combo enemies further." + Environment.NewLine + Environment.NewLine);

            R2API.LanguageAPI.Add(WYATT_PREFIX + "LORE", @"Can't stop now. Can't stop now. Every step I take is a step I can't take back. Come hell or high water I will find it.

It's all a rhythm, just a rhythm. Every time I step out of line is a punishment. I will obey the groove. Nothing can stop me now.

Every scar is worth it. I can feel it, I am coming closer to it. I will have it, and it will be mine.

No matter the blood, it's worth it. I will find what I want, and I will come home.

No matter how many I slaughter, it will be mine... It can't hide from me from me forever.

It's here, I can feel it. This security chest, it has it. I crack it open, and I find it.

It's... finally mine. I hold it in my bruised hands. Has it really been years? 

She'll love this, I know.

");
            #endregion body

            #region skills

            R2API.LanguageAPI.Add("KEYWORD_WEIGHTLESS", "<style=cKeywordName>Weightless</style><style=cSub><style=cIsUtility>Removes gravity</style> from target and <style=cIsUtility>Slows movement and attack speed</style>.</style>");
            R2API.LanguageAPI.Add("KEYWORD_SPIKED", "<style=cKeywordName>Spiking</style><style=cSub>Forces an enemy to fall downwards, causing a <style=cIsDamage>shockwave</style> if they impact terrain, dealing <style=cIsDamage>200% damage</style> + <style=cIsDamage>30% damage</style> per meter fallen.</style>");
            R2API.LanguageAPI.Add("KEYWORD_FLOW", "<style=cKeywordName>Flow</style><style=cSub>Gain <style=cIsDamage>30 armor</style> + <style=cIsDamage>5 armor</style> for each stack of <style=cIsUtility>Groove</style>.\nGain a <style=cIsUtility>Double Jump</style>.\nGain <style=cIsUtility>+30% Cooldown Reduction.</style></style>");
            
            R2API.LanguageAPI.Add(WYATT_PREFIX + "PASSIVE_NAME", "Walkman");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "PASSIVE_DESCRIPTION", "On hit, gain a stack of <style=cIsUtility>Groove</style>, granting <style=cIsUtility>20% move speed</style> per stack. Diminishes out of combat.");

            R2API.LanguageAPI.Add(WYATT_PREFIX + "PRIMARY_COMBO_NAME", "G22 Grav-Broom");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "PRIMARY_COMBO_DESCRIPTION", "<style=cIsUtility>Agile</style>. Swing your broom for <style=cIsDamage>100% damage</style>. Every third hit deals <style=cIsDamage>200% damage</style> and applies <style=cIsUtility>weightless</style>.");

            R2API.LanguageAPI.Add(WYATT_PREFIX + "SECONDARY_TRASHOUT_NAME", "Trash Out");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "SECONDARY_TRASHOUT_DESCRIPTION", "<s>Deploy a winch</s> Magically fly towards an enemy, hit them for <style=cIsDamage>300%</style> damage and <style=cIsDamage>spike</style> them.");

            R2API.LanguageAPI.Add(WYATT_PREFIX + "UTILITY_FLOW_NAME", "Flow");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "UTILITY_FLOW_DESCRIPTION", "Activate <style=cIsDamage>Flow</style> for <style=cIsUtility>4 seconds</style> + <style=cIsUtility>0.4 seconds</style> for each stack of <style=cIsUtility>Groove</style>, gaining <style=cIsDamage>armor, a double jump, and cooldown reduction</style>. During <style=cIsDamage>Flow</style>, you are unable to lose or gain <style=cIsUtility>Groove</style>. After <style=cIsDamage>Flow</style> ends, lose all stacks of <style=cIsUtility>Groove</style>.");

            R2API.LanguageAPI.Add(WYATT_PREFIX + "SPECIAL_MAID_NAME", "M88 MAID");
            R2API.LanguageAPI.Add(WYATT_PREFIX + "SPECIAL_MAID_DESCRIPTION", "Send your <style=cIsUtility>MAID</style> unit barreling through enemies for <style=cIsDamage>300% damage</style> before stopping briefly and returning to you. Activate again to reel toward the <style=cIsUtility>MAID</style> and explode for <style=cIsDamage>500% damage</style>, applying <style=cIsUtility>Weightless</style>.");
            #endregion

            #region skins
            R2API.LanguageAPI.Add(WYATT_PREFIX + "CLASSIC_SKIN", "Classic");
            R2API.LanguageAPI.Add("ACHIEVEMENT_CLOUDBURSTWYATTCLEARGAMEMONSOON_NAME", "Custodian: Mastery");
            R2API.LanguageAPI.Add("ACHIEVEMENT_CLOUDBURSTWYATTCLEARGAMEMONSOON_DESCRIPTION", "As Custodian, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}