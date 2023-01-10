using EntityStates;
//using Cloudburst.Cores;
using Cloudburst.Wyatt.Components;

namespace Cloudburst.CEntityStates.Wyatt
{
    class RetrieveMaid : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                gameObject.GetComponent<MAIDManager>().RetrieveMAIDAuthority();
                //skillLocator.special.SetPropertyValue("cooldownRemaining", (Single)3);
                //skillLocator.special.cooldownRemaining
                outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}