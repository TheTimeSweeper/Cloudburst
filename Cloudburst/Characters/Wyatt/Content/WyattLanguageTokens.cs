using System;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattLanguageTokens
    {
        public static void AddLanguageTokens(string WYATT_PREFIX)
        {
            #region body
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
            #endregion body
        }
    }
}