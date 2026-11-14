using EntityStates;
using RoR2;
using UnityEngine;

namespace Cloudburst.Enemies.Guardsman.EntityStates
{
    //overhead halberd chop, split into windup/swing/end so each part can be tuned on its own
    public class OverheadSwing : BaseSkillState
    {
        public static float damageCoefficient = 4.5f;
        public static float procCoefficient = 1f;
        public static float pushAwayForce = 2000f;
        public static float baseWindupDuration = 1.2f;
        public static float baseSwingDuration = 0.4f;
        public static float baseEndDuration = 1.3f;

        private enum Phase { Windup, Swing, End }

        private Phase phase;
        private float phaseDuration;
        private float phaseStartAge;
        private OverlapAttack attack;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();

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
                //no playback rate parameters on these states either
                animator.speed = attackSpeedStat;
            }
            Util.PlaySound(global::EntityStates.ImpMonster.DoubleSlash.enterSoundString, gameObject);
            EnterPhase(Phase.Windup, baseWindupDuration, "SwingStarterEndPart1_windup");
        }

        private void EnterPhase(Phase newPhase, float baseDuration, string animationState)
        {
            phase = newPhase;
            phaseDuration = baseDuration / attackSpeedStat;
            phaseStartAge = fixedAge;
            PlayCrossfade("FullBody, Override", animationState, 0.05f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StopMoving();

            if (phase == Phase.Swing && isAuthority)
            {
                attack.Fire();
            }

            if (fixedAge - phaseStartAge < phaseDuration)
                return;

            switch (phase)
            {
                case Phase.Windup:
                    Util.PlaySound(global::EntityStates.ImpMonster.DoubleSlash.slashSoundString, gameObject);
                    EnterPhase(Phase.Swing, baseSwingDuration, "SwingStarterEndPart1_swing");
                    break;
                case Phase.Swing:
                    EnterPhase(Phase.End, baseEndDuration, "SwingStarterEndPart1_end");
                    break;
                case Phase.End:
                    if (isAuthority)
                    {
                        outer.SetNextStateToMain();
                    }
                    break;
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
