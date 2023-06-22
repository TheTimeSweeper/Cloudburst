using BepInEx;
using Cloudburst.Characters;
using Cloudburst.Characters.Wyatt;
using Cloudburst.Items.Gray;
using Cloudburst.Items.Gray.BlastBoot;
using Cloudburst.Items.Gray.RiftBubble;
using Cloudburst.Items.Green;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.ExpansionManagement;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cloudburst
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]
    //[R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]
    public class Cloudburst : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "CloudBurstTeam";
        public const string PluginName = "Cloudburst";
        public const string PluginVersion = "1.0.0";

        private static ExpansionDef dlc1 = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();

        public static AssetBundle CloudburstAssets;
        public static AssetBundle OldCloudburstAssets;
        public static AssetBundle WyattAssetBundle;

        public static List<AssetBundle> AssetBundles = new List<AssetBundle>();

        public static ExpansionDef cloudburstExpansion;

        public static Cloudburst instance;

        public void Awake()
        {
            instance = this;

            Log.Init(Logger);

            GetBundle();
            GetSoundBank();

            Modules.Compat.Init();

            cloudburstExpansion = ScriptableObject.CreateInstance<ExpansionDef>();
            cloudburstExpansion.nameToken = "EXPANSION_CLOUDBURST_NAME";
            cloudburstExpansion.descriptionToken = "EXPANSION_CLOUDBURST_DESCRIPTION";
            cloudburstExpansion.iconSprite = CloudburstAssets.LoadAsset<Sprite>("CloudburstExpansionIcon");
            cloudburstExpansion.disabledIconSprite = dlc1.disabledIconSprite;

            ContentAddition.AddExpansionDef(cloudburstExpansion);

            LanguageAPI.Add("EXPANSION_CLOUDBURST_NAME", "Cloudburst");
            LanguageAPI.Add("EXPANSION_CLOUDBURST_DESCRIPTION", "Adds content from the 'Cloudburst' mod to the game.");

            SetupItems();

            Modules.ItemDisplays.PopulateDisplays();

            new WyattSurvivor().Initialize();

            Log.Info(nameof(Awake) + " done.");
        }

        private void GetSoundBank()
        {
            using (var manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Cloudburst.Assets.WyattBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        public void SetupItems()
        {
            //BlastBoot.Setup();
            BismuthEarrings.Setup();
            EnigmaticKeycard.Setup();
            FabinhoruDagger.Setup();
            GlassHarvester.Setup();
            JapesCloak.Setup();
            //RiftBubble.Setup();
        }

        public void GetBundle()
        {
            if (CloudburstAssets == null)
            {
                try
                {
                    CloudburstAssets = LoadAssetBundle("Cloudburst.Assets.cloudburst");
                    WyattAssetBundle = LoadAssetBundle("Cloudburst.Assets.wyatt");
                    OldCloudburstAssets = LoadAssetBundle("Cloudburst.Assets.oldcloudburst");
                }
                catch
                {
                    
                }
            }
            if (CloudburstAssets != null)
            {
                Log.Info("Successfully loaded Asset Bundle");
                ConvertMaterialsIfItWasIncrediblyUnoptimizedAndIDidntCareToImproveEnigmasCode();
            } else
            {
                Log.Error("AAAAAA");
            }
        }

        private static AssetBundle LoadAssetBundle(string name)
        {
            AssetBundle assetBundle;
            using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                assetBundle = AssetBundle.LoadFromStream(assetStream);
            }

            AssetBundles.Add(assetBundle);

            return assetBundle;
        }

        public static Shader hgs = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGStandard.shader").WaitForCompletion();
        public static Shader hgicr = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGIntersectionCloudRemap.shader").WaitForCompletion();
        public static Shader hgcr = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGCloudRemap.shader").WaitForCompletion();
        public static Shader hgsp = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGSolidParallax.shader").WaitForCompletion();
        public static Shader hgdw = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGDistantWater.shader").WaitForCompletion();
        public static Shader hgocr = Addressables.LoadAssetAsync<Shader>("RoR2/Base/Shaders/HGOpaqueCloudRemap.shader").WaitForCompletion();
        public void ConvertMaterialsIfItWasIncrediblyUnoptimizedAndIDidntCareToImproveEnigmasCode()
        {
            var materials = OldCloudburstAssets.LoadAllAssets<Material>();

            //It may be shitty, but it works. Therefore, it is perfect.
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].shader.name == "Standard")
                {
                    materials[i].shader = hgs;
                }
                if (materials[i].name.Contains("GLASS"))
                {
                    materials[i].shader = hgicr;
                }
                switch (materials[i].shader.name)
                {

                    case "Hopoo Games/FX/Cloud Remap Proxy":
                        //LogCore.LogI("material");
                        materials[i].shader = hgcr;
                        //LogCore.LogI(materials[i].shader.name);
                        break;

                    case "stubbed_Hopoo Games/FX/Cloud Remap Proxy":
                        materials[i].shader = hgcr;
                        break;

                    case "stubbed_Hopoo Games/Deferred/Standard Proxy":
                        materials[i].shader = hgs;
                        break;

                    case "Hopoo Games/Deferred/Standard Proxy":
                        materials[i].shader = hgs;
                        break;

                    case "stubbed_Hopoo Games/FX/Solid Parallax Proxy":
                        materials[i].shader = hgsp;
                        break;
                    case "stubbed_Hopoo Games/Environment/Distant Water Proxy":
                        materials[i].shader = hgdw;
                        break;
                    case "Hopoo Games/FX/Cloud Intersection Remap Proxy":
                        materials[i].shader = hgicr;
                        break;
                    case "Hopoo Games/FX/Cloud Opaque Cloud Remap Proxy":
                        materials[i].shader = hgocr;
                        break;
                }
            }
        }
    }
}
