using EntityStates;
using RoR2;
using UnityEngine;
using Cloudburst.Characters.Wyatt;
using Cloudburst.Wyatt.Components;

namespace Cloudburst.CEntityStates.Wyatt
{
    public class TrashOutHit : BasicMeleeAttack
    {
        public override void OnEnter()
        {
            hitBoxGroupName = "HitboxSwingLarge";
            mecanimHitboxActiveParameter = "BroomSpike.active";
            baseDuration = 0.4f;
            duration = baseDuration / attackSpeedStat;
            hitPauseDuration = 0.2f;
            damageCoefficient = WyattConfig.M2Damage.Value;
            procCoefficient = 1;
            shorthopVelocityFromHit = 0;
            swingEffectPrefab = WyattEffects.notMercSlashEffectThicc;
            swingEffectMuzzleString = "MuzzleSwingSpike";
            hitEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxmedium");
            beginSwingSoundString = "Play_Wyatt_Whoosh";
            impactSound = WyattAssets.hitWhipSound;
            base.OnEnter();
            base.PlayAnimation("FullBody, Override", "kickSwing");
            R2API.DamageAPI.AddModdedDamageType(overlapAttack, WyattDamageTypes.applyGroove);
        }

        public override void BeginMeleeAttackEffect()
        {
            base.BeginMeleeAttackEffect();

            base.characterMotor.velocity = Vector3.up * 18f;
        }

        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();
            WyattNetworkCombat networkCombat = GetComponent<WyattNetworkCombat>();
            for (int i = 0; i < hitResults.Count; i++)
            {
                HurtBox hurtBox = hitResults[i];
                GameObject hurtBodyObject = hurtBox.healthComponent.gameObject;
                if (hurtBodyObject == null)
                    continue;

                CharacterMotor motor = hurtBox.healthComponent.body.characterMotor;
                if (!motor || (motor && !motor.isGrounded))
                {
                    networkCombat.ApplyBasedAuthority(hurtBox.healthComponent.body.gameObject, gameObject, 2);
                }
            }
        }

        //public override void OnExit()
        //{
        //    base.OnExit();
        //    if (!target.healthComponent.body)
        //        return;
        //    if (target.healthComponent.GetComponent<SpikingComponent>())
        //        return;

        //    if ((target.healthComponent.GetComponent<CharacterMotor>() && !target.healthComponent.body.characterMotor.isGrounded))
        //    {
        //        GetComponent<WyattNetworkCombat>().ApplyBasedAuthority(target.healthComponent.gameObject, gameObject, 1);
        //    }

        //    else if (target.healthComponent.GetComponent<RigidbodyMotor>())
        //    {
        //        GetComponent<WyattNetworkCombat>().ApplyBasedAuthority(target.healthComponent.gameObject, gameObject, 1.5f);
        //    }
        //}
    }
}
