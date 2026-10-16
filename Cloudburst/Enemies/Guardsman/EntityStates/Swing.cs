using EntityStates;
using RoR2;
using UnityEngine;

namespace Cloudburst.Enemies.Guardsman.EntityStates
{
    //big overhead halberd sweep in front of him
    public class Swing : BaseSkillState
    {
        public static float damageCoefficient = 3f;
        public static float procCoefficient = 1f;
        public static float pushAwayForce = 3000f;
        //length of the Swing clip
        public static float baseDuration = 4.17f;
        public static float attackStartFraction = 0.573f;
        public static float attackEndFraction = 0.7067f;

        private float duration;
        private OverlapAttack attack;
        private bool hasSwung;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;

            StopMoving();
            if (characterDirection)
            {
                characterDirection.forward = GetAimRay().direction;
            }

            attack = new OverlapAttack
            {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = GetTeam(),
                damage = damageCoefficient * damageStat,
                procCoefficient = procCoefficient,
                pushAwayForce = pushAwayForce,
                isCrit = RollCrit(),
                hitBoxGroup = FindHitBoxGroup("HitboxSwing"),
                hitEffectPrefab = global::EntityStates.ImpMonster.DoubleSlash.hitEffectPrefab,
            };

            animator = GetModelAnimator();
            if (animator)
            {
                //the swing blend tree angles the halberd towards where he's looking
                animator.SetFloat("swingPitchCycle", animator.GetFloat("aimPitchCycle"));
                //the Swing state has no playback rate parameter, so speed up the whole animator while he's rooted in place
                animator.speed = attackSpeedStat;
            }
            PlayCrossfade("FullBody, Override", "Swing", 0.1f);
            Util.PlaySound(global::EntityStates.ImpMonster.DoubleSlash.enterSoundString, gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StopMoving();

            if (fixedAge >= duration * attackStartFraction && fixedAge <= duration * attackEndFraction)
            {
                if (!hasSwung)
                {
                    hasSwung = true;
                    Util.PlaySound(global::EntityStates.ImpMonster.DoubleSlash.slashSoundString, gameObject);
                }
                //keep firing through the window so the whole sweep can hit, OverlapAttack never hits the same target twice
                if (isAuthority)
                {
                    attack.Fire();
                }
            }

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
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
