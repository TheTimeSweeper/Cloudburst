using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Green.RiftBubble
{
    internal class RiftBehaviour : CharacterBody.ItemBehavior
    {
        private float stopwatch;

        public void FixedUpdate()
        {
            if (!body)
            {
                return;
            }

            stopwatch += Time.fixedDeltaTime;
            if (stopwatch < RiftBubble.GetInterval(stack))
            {
                return;
            }
            stopwatch = 0;

            //shortcut: every client spawns its own bubble off its own timer for the visuals, BuffWard only hands out the debuff on the server. network the bubble if client visuals drift noticeably
            GameObject bubble = Instantiate(RiftBubble.riftBubbleIndicator, body.corePosition, Quaternion.identity);
            bubble.GetComponent<TeamFilter>().teamIndex = body.teamComponent.teamIndex;
            bubble.GetComponent<BuffWard>().radius = RiftBubble.Radius + body.radius;
            Destroy(bubble, RiftBubble.GetDuration(stack));
        }
    }
}
