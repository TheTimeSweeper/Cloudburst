using EntityStates;
//using Cloudburst.Cores;
using UnityEngine;
using RoR2.Projectile;
using Cloudburst.Wyatt.Components;
using RoR2;
using System.Collections;
using Cloudburst.Characters.Wyatt;

namespace Cloudburst.CEntityStates.Wyatt
{
    public class DeployMaid : BaseSkillState
    {
        //it's a timer. love u nigma
        private float theDevilHasSomeHardToReadFinePrint = 0;

        private bool unstable = false;

        private WyattMAIDManager blaseballManager = null;
     
        private bool solarEclipse = false;

        public override void OnEnter()
        {
            // base.activatorSkillSlot.skillDef.baseRechargeInterval = 5;

            base.OnEnter();
            blaseballManager = GetComponent<WyattMAIDManager>();
            blaseballManager.OnRetrival += BlaseballManager_OnRetrival;
            // blaseball.sunset += Blaseball_sunset;
            if (base.isAuthority)
            {
                FireProjectile();
                //outer.SetNextStateToMain();
            }
        }

        private void Blaseball_sunset()
        {
            solarEclipse = true;
        }

        IEnumerator Wait()
        {
            base.characterMotor.useGravity = false;
            base.characterMotor.velocity = new Vector3(0, 18, 0);
            bool stopped = false;

            base.StartAimMode();
            //todo remove comments and move wyattrocket bullshit into here
            //and get rid of coroutine lol
            //if (base.IsKeyDownAuthority() && stopped == false)
            //{
            //    base.characterMotor.useGravity = true;
            //    if (base.gameObject.GetComponent<WyattRocket>() == null)
            //    {
            //        WyattRocket based = base.gameObject.AddComponent<WyattRocket>();
            //        based.interval = 1f;
            //        //LogCore.LogI(dis);
            //    }
            //    base.PlayAnimation("Fullbody, Override", "kick");
            //    stopped = true;

            //    //yield return true
            //}

            //no more wait. instant explode
            yield return new WaitForSeconds(0);// 0.5f);

            //base.characterMotor.velocity = new Vector3(0, 0, 0);

            if (stopped != true)
            {
                base.characterMotor.velocity = new Vector3(0, 18, 0);


                if (base.gameObject.GetComponent<WyattRocket>() == null)
                {
                    WyattRocket based = base.gameObject.AddComponent<WyattRocket>();
                    based.interval = 1f;
                    //LogCore.LogI(dis);
                }
                base.PlayAnimation("Fullbody, Override", "kick");

                base.characterMotor.useGravity = true;
            }
        }

        private void BlaseballManager_OnRetrival(bool nat, GenericSkill arg2, Vector3 dis)
        {
            solarEclipse = true;
            if (!nat)
            {
                base.activatorSkillSlot.rechargeStopwatch = (0.5f * base.activatorSkillSlot.finalRechargeInterval);
            }
            else
            {
                //base.activatorSkillSlot.finalRechargeInterval = 10;
                //who cares about underlying issues in my code
                //no one's gonna read it anyways :^]]

                outer.StartCoroutine(Wait());
            }

            //else
            {//
             //   base.activatorSkillSlot.skillDef.baseRechargeInterval = 5;
            }
            Log.Warning("nip");
            base.activatorSkillSlot.DeductStock(1);
            blaseballManager.OnRetrival -= BlaseballManager_OnRetrival;
        }

        public override void OnExit()
        {
            base.OnExit();
            blaseballManager.OnRetrival -= BlaseballManager_OnRetrival;
        }
        public void FireProjectile()
        {
            var aimRay = base.GetAimRay();
            FireProjectileInfo info = new FireProjectileInfo()
            {
                crit = RollCrit(),
                //damage = (5f + (characterBody.GetBuffCount(Custodian.instance.wyattCombatDef) * .25f)) * damageStat,
                damage = damageStat * WyattConfig.M4MaidProjectileDamage.Value,
                damageColorIndex = RoR2.DamageColorIndex.Default,
                damageTypeOverride = DamageType.Generic,
                force = 0,
                owner = gameObject,
                position = aimRay.origin,
                procChainMask = default,
                projectilePrefab = Characters.Wyatt.WyattAssets.wyattMaidBoomerang,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                target = null,
                useFuseOverride = false,
                useSpeedOverride = false,
            };
            ProjectileManager.instance.FireProjectile(info);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            theDevilHasSomeHardToReadFinePrint += Time.fixedDeltaTime;
            if (base.isAuthority && base.IsKeyDownAuthority() && solarEclipse == false && unstable == false && theDevilHasSomeHardToReadFinePrint > 0.3f)
            {
                characterBody.isSprinting = true;
                blaseballManager.RetrieveMAIDAuthority();
                unstable = true;
            }

            if (solarEclipse && base.isAuthority)
            {
                outer.SetNextStateToMain();
                //dW5jb21tZW50IHRoaXMgbGluZSBmb3IgZnVubnkK
                /*outer.SetNextState(new DeployMaid() {
                    solarEclipse = true
                });*/
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}