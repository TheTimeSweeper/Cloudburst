using System;
using Cloudburst.Characters;
using Cloudburst.Characters.Wyatt;
using Cloudburst.Wyatt.Components;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.CEntityStates.Wyatt
{
    class WyattBaseMeleeAttack : BasicMeleeAttack, SteppedSkillDef.IStepSetter
    {

        public int step = 0;
        public static float recoilAmplitude = 0.7f;
        public static float percentDurationBeforeInterruptable = 0.6f;
        public float bloom = 1f;
        //public static float comboFinisherBaseDuration = 0.5f;
        //public static float comboFinisherhitPauseDuration = 0.15f;
        //public static float comboFinisherBloom = 0.5f;
        //public static float comboFinisherBaseDurationBeforeInterruptable = 0.5f;
        //private string animationStateName;
        private float durationBeforeInterruptable;

        private bool isComboFinisher
        {
            get
            {
                return this.step == 2;
            }
        }
        //scrapping swing down (what's the opposite of uppercut? lowercut?) and just making it always uppercut
        private bool isUppercut
        {
            get => true;
            //get => base.isGrounded;
        }
        private string stepAnimationStateName
        {
            get
            {
                switch (this.step)
                {
                    default:
                    case 0:
                        return "Swing1";
                    case 1:
                        return "Swing2";
                    case 2:
                        return "Swing3";
                }
            }
        }
        private string stepMecanimActiveParameter
        {
            get
            {
                switch (step)
                {
                    default:
                    case 0:
                        return "BroomSwing1.active";
                    case 1:
                        return "BroomSwing2.active";
                    case 2:
                        return "BroomSwing3.active";
                }
            }
        }


        private string stepSwingMuzzle
        {
            get
            {
                switch (step)
                {
                    default:
                    case 0:
                        return "MuzzleSwing1";
                    case 1:
                        return "MuzzleSwing2";
                    case 2:
                        if (isUppercut)
                        {
                            return "MuzzleSwingUppercut";
                        } else
                        {
                            return "MuzzleSwingSpike";
                        }
                }
            }
        }

        public override void OnEnter()
        {
            this.hitBoxGroupName = "HitboxSwing";
            if (isComboFinisher) this.hitBoxGroupName = "HitboxSwingLarge";
            this.mecanimHitboxActiveParameter = stepMecanimActiveParameter;
            
            this.baseDuration = WyattConfig.M1AttackDuration.Value;// 0.5f;
            if (isComboFinisher) baseDuration = WyattConfig.M1AttackDurationFinisher.Value;// 0.8f;
            //this.duration = this.baseDuration / base.attackSpeedStat;

            this.hitPauseDuration = 0.02f;
            if (isComboFinisher) hitPauseDuration = 0.1f;

            this.damageCoefficient = WyattConfig.M1Damage.Value; //1;
            if (isComboFinisher) damageCoefficient = WyattConfig.M1DamageFinisher.Value;// 2f;

            this.procCoefficient = 1f;
            this.shorthopVelocityFromHit = 4;
            if (isComboFinisher) shorthopVelocityFromHit = 10f;

            swingEffectPrefab = WyattEffects.notMercSlashEffect;// BandaidConvert.Resources.Load<GameObject>("prefabs/effects/GrandparentGroundSwipeTrailEffect");
            if (isComboFinisher) swingEffectPrefab = WyattEffects.notMercSlashEffectThicc;
            hitEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxmedium");

            beginSwingSoundString = "Play_Wyatt_Whoosh";
            impactSound = WyattAssets.hitSound;


            /*EffectManager.SpawnEffect(Effects.shaderEffect, new EffectData()
            {
                origin = base.transform.position,
            }, false);*/

            base.OnEnter();

            R2API.DamageAPI.AddModdedDamageType(overlapAttack, WyattDamageTypes.applyGroove);
        }

        public override float CalcDuration()
        {
            float duration = base.CalcDuration();

            this.durationBeforeInterruptable = percentDurationBeforeInterruptable * duration;
            return duration;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode();
            animator.SetFloat("BroomFinisherSpike", isUppercut ? 0 : 1, 0.06f, Time.fixedDeltaTime);
        }
        
        public override void BeginMeleeAttackEffect()
        {

            swingEffectMuzzleString = stepSwingMuzzle;
            base.BeginMeleeAttackEffect();
        }

        public override void OnExit()
        {
            base.OnExit();

        }

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
            //despite what the animation is playing, decided I want to decide when it lands what the hit does
            if (this.isComboFinisher && isUppercut)
            {
                //overlapAttack.damageType = DamageTypeCore.antiGrav | DamageType.Generic;
                R2API.DamageAPI.AddModdedDamageType(overlapAttack, WyattDamageTypes.antiGravDamage); 
            } else
            {
                R2API.DamageAPI.RemoveModdedDamageType(overlapAttack, WyattDamageTypes.antiGravDamage);
            }
        }        

        public override void PlayAnimation()
        {
            /*EffectManager.SpawnEffect(Effects.blackHoleIncisionEffect, new EffectData()
            {
                origin = base.transform.position,
                scale = 10,
                rotation = Quaternion.identity, 
            }, false);*/
            //bool moving = this.animator.GetBool("isMoving");
            //bool grounded = this.animator.GetBool("isGrounded");

            //if (!moving && grounded)
            //{
            //    base.PlayCrossfade("FullBody, Override", this.animationStateName, "BroomSwing.playbackRate", this.duration, 0.05f);
            //}

            base.PlayCrossfade("Gesture, Override", this.stepAnimationStateName, "BroomSwing.playbackRate", this.duration, 0.05f);
        }        

        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();
            base.characterBody.AddSpreadBloom(this.bloom);
            if (isComboFinisher)
            {
                WyattNetworkCombat networkCombat = GetComponent<WyattNetworkCombat>();
                for (int i = 0; i < hitResults.Count; i++)
                {
                    HurtBox hurtBox = hitResults[i];
                    GameObject hurtBodyObject = hurtBox.healthComponent.gameObject;
                    if (hurtBodyObject == null)
                        continue;
                    
                    if (isUppercut)
                    {
                        networkCombat.ApplyKnockupAuthority(hurtBodyObject, WyattConfig.M1UpwardsLiftForce.Value);
                        networkCombat.ApplyKnockbackAuthority(hurtBodyObject, GetAimRay().direction, WyattConfig.M1KnockbackForce.Value);
                    }
                    else
                    {
                        CharacterMotor motor = hurtBox.healthComponent.body.characterMotor;
                        if (!motor || (motor && !motor.isGrounded))
                        {
                            networkCombat.ApplyBasedAuthority(hurtBox.healthComponent.body.gameObject, gameObject, 2);
                        }
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (base.fixedAge >= this.durationBeforeInterruptable)
            {
                return InterruptPriority.Any;
            }
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)this.step);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.step = (int)reader.ReadByte();
        }

        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            this.step = i;
        }
    }
}

