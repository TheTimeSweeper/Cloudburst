using RoR2;
using RoR2.ContentManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cloudburst.Modules
{
    internal static class ItemDisplayCheck
    {
        public static List<Object> allDisplayedItems = null;

        private static Object[] vanillaItems = null;
        private static Object[] dlc1Items = null;
        private static Object[] junkItems = null;

        private static void GatherAllItems()
        {
            allDisplayedItems = new List<Object>(ItemDisplays.KeyAssetDisplayPrefabs.Keys);

            for (int i = 0; i < ContentManager.allLoadedContentPacks.Length; i++)
            {
                ReadOnlyContentPack contentPack = ContentManager.allLoadedContentPacks[i];

                switch (contentPack.identifier)
                {
                    case "RoR2.BaseContent":
                        vanillaItems = contentPack.itemDefs.ToArray();
                        vanillaItems = vanillaItems.Concat(contentPack.equipmentDefs.ToArray()).ToArray();
                        break;
                    case "RoR2.DLC1":
                        dlc1Items = contentPack.itemDefs.ToArray();
                        dlc1Items = dlc1Items.Concat(contentPack.equipmentDefs.ToArray()).ToArray();
                        break;
                    case "RoR2.Junk":
                        junkItems = contentPack.itemDefs.ToArray();
                        junkItems = junkItems.Concat(contentPack.equipmentDefs.ToArray()).ToArray();
                        break;
                }
            }


            allDisplayedItems.Sort((item1, item2) => {
                ////sort itemdefs by tier
                //if (item1 is ItemDef && item2 is ItemDef)
                //{
                //    return (item1 as ItemDef).tier.CompareTo((item2 as ItemDef).tier);
                //}

                //sort by assetname
                if (item1 is ItemDef && item2 is ItemDef)
                {
                    return (item1 as ItemDef).name.CompareTo((item2 as ItemDef).name);
                }
                if (item1 is EquipmentDef && item2 is EquipmentDef)
                {
                    return (item1 as EquipmentDef).name.CompareTo((item2 as EquipmentDef).name);
                }
                //sort equipmentedefs last
                if (item1 is ItemDef && item2 is EquipmentDef)
                {
                    return -1;
                }
                if (item1 is EquipmentDef && item2 is ItemDef)
                {
                    return 1;
                }

                return 0;
            });


            ////filter out equipmentdefs and only keep item tiers we're worried about
            ////evolution only gives monsters tier 1 2 and 3 items. I suppose with mods they can get other tiers but we'll keep the scope low for now
            //for (int i = allDisplayedItems.Count - 1; i >= 0; i--)
            //{
            //    ItemDef item = allDisplayedItems[i] as ItemDef;

            //    if (!item)
            //    {
            //        allDisplayedItems.Remove(allDisplayedItems[i]);
            //        continue;
            //    }

            //    if (item.tier != ItemTier.Tier1 &&
            //        item.tier != ItemTier.Tier2 &&
            //        item.tier != ItemTier.Tier3)
            //    {
            //        allDisplayedItems.Remove(item);
            //    }
            //}

        }

        public static void PrintUnused(ItemDisplayRuleSet itemDisplayRuleSet, string bodyName)
        {
            PrintUnused(itemDisplayRuleSet.keyAssetRuleGroups.ToList(), bodyName);
        }

        public static void PrintUnused(IEnumerable<ItemDisplayRuleSet.KeyAssetRuleGroup> ruleSet, string bodyName)
        {
            string missingDisplays = $"generating item displays for {bodyName}";

            //grab all keyassets
            if(allDisplayedItems == null)
            GatherAllItems();

            //remove from list keyassets that we already have displays for
            List<Object> missingKeyAssets = new List<Object>(allDisplayedItems);
            string firstCompatibleChild = "";
            foreach (ItemDisplayRuleSet.KeyAssetRuleGroup ruleGroup in ruleSet)
            {
                if (ruleGroup.displayRuleGroup.rules.Length == 0)
                    continue;

                missingKeyAssets.Remove(ruleGroup.keyAsset);
                if (string.IsNullOrEmpty(firstCompatibleChild))
                {
                    firstCompatibleChild = ruleGroup.displayRuleGroup.rules[0].childName;
                }
            }

            //print all display rules
            foreach (Object keyAsset in missingKeyAssets)
            {              
                //grab the contentpack from which the keyasset is from
                string content = "MISSING";
                if (vanillaItems.Contains(keyAsset))
                {
                    content = "RoR2Content";
                }
                if (dlc1Items.Contains(keyAsset))
                {
                    content = "DLC1Content";
                }
                if (junkItems.Contains(keyAsset))
                {
                    content = "JunkContent";
                }

                string thing = $"";
                if (ItemDisplays.KeyAssetDisplayPrefabs.ContainsKey(keyAsset))
                {
                    //if we have a displayprefab for it (Populated in ItemDisplays.PopulateDisplays),
                        //generate a rule formatted to the code in this project
                    thing += SpitOutNewRule(keyAsset, content, firstCompatibleChild, ItemDisplays.KeyAssetDisplayPrefabs[keyAsset]);
                }
                else
                {
                    Log.Error($"COULD NOT FIND DISPLAY PREFABS FOR KEYASSET {keyAsset}");
                }
                missingDisplays += thing;
            }
            //add them all to a giant string and print it out, formatted
            Log.Message(missingDisplays);
        }

        private static string SpitOutNewRule(Object asset, string content, string firstCompatibleChild, List<string> prefabNames)
        {
            string contentType = asset is ItemDef ? "Items" : "Equipment";

            if (prefabNames.Count == 0)
                return $"\n[NO PREFABS FOUND FOR THE KEYASSET {asset}";

            ////generate a simple rule
            //if (prefabNames.Count == 1)
            //{
            //    return $"\n            itemDisplayRules.Add(ItemDisplays.CreateGenericDisplayRule({content}.{contentType}.{asset.name}, \"{prefabNames[0]}\",\n" +
            //             $"                \"{firstCompatibleChild}\",\n" +
            //              "                new Vector3(2, 2, 2),\n" +
            //              "                new Vector3(0, 0, 0),\n" +
            //              "                new Vector3(1, 1, 1)));";
            //}

            //always generate a rule like this
            //if there are multiple display prefabs int for the same item (gravboots, afterburners),
                //create a multiple-prefab display rule like this
            string newRule = $"\n            itemDisplayRules.Add(ItemDisplays.CreateDisplayRuleGroupWithRules({content}.{contentType}.{asset.name}";
            for (int i = 0; i < prefabNames.Count; i++)
            {
                newRule += ",\n" + 
                         $"                ItemDisplays.CreateDisplayRule(ItemDisplays.LoadDisplay(\"{prefabNames[i]}\"),\n" +
                         $"                    \"{firstCompatibleChild}\",\n" +
                          "                    new Vector3(2, 2, 2),\n" +
                          "                    new Vector3(0, 0, 0),\n" +
                          "                    new Vector3(1, 1, 1)\n" +
                          "                )";
            }
            newRule += "));";
            return newRule;
        }
    }
}