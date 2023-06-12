using Cloudburst.Characters;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace Cloudburst.Wyatt.Components
{
    public class WyattMAIDSkillDef : SkillDef
    {
        private enum MAIDState
        {
            None = -1,
            Deployed,
            Reeling,
            Idle,
            Count

        }

        private MAIDState MAID = MAIDState.None;

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot)
        {
            base.OnFixedUpdate(skillSlot);
            //FUCK HOPOO
            //FUCK YOUR WACK ASS SKILL SYSTEM
            var data = (InstanceData)skillSlot.skillInstanceData;
            MAIDManager mm = data.manager;
            if (mm)
            {
                if (mm.maid)
                {
                    MAID = MAIDState.Deployed;
                }
                if (!mm.maid)
                {
                    MAID = MAIDState.Idle;
                }
                if (mm.startReel)
                {
                    MAID = MAIDState.Reeling;
                }
            }
        }

        public override Sprite GetCurrentIcon(GenericSkill skillSlot)
        {
            switch (MAID)
            {
                case MAIDState.Deployed:
                    return WyattSurvivor.instance.MaidSprite2;
                //    return AssetsCore.wyattSpecial2;
                case MAIDState.Reeling:
                    return WyattSurvivor.instance.MaidSpriteTempWhatIsThis;

                case MAIDState.Idle:
                    return WyattSurvivor.instance.MaidSprite1;
                    //    return AssetsCore.wyattSpecial;
            }
            return base.GetCurrentIcon(skillSlot);
        }

        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new WyattMAIDSkillDef.InstanceData
            {
                manager = skillSlot.GetComponent<MAIDManager>(),
                subbed = false
            };
        }

        public bool DetermineExecution([NotNull] GenericSkill skillSlot)
        {
            switch (MAID)
            {
                case MAIDState.Idle:
                    return true;
                case MAIDState.Deployed:
                    return true;
                case MAIDState.Reeling:
                    return false;
                default:
                    return false;
            }
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.HasRequiredStockAndDelay(skillSlot) && this.DetermineExecution(skillSlot);
        }


        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public InstanceData()
            {
            }
            public MAIDManager manager;
            /// <summary>
            /// ay yo what's up #logang today we're going to be heading in the #japanesesuicideforest here in japan, 
            /// but first of all make sure to smash that like button, share this video, and subscribe for more vlogs like this one right here, 
            /// also make sure to follow me on twitter, instagram google +, and like my page on facebook and pintrest. 
            /// before heading in here i just wanna say suicide is not a joke but is that a dead body i'm not fucking with ya'll let's get the camera in there. 
            /// asyou can see this person is dead if you don't want to end up like him make sure you stay subscribed 
            /// you know he probably ain't have no friends but if your in the #logang you know logan is your best friend haha chilling 
            /// anyways thanks for watching guys 
            /// and i won't be monetizing this video but make sure to check out my merch in the description, 
            /// and dont forget to like share and subscribe and peace out #logang haha
            /// </summary>
            public bool subbed;
        }
    }
}