using BepInEx;
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
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cloudburst
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]
    public class Cloudburst : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "CloudBurstTeam";
        public const string PluginName = "Cloudburst";
        public const string PluginVersion = "1.0.0";

        private static ExpansionDef dlc1 = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();

        public static AssetBundle CloudburstAssets;
        public static AssetBundle OldCloudburstAssets;

        public static ExpansionDef cloudburstExpansion;

        public void Awake()
        {
            Log.Init(Logger);

            GetBundle();

            cloudburstExpansion = ScriptableObject.CreateInstance<ExpansionDef>();
            cloudburstExpansion.nameToken = "EXPANSION_CLOUDBURST_NAME";
            cloudburstExpansion.descriptionToken = "EXPANSION_CLOUDBURST_DESCRIPTION";
            cloudburstExpansion.iconSprite = CloudburstAssets.LoadAsset<Sprite>("CloudburstExpansionIcon");
            cloudburstExpansion.disabledIconSprite = dlc1.disabledIconSprite;

            ContentAddition.AddExpansionDef(cloudburstExpansion);

            LanguageAPI.Add("EXPANSION_CLOUDBURST_NAME", "Cloudburst");
            LanguageAPI.Add("EXPANSION_CLOUDBURST_DESCRIPTION", "Adds content from the 'Cloudburst' mod to the game.");

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            SetupItems();
           

            Log.Info(nameof(Awake) + " done.");
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1 != default && arg1.name == "title")
            {
                var menu = GameObject.Find("MainMenu");
                var title = menu.transform.Find("MENU: Title/TitleMenu/SafeZone/ImagePanel (JUICED)/LogoImage");
                var indicator = menu.transform.Find("MENU: Title/TitleMenu/MiscInfo/Copyright/Copyright (1)");

                var build = indicator.GetComponent<HGTextMeshProUGUI>();

                build.fontSize += 4;
                build.text = build.text + Environment.NewLine + $"Cloudburst Version: " + PluginVersion;

                title.GetComponent<Image>().sprite = CloudburstAssets.LoadAsset<Sprite>("texCloudburstLogo");
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
            RiftBubble.Setup();
        }

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
                    using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Cloudburst.Assets.oldcloudburst"))
                    {
                        OldCloudburstAssets = AssetBundle.LoadFromStream(assetStream);
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
