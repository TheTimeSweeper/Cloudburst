using UnityEngine;

using RoR2;
using TMPro;
using RoR2.UI;

using UnityEngine.Rendering.PostProcessing;
using System.Linq;
using R2API;
using System;
using Cloudburst.Modules;

namespace Cloudburst.Characters.Wyatt
{
    public class WyattEffects
    {
        public static GameObject wyattSwingTrail;
        public static GameObject wyatt2SwingTrail;


        public static GameObject maidCleanseEffect;

        public static GameObject maidTouchEffect;
        public static GameObject maidTriggerEffect;

        //public static GameObject willIsNotPoggers;
        //public static GameObject willIsStillTotallyNotPoggers;
        //public static GameObject wyattSlam;
        //public static GameObject wyattGrooveEffect;

        public static GameObject ericAndreMoment;
        public static GameObject tiredOfTheDingDingDing;

        public static GameObject bigZapEffectPrefabArea;

        public static GameObject notMercSlashEffect;
        public static GameObject notMercSlashEffectThicc;

        public static void OnLoaded()
        {
            CreateNewEffects();

            //blackHoleIncisionEffect = CreateAsset("UniversalIncison", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Always);
            //wyattSlam = CreateEffect("DebugEffect");//, false, false,      true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Medium);
            maidTouchEffect = CreateEffect("TracerCaptainDefenseMatrix");
            //wyattGrooveEffect = CreateEffect("WyattGrooveEffect");
            ericAndreMoment = CreateEffect("WyattHitEffect");
            tiredOfTheDingDingDing = CreateEffect("WyattSpikeEffect");
            //fabinin = CreateEffect("fabin");

            bigZapEffectPrefabArea = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/lightningstakenova");

            //maidTriggerEffect = CreateEffect("MAIDTriggerEffect");
            maidCleanseEffect = CreateAsset("MAIDCleanEffect", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Always);

            createMercSlashesIMean();
        }

        private static void createMercSlashesIMean()
        {
            notMercSlashEffect = Assets.CloneAndColorEffect("RoR2/Base/Merc/MercSwordSlash.prefab", new Color(1, 0.015f, 0), "OrangeWyattMercSwordSlash");
            notMercSlashEffect.transform.Find("SwingTrail").localScale = new Vector3(1.5f, 1.7f, 3);
            ContentAddition.AddEffect(notMercSlashEffect);

            notMercSlashEffectThicc = PrefabAPI.InstantiateClone(notMercSlashEffect, "OrangeWyattMercSwordSlashThicc", false);
            notMercSlashEffectThicc.transform.Find("SwingTrail").localScale = new Vector3(2, 2, 10);
            ContentAddition.AddEffect(notMercSlashEffectThicc);
        }

        protected static void CreateNewEffects()
        {
            //WillIsStillNotPoggersMonthsLater();
            //WillIsStillNotPoggers();
        }

        //private static void WillIsStillNotPoggers()
        //{
        //    willIsNotPoggers = CreateAsset("WillIsNotPoggers", false, false, true, "", false, VFXAttributes.VFXIntensity.Low, VFXAttributes.VFXPriority.Always);
        //    var unfortunatelyWillIsStillNotPoggers = willIsNotPoggers.AddComponent<ShakeEmitter>();

        //    unfortunatelyWillIsStillNotPoggers.wave = new Wave()
        //    {
        //        amplitude = 0.5f,
        //        cycleOffset = 0,
        //        frequency = 100,
        //    };
        //    unfortunatelyWillIsStillNotPoggers.duration = 0.5f;
        //    unfortunatelyWillIsStillNotPoggers.radius = 51;
        //    unfortunatelyWillIsStillNotPoggers.amplitudeTimeDecay = true;
        //}

        //private static void WillIsStillNotPoggersMonthsLater()
        //{
        //    willIsStillTotallyNotPoggers = CreateAsset("WyattSuperJumpEffect", false, false, true, "", false, VFXAttributes.VFXIntensity.Low, VFXAttributes.VFXPriority.Always);
        //    var unfortunatelyWillIsStillNotPoggers = willIsStillTotallyNotPoggers.AddComponent<ShakeEmitter>();

        //    unfortunatelyWillIsStillNotPoggers.wave = new Wave()
        //    {
        //        amplitude = 0.5f,
        //        cycleOffset = 0,
        //        frequency = 100,
        //    };
        //    unfortunatelyWillIsStillNotPoggers.duration = 0.5f;
        //    unfortunatelyWillIsStillNotPoggers.radius = 51;
        //    unfortunatelyWillIsStillNotPoggers.amplitudeTimeDecay = true;
        //}

        public static GameObject CreateEffect(string name)
        {
            GameObject obj = Modules.Assets.LoadAsset<GameObject>(name);
            MaterialSwapper.RunSwappers(obj);

            ContentAddition.AddEffect(obj);
            return obj;
        }

        public static GameObject CreateAsset(string name, bool positionAtReferencedTransform, bool parentToReferencedTransform, bool applyScale, string soundName, bool disregardZScale, VFXAttributes.VFXIntensity intensity, VFXAttributes.VFXPriority priority)
        {
            GameObject obj = Modules.Assets.LoadAsset<GameObject>(name);
            MaterialSwapper.RunSwappers(obj);
            EffectComponent effectComponent = obj.AddComponent<EffectComponent>();
            VFXAttributes attributes = obj.AddComponent<VFXAttributes>();
            DestroyOnParticleEnd destroyOnEnd = obj.AddComponent<DestroyOnParticleEnd>();

            effectComponent.effectIndex = EffectIndex.Invalid;
            effectComponent.positionAtReferencedTransform = positionAtReferencedTransform;
            effectComponent.parentToReferencedTransform = parentToReferencedTransform;
            effectComponent.applyScale = applyScale;
            effectComponent.soundName = soundName;
            effectComponent.disregardZScale = disregardZScale;

            attributes.vfxIntensity = intensity;
            attributes.vfxPriority = priority;

            ContentAddition.AddEffect(obj);

            //Content.ContentHandler.Effects.RegisterEffect(new EffectDef()
            //{

            //    prefab = obj,
            //    prefabEffectComponent = obj.GetComponent<EffectComponent>(),
            //    prefabVfxAttributes = obj.GetComponent<VFXAttributes>(),
            //    prefabName = obj.name,
            //});
            return obj;
        }
    }
}
