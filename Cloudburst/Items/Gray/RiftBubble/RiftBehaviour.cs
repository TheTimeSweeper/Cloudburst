using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Gray.RiftBubble
{
    internal class RiftBehaviour : CharacterBody.ItemBehavior
    {
        public GameObject indicatorInstance;
        public BuffWard ward;

        public void Awake()
        {
            //AddItemBehavior only sets body after AddComponent, so wait for it to enable us
            base.enabled = false;
        }

        public void OnEnable()
        {
            //every client spawns its own bubble for the visuals, BuffWard only hands out the debuff on the server
            indicatorInstance = Instantiate(RiftBubble.riftBubbleIndicator, transform);
            indicatorInstance.GetComponent<TeamFilter>().teamIndex = body.teamComponent.teamIndex;
            ward = indicatorInstance.GetComponent<BuffWard>();
            UpdateRadius();
        }

        public void FixedUpdate()
        {
            UpdateRadius();
        }

        private void UpdateRadius()
        {
            if (ward)
            {
                ward.radius = RiftBubble.BaseRadius + RiftBubble.StackingRadius * (stack - 1) + body.radius;
            }
        }

        public void OnDisable()
        {
            if (indicatorInstance)
            {
                Destroy(indicatorInstance);
            }
        }
    }
}
