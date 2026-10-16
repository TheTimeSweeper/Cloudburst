using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Green.RestlessRings
{
    //lives on the item display, runs on every client. buffs are networked so we just read them off the body
    public class RestlessRingsGlow : MonoBehaviour
    {
        private ItemFollowerSmooth itemFollower;
        private CharacterModel characterModel;
        private GameObject cachedFollowerInstance;
        private Renderer ringRenderer;
        private ParticleSystem healthEmitter;
        private BuffDef shownBuff;

        private static Material[] armorMaterials;
        private static Material[] attackSpeedMaterials;
        private static Material[] regenMaterials;

        private void Start()
        {
            itemFollower = GetComponent<ItemFollowerSmooth>();
            characterModel = GetComponentInParent<CharacterModel>();

            if (armorMaterials == null)
            {
                armorMaterials = new Material[] { RestlessRings.armorMaterial, RestlessRings.basicMaterial };
                attackSpeedMaterials = new Material[] { RestlessRings.attackSpeedMaterial, RestlessRings.basicMaterial };
                regenMaterials = new Material[] { RestlessRings.regenMaterial, RestlessRings.basicMaterial };
            }
        }

        private void FixedUpdate()
        {
            if (!characterModel || !characterModel.body || !itemFollower)
                return;

            GameObject followerInstance = itemFollower.followerInstance;
            if (!followerInstance)
                return;

            //follower gets rebuilt when visibility changes
            if (followerInstance != cachedFollowerInstance)
            {
                cachedFollowerInstance = followerInstance;
                Transform cube = followerInstance.transform.Find("RINGSNEW/Cube");
                ringRenderer = cube ? cube.GetComponent<Renderer>() : null;
                Transform emitter = cube ? cube.Find("HealthEmitter") : null;
                healthEmitter = emitter ? emitter.GetComponent<ParticleSystem>() : null;
                shownBuff = null;
            }

            BuffDef currentBuff = null;
            for (int i = 0; i < RestlessRings.ringBuffs.Length; i++)
            {
                if (characterModel.body.HasBuff(RestlessRings.ringBuffs[i]))
                {
                    currentBuff = RestlessRings.ringBuffs[i];
                    break;
                }
            }

            if (currentBuff == shownBuff || currentBuff == null)
                return;

            shownBuff = currentBuff;

            if (ringRenderer)
            {
                if (currentBuff == RestlessRings.armorBuff)
                    ringRenderer.sharedMaterials = armorMaterials;
                else if (currentBuff == RestlessRings.attackSpeedBuff)
                    ringRenderer.sharedMaterials = attackSpeedMaterials;
                else
                    ringRenderer.sharedMaterials = regenMaterials;
            }

            if (currentBuff == RestlessRings.regenBuff && healthEmitter)
            {
                healthEmitter.Emit(5);
            }
        }
    }
}
