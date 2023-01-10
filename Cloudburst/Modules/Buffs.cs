using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace Cloudburst.Modules
{
    public static class Buffs
    {
        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            R2API.ContentAddition.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}