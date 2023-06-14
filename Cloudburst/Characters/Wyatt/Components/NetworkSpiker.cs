using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Wyatt.Components
{
    public class NetworkSpiker : NetworkBehaviour
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
    }
}