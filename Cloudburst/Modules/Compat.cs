using Cloudburst.Characters.Wyatt;
using On.RoR2;
using System;
using UnityEngine;

namespace Cloudburst.Modules
{
    public class Compat
    {
        public static bool RiskOfOptionsInstalled;
        public static bool MemeInstalled;

        public static void Init()
        {
            RiskOfOptionsInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
            MemeInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI");
        }
    }
}