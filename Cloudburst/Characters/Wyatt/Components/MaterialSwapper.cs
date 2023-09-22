using Cloudburst;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MaterialSwapper : MonoBehaviour
{
    private static Dictionary<string, string> _nameToAddressable;
    public static Dictionary<string, string> nameToAddressable {
        get {
            if(_nameToAddressable == null)
            {
                InitDictionary();
            }
            return _nameToAddressable;
        }
    }

    public static void InitDictionary()
    {
        _nameToAddressable = new Dictionary<string, string>();
        _nameToAddressable["matOpaqueDustTrail"] = "RoR2/Base/Common/VFX/matOpaqueDustTrail.mat";
        _nameToAddressable["matDistortionFaded"] = "RoR2/Base/Common/VFX/matDistortionFaded.mat";
        _nameToAddressable["matOmniRingRock"] = "RoR2/Base/skymeadow/matOmniRingRock.mat";
        _nameToAddressable["matGolemExplosion"] = "RoR2/Base/Common/VFX/matGolemExplosion.mat";
        
        _nameToAddressable["matOpaqueDustLarge"] = "RoR2/Base/Common/VFX/matOpaqueDustLarge.mat";
        _nameToAddressable["matDebris1"] = "RoR2/Base/Common/VFX/matDebris1.mat";
        _nameToAddressable["matSandDetailRock"] = "RoR2/Base/Common/Props/matSandDetailRock.mat";
        
        _nameToAddressable["matBaubleEffect"] = "RoR2/Base/SlowOnHit/matBaubleEffect.mat";
        _nameToAddressable["matCleanseCore"] = "RoR2/Base/Cleanse/matCleanseCore.mat";
        _nameToAddressable["matCleanseWater"] = "RoR2/Base/Cleanse/matCleanseWater.mat";
        
        _nameToAddressable["matTracerBright"] = "RoR2/Base/Common/VFX/matTracerBright.mat";
    }

    public static void RunSwappers(GameObject gob)
    {
        foreach (MaterialSwapper swapper in gob.GetComponentsInChildren<MaterialSwapper>())
        {
            swapper.SwapMaterial();
        }
    }

    public string materialName = "";

    public void SwapMaterial()
    {
        string address = materialName;
        if (nameToAddressable.ContainsKey(materialName))
        {
            address = nameToAddressable[materialName];
        }
        GetComponent<Renderer>().material = Addressables.LoadAssetAsync<Material>(address).WaitForCompletion();
    }
}
