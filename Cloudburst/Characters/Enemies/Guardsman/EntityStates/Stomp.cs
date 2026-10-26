using EntityStates;
using RoR2;
using UnityEngine;

namespace Cloudburst.Enemies.Guardsman.EntityStates
{
    //close range shockwave that knocks everything nearby into the air
    public class Stomp : BaseSkillState
    {
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.55f;
        public static float fireFraction = 0.5f;
        public static float baseForce = 1500f;
        public static float upwardForce = 3000f;
        public static float blastRadius = 10f;

        private float duration;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            animator = GetModelAnimator();
            if (animator)
            {
                //the Stomp state has no playback rate parameter either
                animator.speed = attackSpeedStat;
            }
            StopMoving();
            if (characterDirection)
            {
                characterDirection.forward = GetAimRay().direction;
            }
            PlayCrossfade("FullBody, Override", "Stomp", 0.1f);
            Util.PlaySound(global::EntityStates.BeetleGuardMonster.GroundSlam.initialAttackSoundString, gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StopMoving();

            if (!hasFired && fixedAge >= duration * fireFraction)
            {
                Fire();
            }

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        private void Fire()
        {
            hasFired = true;
            if (!isAuthority)
                return;

            Vector3 position = characterBody.footPosition;
            EffectManager.SpawnEffect(Guardsman.stompEffect, new EffectData
            {
                origin = position,
                scale = blastRadius
            }, true);

            new BlastAttack
            {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = GetTeam(),
                position = position,
                radius = blastRadius,
                baseDamage = damageCoefficient * damageStat,
                procCoefficient = procCoefficient,
                baseForce = baseForce,
                bonusForce = Vector3.up * upwardForce,
                crit = RollCrit(),
                damageType = DamageType.Generic,
                damageColorIndex = DamageColorIndex.Default,
                falloffModel = BlastAttack.FalloffModel.Linear,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
                losType = BlastAttack.LoSType.NearestHit,
                procChainMask = default(ProcChainMask),
            }.Fire();
        }

        private void StopMoving()
        {
            if (characterMotor)
            {
                characterMotor.moveDirection = Vector3.zero;
            }
        }

        public override void OnExit()
        {
            if (animator)
            {
                animator.speed = 1f;
            }
            PlayCrossfade("FullBody, Override", "BufferEmpty", 0.1f);
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
