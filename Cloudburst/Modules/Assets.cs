using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;
using System;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Modules
{
    internal static class Assets
    {
        public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object {
            for (int i = 0; i < Cloudburst.AssetBundles.Count; i++) {

                if (Cloudburst.AssetBundles[i].Contains(assetName))
                {
                    return Cloudburst.AssetBundles[i].LoadAsset<T>(assetName);
                }
            }
            
            Log.Error($"Could not load asset {assetName} from assetbundles");
            return null;
        }

        public static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        public static GameObject CloneAndColorEffect(string addressablesPath, Color color, string name, bool network = true)
        {

            GameObject MercSwordSlash = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(addressablesPath).WaitForCompletion(), name, network);

            recolorEffects(color, MercSwordSlash);

            return MercSwordSlash;
        }

        public static GameObject CloneAndColorEffectLegacy(string legacyPath, Color color, string name)
        {

            GameObject MercSwordSlash = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>(legacyPath), name);

            recolorEffects(color, MercSwordSlash);

            return MercSwordSlash;
        }

        public static void recolorEffects(Color color, GameObject MercSwordSlash)
        {
            ParticleSystemRenderer[] rends = MercSwordSlash.GetComponentsInChildren<ParticleSystemRenderer>();

            foreach (ParticleSystemRenderer rend in rends)
            {
                rend.material.SetColor("_MainColor", color);
                rend.material.SetColor("_Color", color);
                rend.material.SetColor("_TintColor", color);
            }

            //didn't work so saving the processing
            //ParticleSystem[] particles = MercSwordSlash.GetComponentsInChildren<ParticleSystem>();

            //foreach (ParticleSystem particleSystem in particles) {

            //    ParticleSystem.MainModule main = particleSystem.main;
            //    main.startColor = color;
            //}
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            R2API.ContentAddition.AddNetworkSoundEventDef(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (Renderer rend in objectToConvert.GetComponentsInChildren<Renderer>())
            {
                if (rend is ParticleSystemRenderer)
                    continue;

                rend?.material?.SetHopooMaterial();
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }


        public static GameObject LoadSurvivorModel(string modelName) {
            GameObject model = LoadAsset<GameObject>(modelName);
            if (model == null) {
                Log.Error("Trying to load a null model- check to see if the BodyName in your code matches the prefab name of the object in Unity\nFor Example, if your prefab in unity is 'mdlHenry', then your BodyName must be 'Henry'");
                return null;
            }

            return PrefabAPI.InstantiateClone(model, model.name, false);
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        public static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false);
        }

        public static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false);
        }

        public static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform);
        }

        public static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {
            GameObject newEffect = Assets.LoadAsset<GameObject>(resourceName);

            if (!newEffect)
            {
                Log.Error("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        public static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        public static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            R2API.ContentAddition.AddEffect(effectPrefab);
            //EffectDef newEffectDef = new EffectDef();
            //newEffectDef.prefab = effectPrefab;
            //newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            //newEffectDef.prefabName = effectPrefab.name;
            //newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            //newEffectDef.spawnSoundEventName = soundName;

            //R2API.ContentAddition.AddEffect(newEffectDef);
        }
    }
}