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
    //TODO:
    //Fix the combo finisher being weird.

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
        
        private bool spawnEffect = false;
        private string animationStateName;

        public override bool allowExitFire
        {
            get
            {
                return base.characterBody && !base.characterBody.isSprinting;
            }
        }

 

        public override void OnEnter()
        {
            //this.hitBoxGroupName = "TempHitbox";
            this.hitBoxGroupName = "TempHitboxLarge";
            this.mecanimHitboxActiveParameter = GetMecanimActiveParameter();
            this.baseDuration = 0.8f;
            this.duration = this.baseDuration / base.attackSpeedStat;
            this.hitPauseDuration = 0.05f;
            this.damageCoefficient = 2f;
            //this.damageCoefficient = (2f + (characterBody.GetBuffCount(Custodian.instance.wyattCombatDef) * 0.1f));
            this.procCoefficient = 1f;
            this.durationBeforeInterruptable = percentDurationBeforeInterruptable * duration;
            this.shorthopVelocityFromHit = 5;

            spawnEffect = false;
            //swingEffectPrefab = BandaidConvert.Resources.Load<GameObject>("prefabs/effects/GrandparentGroundSwipeTrailEffect");
            hitEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxmedium");
            //swingEffectMuzzleString = "WinchHole";//"//SwingTrail";

            /*var obj = CloudburstPlugin.Instantiate<GameObject>(AssetsCore.mainAssetBundle.LoadAsset<GameObject>("mdlSpitter"), new Vector3(201f, -128.8f, 143f), Quaternion.Euler(new Vector3(0, -43.019f, 0)));

            obj.layer = LayerIndex.world.intVal;
            obj.transform.position = base.transform.position;
            obj.transform.localScale = new Vector3(10, 10, 10);
            NetworkServer.Spawn(obj);*/

            /*EffectManager.SpawnEffect(Effects.shaderEffect, new EffectData()
            {
                origin = base.transform.position,
            }, false);*/

            //LogCore.LogW(step);

            if (isComboFinisher)
            {
                // LogCore.LogW("finisher");
                this.hitBoxGroupName = "TempHitbox";
                if (isGrounded)
                {
                    forceVector = new Vector3(0, 1000, 0);
                }
                //this.baseDuration = 1f;
                //this.duration = this.baseDuration / base.attackSpeedStat;
                this.hitPauseDuration = 0.2f;
                this.damageCoefficient = 4f;
            }
            //else { LogCore.LogW("not finisher"); }

            base.OnEnter();
            base.characterDirection.forward = base.GetAimRay().direction;
            base.characterMotor.ApplyForce(GetAimRay().direction * 100, true, false);


        }

        private string GetMecanimActiveParameter()
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

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode();
        }


        public override void BeginMeleeAttackEffect()
        {
            if (!spawnEffect)
            {
                spawnEffect = true;
                if (base.isAuthority)
                {

                   // EffectManager.SimpleMuzzleFlash(obj, base.gameObject, "SwingTrail", true);
                }
            }
        }


        public override void OnExit()
        {
            base.OnExit();

        }

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
            if (this.isComboFinisher && isGrounded)
            {
                //overlapAttack.damageType = DamageTypeCore.antiGrav | DamageType.Generic;
                R2API.DamageAPI.AddModdedDamageType(overlapAttack, WyattDamageTypes.antiGravDamage); 
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
            this.animationStateName = "";
            switch (this.step)
            {
                case 0:
                    this.animationStateName = "Swing1";
                    break;
                case 1:
                    this.animationStateName = "Swing2";
                    break;
                case 2:
                    if (base.isGrounded)
                    {
                        this.animationStateName = "Swing3";
                    } else
                    {
                        animationStateName = "Swing3-2";
                    }
                    break;
            }
            //bool moving = this.animator.GetBool("isMoving");
            //bool grounded = this.animator.GetBool("isGrounded");

            //if (!moving && grounded)
            //{
            //    base.PlayCrossfade("FullBody, Override", this.animationStateName, "BroomSwing.playbackRate", this.duration, 0.05f);
            //}

            base.PlayCrossfade("Gesture, Override", this.animationStateName, "BroomSwing.playbackRate", this.duration, 0.05f);
        }

        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();
            base.characterBody.AddSpreadBloom(this.bloom);
            if (isComboFinisher)
            {
                if (!base.isGrounded)
                {
                    for (int i = 0; i < hitResults.Count; i++)
                    {
                        HurtBox hurtBox = hitResults[i];

                        SpikingComponent cringe = hurtBox.healthComponent.body.gameObject.AddComponent<SpikingComponent>();
                        cringe.interval = 1f;
                        cringe.originalSpiker = this.gameObject;
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

