using BepInEx;
using Cloudburst.Items.Gray;
using Cloudburst.Items.Gray.RiftBubble;
using Cloudburst.Items.Green;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.ExpansionManagement;
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

        private static ExpansionDef dlc1 = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();

        public static AssetBundle CloudburstAssets;
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
