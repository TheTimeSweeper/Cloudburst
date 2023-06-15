using Cloudburst.Characters.Wyatt;
using On.RoR2;
using System;
using UnityEngine;

namespace Cloudburst.Modules
{

    public class Compat
    {
        public static bool RiskOfOptionsInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        public static void Init()
        {
            RiskOfOptionsInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

            if(BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {

                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
        }

        private static void SurvivorCatalog_Init(SurvivorCatalog.orig_Init orig)
        {
            GameObject skele = Assets.LoadAsset<GameObject>("WyattMeme");
            EmotesAPI.CustomEmotesAPI.ImportArmature(WyattSurvivor.instance.bodyPrefab, skele, false);
            //skele.GetComponentInChildren<BoneMapper>().scale = 1.5f;

            orig();
        }
    }
}