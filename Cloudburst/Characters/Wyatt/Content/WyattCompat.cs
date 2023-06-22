using Cloudburst.Modules;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattCompat
    {
        public static void Init()
        {
            if (Modules.Compat.MemeInstalled)
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            GameObject skele = Assets.LoadAsset<GameObject>("WyattMeme");
            EmotesAPI.CustomEmotesAPI.ImportArmature(WyattSurvivor.instance.bodyPrefab, skele, false);
            //skele.GetComponentInChildren<BoneMapper>().scale = 1.5f;

            orig();
        }

    }
}