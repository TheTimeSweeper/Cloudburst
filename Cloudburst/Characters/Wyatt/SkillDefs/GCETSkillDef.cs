using System;
using Cloudburst.GlobalComponents;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;

namespace Cloudburst.Wyatt.Components
{
    public class GCETSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new GCETSkillDef.InstanceData
            {
                droneTracker = skillSlot.GetComponent<GenericCursorEnemyTracker>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            GenericCursorEnemyTracker HANDDroneTracker = ((GCETSkillDef.InstanceData)skillSlot.skillInstanceData).droneTracker;
            return (HANDDroneTracker != null) ? HANDDroneTracker.GetTrackingTarget() : null;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return GCETSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HasTarget(skillSlot);
        }


        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public GenericCursorEnemyTracker droneTracker;
        }
    }
}
