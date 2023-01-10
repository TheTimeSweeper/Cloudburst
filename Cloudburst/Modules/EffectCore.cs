using UnityEngine;

using RoR2;
using TMPro;
using RoR2.UI;

using UnityEngine.Rendering.PostProcessing;
using System.Linq;
using R2API;

namespace Cloudburst.Cores
{
    public class Effects
    {
        public static GameObject wyattSwingTrail;
        public static GameObject wyatt2SwingTrail;


        public static GameObject maidCleanseEffect;

        public static GameObject maidTouchEffect;

        public static GameObject willIsNotPoggers;
        public static GameObject willIsStillTotallyNotPoggers;
        public static GameObject wyattSlam;
        public static GameObject wyattGrooveEffect;

        public static GameObject ericAndreMoment;
        public static GameObject tiredOfTheDingDingDing;

        public static void OnLoaded()
        {
            CreateNewEffects();

            //blackHoleIncisionEffect = CreateAsset("UniversalIncison", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Always);
            wyattSlam = CreateEffect("DebugEffect");//, false, false,      true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Medium);
            maidTouchEffect = CreateEffect("TracerCaptainDefenseMatrix");
            wyattGrooveEffect = CreateEffect("WyattGrooveEffect");
            ericAndreMoment = CreateEffect("WyattHitEffect");
            tiredOfTheDingDingDing = CreateEffect("WyattSpikeEffect");
            //fabinin = CreateEffect("fabin");
        }

        private static void CreateMAIDCleanseEffect()
        {
            maidCleanseEffect = CreateAsset("MAIDCleanEffect", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Always);
        }

        private static void WillIsStillNotPoggers()
        {
            willIsNotPoggers = CreateAsset("WillIsNotPoggers", false, false, true, "", false, VFXAttributes.VFXIntensity.Low, VFXAttributes.VFXPriority.Always);
            var unfortunatelyWillIsStillNotPoggers = willIsNotPoggers.AddComponent<ShakeEmitter>();

            unfortunatelyWillIsStillNotPoggers.wave = new Wave()
            {
                amplitude = 0.5f,
                cycleOffset = 0,
                frequency = 100,
            };
            unfortunatelyWillIsStillNotPoggers.duration = 0.5f;
            unfortunatelyWillIsStillNotPoggers.radius = 51;
            unfortunatelyWillIsStillNotPoggers.amplitudeTimeDecay = true;
        }

        private static void WillIsStillNotPoggersMonthsLater()
        {
            willIsStillTotallyNotPoggers = CreateAsset("WyattSuperJumpEffect", false, false, true, "", false, VFXAttributes.VFXIntensity.Low, VFXAttributes.VFXPriority.Always);
            var unfortunatelyWillIsStillNotPoggers = willIsStillTotallyNotPoggers.AddComponent<ShakeEmitter>();

            unfortunatelyWillIsStillNotPoggers.wave = new Wave()
            {
                amplitude = 0.5f,
                cycleOffset = 0,
                frequency = 100,
            };
            unfortunatelyWillIsStillNotPoggers.duration = 0.5f;
            unfortunatelyWillIsStillNotPoggers.radius = 51;
            unfortunatelyWillIsStillNotPoggers.amplitudeTimeDecay = true;
        }

        protected static void CreateNewEffects()
        {
            CreateMAIDCleanseEffect();
            WillIsStillNotPoggersMonthsLater();
            WillIsStillNotPoggers();
        }

        public static GameObject CreateEffect(string name)
        {
            GameObject obj = Modules.Assets.LoadAsset<GameObject>(name);

            R2API.ContentAddition.AddEffect(obj);

            //Content.ContentHandler.Effects.RegisterEffect(new EffectDef()
            //{
            //    prefab = obj,
            //});
            return obj;
        }

        public static GameObject CreateAsset(string name, bool positionAtReferencedTransform, bool parentToReferencedTransform, bool applyScale, string soundName, bool disregardZScale, VFXAttributes.VFXIntensity intensity, VFXAttributes.VFXPriority priority)
        {
            GameObject obj = Modules.Assets.LoadAsset<GameObject>(name);
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

            R2API.ContentAddition.AddEffect(obj);

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
