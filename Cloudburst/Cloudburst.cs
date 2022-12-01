using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]
    public class Cloudburst : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "WHO";
        public const string PluginName = "Cloudburst";
        public const string PluginVersion = "0.0.0";

        public void Awake()
        {
            Log.Init(Logger);

            Log.LogInfo(nameof(Awake) + " done.");
        }
    }
}
