using Cloudburst.Modules;
using RoR2;
using UnityEngine;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattUnlockables
    {
        public static UnlockableDef masteryUnlockable;

        public static void Init()
        {
            masteryUnlockable = Unlockables.CreateUnlockableDef(
                "cloudburst.skins.wyatt.mastery",
                WyattSurvivor.WYATT_PREFIX + "MASTERY_SKIN",
                Asset.LoadAsset<Sprite>("texIconWyattSkinClassic"));
        }
    }
}
