using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Wyatt.Components
{
    public class WyattNetworkCombat : NetworkBehaviour
    {
        public void ApplyBasedAuthority(GameObject spikee, GameObject spiker, float interval)
        {
            if (NetworkServer.active)
            {
                RpcApplySpike(spikee, spiker, interval);
            } 
            else
            {
                CmdApplySpike(spikee, spiker, interval);
            }
        }
        [Command]
        private void CmdApplySpike(GameObject spikee, GameObject spiker, float interval)
        {
            RpcApplySpike(spikee, spiker, interval);
        }
        [ClientRpc]
        private void RpcApplySpike(GameObject spikee, GameObject spiker, float interval)
        {
            ApplySpikeInternal(spikee, spiker, interval);
        }
        private void ApplySpikeInternal(GameObject spikee, GameObject spiker, float interval)
        {
            if (spikee != null)
            {
                SpikingComponent spike = spikee.AddComponent<SpikingComponent>();
                spike.originalSpiker = spiker;
                spike.interval = interval;
            }
        }

        public void ApplyKnockupAuthority(GameObject victimBody, float acceleration)
        {
            if (NetworkServer.active)
            {
                ApplyKnockupInternal(victimBody, acceleration);
            }
            else
            {
                CmdApplyKnockup(victimBody, acceleration);
            }
        }
        [Command]
        private void CmdApplyKnockup(GameObject victimBody, float acceleration)
        {
            ApplyKnockupInternal(victimBody, acceleration);
        }

        private static void ApplyKnockupInternal(GameObject victimBody, float acceleration)
        {
            CCUtilities.AddUpwardImpulseToBody(victimBody, acceleration);
        }


        public void ApplyKnockbackAuthority(GameObject victimBody, Vector3 directionAuthority, float acceleration)
        {
            if (NetworkServer.active)
            {
                ApplyKnockbackInternal(victimBody, directionAuthority, acceleration);
            }
            else
            {
                CmdApplyKnockback(victimBody, directionAuthority, acceleration);
            }
        }
        [Command]
        private void CmdApplyKnockback(GameObject victimBody, Vector3 directionAuthority, float acceleration)
        {
            ApplyKnockbackInternal(victimBody, directionAuthority, acceleration);
        }

        private static void ApplyKnockbackInternal(GameObject victimBody, Vector3 directionAuthority, float acceleration)
        {
            CCUtilities.AddForwardImpulseToBody(victimBody, directionAuthority, acceleration);
        }
    }
}