using Cloudburst.Wyatt.Components;
using EntityStates;
using RoR2;
using System.Collections.Generic;

namespace Cloudburst.CEntityStates.Wyatt
{
    class ActivateFlow : BaseSkillState
    {
        public static float baseDuration = 0.1f;
        private WyattWalkmanBehavior walkman;
        public override void OnEnter()
        {
            base.OnEnter();
            
            walkman = GetComponent<WyattWalkmanBehavior>();
            if (walkman.flowing == false)
            {
                walkman.ActivateFlowAuthority();
            }
            PlayAnimation("LeftArm, Override", "Groovy");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= baseDuration)
            { 
                if (isAuthority)
                {
                    outer.SetNextStateToMain();
                };
            }
        }
    }
}