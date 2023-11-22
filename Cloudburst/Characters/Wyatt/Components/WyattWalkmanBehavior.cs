using System;
using System.Collections.Generic;
using Cloudburst.Characters.Wyatt;
using RoR2;
using RoR2.Stats;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Wyatt.Components
{
    public class WyattWalkmanBehavior : NetworkBehaviour, IOnDamageDealtServerReceiver
    {
        private CharacterBody characterBody;
        private ParticleSystem grooveEffect;
        private ParticleSystem grooveEffect2;
        private ChildLocator childLocator;
        private ParticleSystem flowEffect;
        private bool loseStacks { get { return stopwatch >= 3 && !flowing; } }

        private float stopwatch = 0;

        private float drainTimer = 0;

        private string _currentStage = "Default";

        public static Dictionary<string, string> sceneToStageName = new Dictionary<string, string>()
        {
            {"golemplains", "TitanicPlains"}  ,
            {"golemplains2", "TitanicPlains"} ,
            {"itgolemplains", "TitanicPlains"},
            {"blackbeach", "DistantRoost"}    ,
            {"blackbeach2", "DistantRoost"}   ,
            {"snowyforest", "SiphonedForest"}   ,

            {"goolake", "AbandonedAqueduct"}   ,
            {"itgoolake", "AbandonedAqueduct"}   ,
            {"foggyswamp", "WetlandAspect"}   ,
            {"ancientloft", "AphelianSanctuary"}   ,

            {"frozenwall", "RallypointDelta"}   ,
            {"itfrozenwall", "RallypointDelta"}   ,
            {"wispgraveyard", "ScorchedAcres"}   ,
            {"sulfurpools", "SulfurPools"}   ,

            {"dampcavesimple", "AbyssalDepths" },
            {"itdampcave", "AbyssalDepths" },
            {"shipgraveyard", "SirensCall" },
            {"rootjungle", "SunderedGrove" },

            {"skymeadow", "SkyMeadow" },
            {"itskymeadow", "SkyMeadow" },

            {"moon", "Moon" },
            {"moon2", "Moon" },
            {"itmoon", "Moon" },

            {"mithrix", "Mithrix" },

            {"arena", "VoidFields" },
            {"voidstage", "VoidLocus" },
            {"voidraid", "ThePlanetarium" },

            {"artifactworld", "BulwarksAmbry" },
            {"bazaar", "Bazaar" },
            {"goldshores", "GildedCoast" },
            {"limbo", "AMomentWhole" },
            {"mysteryspace", "AMomentFractured" },
        };


        [SyncVar]
        public bool flowing = false;

        private void Awake()
        {
            characterBody = base.GetComponent<CharacterBody>();
            childLocator = base.gameObject.GetComponentInChildren<ChildLocator>();

            grooveEffect = childLocator.FindChild("MusicEffect1").GetComponent<ParticleSystem>();
            grooveEffect2 = childLocator.FindChild("MusicEffect2").GetComponent<ParticleSystem>();

            flowEffect = childLocator.FindChild("MusicEffect3").GetComponent<ParticleSystem>();
        }

        //now I know why playing effects on the body is retared
        #region network Effects
        private void PlayGrooveEffectServer()
        {
            RpcPlayGrooveEFfect();
        }
        [ClientRpc]
        private void RpcPlayGrooveEFfect()
        {
            grooveEffect2.Play();
            grooveEffect.Play();
        }
        
        private void PlayFlowEffectServer()
        {
            RpcPlayFlowEFfect();
        }
        [ClientRpc]
        private void RpcPlayFlowEFfect()
        {
            flowEffect.Play();

            AkSoundEngine.SetRTPCValue("Wyatt_Groove_Volume", WyattConfig.M3FlowMusicVolume.Value);
            if (WyattConfig.M3FlowPlayMusic.Value)
            {
                SetCurrentStage();
                Util.PlaySound($"Play_Groove_{_currentStage}_Loop", gameObject);
            }
        }
        
        private void StopFlowEffectServer()
        {
            RpcPStopFlowEFfect();
        }
        [ClientRpc]
        private void RpcPStopFlowEFfect()
        {
            flowEffect.Stop();

            if (WyattConfig.M3FlowPlayMusic.Value)
            {
                Util.PlaySound($"Stop_Groove_{_currentStage}_Loop", gameObject);
                Util.PlaySound($"Play_Groove_{_currentStage}_End", gameObject);
            }
        }
        #endregion

        private void Start()
        {
            On.RoR2.CharacterBody.OnBuffFinalStackLost += CharacterBody_OnBuffFinalStackLost;
            BossGroup.onBossGroupStartServer += BossGroup_onBossGroupStartServer;
        }

        void OnDestroy() {
            On.RoR2.CharacterBody.OnBuffFinalStackLost -= CharacterBody_OnBuffFinalStackLost;
            BossGroup.onBossGroupStartServer -= BossGroup_onBossGroupStartServer;

            if (NetworkServer.active)
            {
                StopFlowEffectServer();
            }
        }

        private void CharacterBody_OnBuffFinalStackLost(On.RoR2.CharacterBody.orig_OnBuffFinalStackLost orig, CharacterBody self, BuffDef buffDef)
        {
            if (flowing && NetworkServer.active && characterBody == self && buffDef == WyattBuffs.wyattFlowBuffDef)
            {
                //flowing has stopped
                CCUtilities.SafeRemoveAllOfBuff(WyattBuffs.wyattGrooveBuffDef, characterBody);
                flowing = false;
                StopFlowEffectServer();
            }
            orig(self, buffDef);
        }

        private void BossGroup_onBossGroupStartServer(BossGroup bossGroup)
        {

        }

        public void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                //fixedupdate but only on server
                ServerFixedUpdate();
            }
        }

        private void ServerFixedUpdate()
        {
            if (flowing == false)
            {
                stopwatch += Time.fixedDeltaTime;
                if (loseStacks)
                {
                    drainTimer += Time.fixedDeltaTime;
                    if (drainTimer >= 0.5f)
                    {
                        CCUtilities.SafeRemoveBuffs(WyattBuffs.wyattGrooveBuffDef, characterBody, 2);
                        drainTimer = 0;
                    }
                }
            }
            
        }

        [Server]
        private void TriggerBehaviorInternal(float stacks)
        {
            var cap = 9 + stacks;
            if (characterBody && characterBody.GetBuffCount(WyattBuffs.wyattGrooveBuffDef) < cap)
            {
                PlayGrooveEffectServer();
                /*EffectManager.SpawnEffect(Effects.wyattGrooveEffect, new EffectData()
                {
                    scale = 1,
                    origin = grooveEffect.transform.position
                }, true);*/
                characterBody.AddBuff(WyattBuffs.wyattGrooveBuffDef);
                //characterBody.AddTimedBuff(Custodian.instance.wyattCombatDef, 3);
            }
            stopwatch = 0;
        }



        public void ActivateFlowAuthority()
        {
            if (NetworkServer.active)
            {
                ActivateFlowInternal();
                return;
            }
            CmdActivateFlow();
        }

        [Command]
        private void CmdActivateFlow()
        {
            ActivateFlowInternal();
        }

        [Server]
        private void ActivateFlowInternal()
        {
            int grooveCount = characterBody.GetBuffCount(WyattBuffs.wyattGrooveBuffDef);
            float duration = WyattConfig.M3FlowDurationBase.Value;// 4;

            for (int i = 0; i < grooveCount; i++)
            {
                //add flow until we can't
                duration += WyattConfig.M3FlowDurationPerStack.Value;// 0.4f;
            }

            characterBody.AddTimedBuff(WyattBuffs.wyattFlowBuffDef, duration);
            flowing = true;

            PlayFlowEffectServer();
        }

        public void OnDamageDealtServer(DamageReport damageReport)
        {
            if (flowing == false && R2API.DamageAPI.HasModdedDamageType(damageReport.damageInfo, WyattDamageTypes.applyGroove))
            {
                TriggerBehaviorInternal(1);
            }
        }


        private void SetCurrentStage()
        {
            //maybe check moon man first
            //check for bosses regardless of stage (linkin park for henry)

            string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (sceneToStageName.ContainsKey(scene))
            {
                _currentStage = sceneToStageName[scene];
            }
            if(scene == "moon" || scene == "moon2")
            {
                //_currentStage = "Mithrix";
            }
            
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKey($"[{i}]"))
                {
                    switch (i)
                    {
                        case 0:
                            _currentStage = "Default";
                            break;
                        case 1:
                            _currentStage = "TitanicPlainsOld";
                            break;
                        case 2:
                            _currentStage = "TitanicPlains";
                            break;
                        case 3:
                            _currentStage = "DistantRoost";
                            break;
                        case 4:
                            _currentStage = "SiphonedForest";
                            break;
                        case 5:
                            _currentStage = "WetlandAspect";
                            break;
                        case 6:
                            _currentStage = "AbandonedAqueduct";
                            break;
                    }
                }
            }
        }

    }
}