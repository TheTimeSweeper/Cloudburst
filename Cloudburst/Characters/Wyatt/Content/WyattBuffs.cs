using UnityEngine;

using RoR2;
using Cloudburst.Modules;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattBuffs
    {
        public static BuffDef wyattGrooveBuffDef;
        public static BuffDef wyattFlowBuffDef;
        public static BuffDef wyattAntiGravBuffDef;

        public static void Init()
        {
            wyattGrooveBuffDef = Buffs.AddNewBuff(
                "CloudburstWyattCombatBuff",
                Asset.LoadAsset<Sprite>("WyattVelocity"),
                new Color(1f, 0.7882353f, 0.05490196f),
                true,
                false);

            wyattFlowBuffDef = Buffs.AddNewBuff(
                "CloudburstWyattFlowBuff",
                Asset.LoadAsset<Sprite>("WyattVelocity"),
                CCUtilities.HexToColor("69FFC2"),
                false,
                false);

            wyattAntiGravBuffDef = Buffs.AddNewBuff(
                "CloudburstWyattAntiGravBuff",
                Asset.LoadAsset<Sprite>("texIconBuffAntiGrav"),
                new Color(0.6784314f, 0.6117647f, 0.4117647f),
                false,
                true);
        }

    }
}
