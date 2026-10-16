using EntityStates;
using RoR2;

namespace Cloudburst.Enemies.Guardsman.EntityStates
{
    public class GuardsmanSpawnState : BaseState
    {
        //length of the Spawn clip
        public static float duration = 1.46f;

        public override void OnEnter()
        {
            base.OnEnter();
            PlayAnimation("Body", "Spawn", "Spawn.playbackRate", duration);
            //no soundbank of our own, borrow the beetle guard's
            Util.PlaySound(global::EntityStates.BeetleGuardMonster.SpawnState.spawnSoundString, gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
