using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Wyatt.Components
{
    public class WyattMAIDManager : NetworkBehaviour
    {
        [SyncVar]
        public GameObject maid;
        public GameObject winch;
        [SyncVar]
        public bool startReel;
        private float _stopwatch = 0;

        public event Action<bool, GenericSkill, Vector3> OnRetrival;
        //public event Action<GenericSkill> OnDeploy;
        public event Action sunset;

        private SkillLocator skillLocator;
        private CharacterBody body;
        private CharacterMotor characterMotor;
        private CharacterDirection characterDirection;

        public void Awake()
        {
            skillLocator = GetComponent<SkillLocator>();
            characterMotor = GetComponent<CharacterMotor>();
            body = GetComponent<CharacterBody>();
            characterDirection = GetComponent<CharacterDirection>();
            //click and she's gone!
        }

        public void Invoke(bool bo, Vector3 dis)
        {
            if (NetworkServer.active)
            {
                RpcSetDeploy(bo, dis);
            }
        }

        private void FixedUpdate()
        {
            if (startReel == true && maid && Util.HasEffectiveAuthority(gameObject))
            {
                Vector3 lossyScale = maid.transform.lossyScale;
                var volume = lossyScale.x * 2f * (lossyScale.y * 2f) * (lossyScale.z * 2f);

                _stopwatch += Time.fixedDeltaTime;

                Vector3 velocity = (maid.transform.position - base.transform.position).normalized * 120f;

                characterMotor.velocity = velocity;
                //Log.Warning("set velocity " + velocity.magnitude);
                characterDirection.forward = characterMotor.velocity.normalized;
                //float distance = volume;

                if (_stopwatch > 3)
                {
                    DestroymaidAuthority();
                    Destroy(winch);
                    body.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
                    characterMotor.Motor.ForceUnground();
                    characterMotor.velocity = Vector3.zero;
                    sunset.Invoke();
                    //RpcSetDeploy(true);

                }
                float distance = Vector3.Distance(base.transform.position, maid.transform.position);
                if (distance <= 1.185805)
                {
                    DestroymaidAuthority();
                    Destroy(winch);
                    body.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
                    characterMotor.Motor.ForceUnground();
                    characterMotor.velocity = Vector3.up * 30f;
                    //Log.Warning("set velocity up catrch");
                    // RpcSetDeploy(false);
                }
            }
        }

        private void DestroymaidAuthority()
        {
            if (NetworkServer.active)
            {
                NetworkServer.Destroy(maid);
                startReel = false;
            }
            else
            {
                CmdDestroymaid();
            }
        }

        [Command]
        private void CmdDestroymaid()
        {
            NetworkServer.Destroy(maid);
            startReel = false;
        }

        #region Deployment
        public void DeployMAIDAuthority(GameObject maid)
        {
            if (NetworkServer.active)
            {
                DeployMAIDInternal(maid);
                //RpcDeployMAID(maid);
                return;
            }
            //CmdDeployMAID(maid);
        }

        [ClientRpc]
        private void RpcDeployMAID(GameObject maid)
        {
            DeployMAIDInternal(maid);
        }

        [Command]
        private void CmdDeployMAID(GameObject maid)
        {
            DeployMAIDInternal(maid);
            //RpcDeployMAID(maid);
        }

        private void DeployMAIDInternal(GameObject maid)
        {
            //      CloudburstPlugin.Destroy(this.maid);
            //    CloudburstPlugin.Destroy(winch);
            //  CCUtilities.LogI("Deployed maid!");

            this.maid = maid;
            //RpcSetRetrieve();
        }

        public void GetMAID()
        {
            // OnRetrival.Invoke(true, skillLocator.special);
            //Destroy(winch);

        }
        
        public GameObject GetWinch(GameObject winch)
        {
            if (this.winch)
            {
                Destroy(this.winch);

            }
            this.winch = winch;

            return maid;
        }
        #endregion

        #region Retrival

        private void RetrieveMAIDInternal()
        {
            if (maid)
            {

                _stopwatch = 0;
                startReel = true;
                body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                characterMotor.Motor.ForceUnground();

                var modelAnimator = base.GetComponent<ModelLocator>().modelTransform.GetComponent<Animator>();
                int layerIndex = modelAnimator.GetLayerIndex("Fullbody, Override");
                modelAnimator.speed = 1f;
                modelAnimator.Update(0f);
                modelAnimator.PlayInFixedTime("kick", layerIndex, 0f);
                //base.PlayAnimation("Fullbody, Override", "kick");
                maid.GetComponent<MAIDProjectileController>().FullStop();
                //Destroy(maid);
            }
        }

        public void RetrieveMAIDAuthority()
        {
            if (NetworkServer.active)
            {
                RpcRetrieveMAID();
                return;
            }
            CmdRetrieveMAIDInternal();
        }
        [ClientRpc]
        private void RpcRetrieveMAID()
        {
            RetrieveMAIDInternal();
        }
        [Command]
        private void CmdRetrieveMAIDInternal()
        {
            RpcRetrieveMAID();
        }
        #endregion


        [ClientRpc]
        private void RpcSetDeploy(bool natRetrival, Vector3 vector3)
        {
            //  CCUtilities.LogI("invoke");
            OnRetrival?.Invoke(natRetrival, skillLocator.special, vector3);
            //  skillLocator.special.UnsetSkillOverride(this, Custodian.throwPrimary, GenericSkill.SkillOverridePriority.Replacement);
            //skillLocator.special.SetSkillOverride(this, Custodian.throwPrimary, GenericSkill.SkillOverridePriority.Replacement);
        }
        //[ClientRpc]
        //private void RpcSetRetrieve()
        //{
        //    OnDeploy?.Invoke(skillLocator.special);
        //}
    }
}