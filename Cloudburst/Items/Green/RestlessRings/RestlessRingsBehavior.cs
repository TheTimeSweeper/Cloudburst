using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Items.Green.RestlessRings
{
    //server only, added through AddItemBehavior
    public class RestlessRingsBehavior : CharacterBody.ItemBehavior
    {
        private float timer;
        private BuffDef currentBuff;

        private void Start()
        {
            CycleBuff();
        }

        private void FixedUpdate()
        {
            if (!body)
                return;

            timer += Time.fixedDeltaTime;
            if (timer >= RestlessRings.BuffInterval)
            {
                timer = 0;
                CycleBuff();
            }
        }

        private void CycleBuff()
        {
            if (!NetworkServer.active || !body)
                return;

            BuffDef[] buffs = RestlessRings.ringBuffs;

            //pick a different buff from the current one
            int index = Random.Range(0, currentBuff ? buffs.Length - 1 : buffs.Length);
            if (currentBuff && buffs[index] == currentBuff)
            {
                index = buffs.Length - 1;
            }

            RemoveRingBuffs();

            currentBuff = buffs[index];
            body.AddBuff(currentBuff);

            if (currentBuff == RestlessRings.attackSpeedBuff)
            {
                Util.PlaySound("Play_item_proc_crit_attack_speed1", gameObject);
            }
            else if (currentBuff == RestlessRings.regenBuff)
            {
                Util.PlaySound("char_healing_drone_heal_02", gameObject);
            }
        }

        private void RemoveRingBuffs()
        {
            for (int i = 0; i < RestlessRings.ringBuffs.Length; i++)
            {
                if (body.HasBuff(RestlessRings.ringBuffs[i]))
                {
                    body.RemoveBuff(RestlessRings.ringBuffs[i]);
                }
            }
        }

        private void OnDisable()
        {
            if (NetworkServer.active && body)
            {
                RemoveRingBuffs();
            }
            currentBuff = null;
        }
    }
}
