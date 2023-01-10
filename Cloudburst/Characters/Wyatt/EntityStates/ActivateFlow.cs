using Cloudburst.Wyatt.Components;
using EntityStates;
using RoR2;
using System.Collections.Generic;

namespace Cloudburst.CEntityStates.Wyatt
{
    class ActivateFlow : BaseSkillState
    {
        public static float baseDuration = 0.1f;
        private CustodianWalkmanBehavior walkman;
        public override void OnEnter()
        {
            base.OnEnter();

            walkman = GetComponent<CustodianWalkmanBehavior>();
            if (walkman.flowing == false)
            {
                walkman.ActivateFlowAuthority();
            }
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