using BepInEx;
using Cloudburst.Items;
using R2API;
using R2API.Utils;
using RoR2;
using System.Reflection;
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

        public static AssetBundle CloudburstAssets;

        public void Awake()
        {
            Log.Init(Logger);

            GetBundle();
            SetupItems();
           

            Log.Info(nameof(Awake) + " done.");
        }

        public void SetupItems()
        {
            BismuthEarrings.Setup();
            FabinhoruDagger.Setup();
            GlassHarvester.Setup();
            JapesCloak.Setup();
            RiftBubble.Setup();
        }

        //Stripped from SomewhereElse
        public void GetBundle()
        {
            if (CloudburstAssets == null)
            {
                try
                {
                    using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Cloudburst.Assets.cloudburst"))
                    {
                        CloudburstAssets = AssetBundle.LoadFromStream(assetStream);
                    }
                }
                catch
                {
                    
                }
            }
            if (CloudburstAssets != null)
            {
                Log.Info("Successfully loaded Asset Bundle");
            } else
            {
                Log.Error("AAAAAA");
            }
        }
    }
}
