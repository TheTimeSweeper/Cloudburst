using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items
{
    internal class RiftBehaviour : CharacterBody.ItemBehavior
    {
        public GameObject indicatorInstance;
        public BuffWard ward;
        public void OnEnable()
        {
            indicatorInstance = GameObject.Instantiate<GameObject>(RiftBubble.riftBubbleIndicator, transform);
            if(indicatorInstance != null)
            {
                ward = indicatorInstance.GetComponent<BuffWard>();
                ward.teamFilter.teamIndex = TeamComponent.GetObjectTeam(gameObject);
            }
        }

        public void FixedUpdate()
        {
            if(ward != null)
            {
                ward.radius = stack * 5;
            }
        }

        public void OnDisable()
        {
            if(indicatorInstance != null)
            {
                GameObject.Destroy(indicatorInstance);
            }
        }
    }
}
