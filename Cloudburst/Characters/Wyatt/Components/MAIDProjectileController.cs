using Cloudburst.Characters;
using Cloudburst.Characters.Wyatt;
using Cloudburst.GlobalComponents;
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
    class MAIDProjectileController : MonoBehaviour
    {
        private ProjectileController controller;

        private bool stop = false;
        private bool triggered = false;
        private float stopwatch = 0;
        private bool pause = false;
        private Rigidbody body;
        private GameObject owner = null;
        ProjectileProximityBeamController beamer;
        private BoomerangProjectile boomer;
        private List<Rigidbody> bodies;
        private Vector3 distance;
        private Animator animator;

        public void Awake()
        {
            bodies = new List<Rigidbody>();
            controller = base.gameObject.GetComponent<ProjectileController>();
            boomer = base.gameObject.GetComponent<BoomerangProjectile>();
            beamer = GetComponent<ProjectileProximityBeamController>();
            body = GetComponent<Rigidbody>();
            stop = false;
        }

        public void Start()
        {
            //I AM VIOLATING MVC PLEAS FORGIV
            animator = controller.ghost.GetComponent<Animator>();
            animator.Play("Zoom");
            
            owner = controller.owner;

            owner.gameObject.GetComponent<WyattMAIDManager>().DeployMAIDAuthority(base.gameObject);
            boomer.onFlyBack.AddListener(OnHit);
        }

        private void OnHit()
        {

        }

        private bool PassesFilter(TeamFilter filter)
        {
            if (filter.teamIndex != TeamIndex.Player)
            {
                return true;
            }
            return false;
        }


        private void OnTriggerEnter(Collider other)
        {
            Rigidbody component = other.GetComponent<Rigidbody>();
            ProjectileController controller = other.GetComponent<ProjectileController>();
            //prevent multiple cringes from being cringed!
            if (component && controller && PassesFilter(controller.teamFilter) && !controller.gameObject.GetComponent<ProjectileSlow>())
            {
                EffectData effectData = new EffectData
                {
                    origin = component.transform.position,
                    //pls god
                    start = base.transform.position
                };
                EffectManager.SpawnEffect(WyattEffects.maidTouchEffect, effectData, true);

                EffectData nads = new EffectData
                {
                    origin = controller.transform.position,
                    scale = 1,
                    //pls vs
                };
                EffectManager.SpawnEffect(WyattEffects.maidCleanseEffect, nads, true);

                Util.PlaySound("step_land_shallow_water_01", component.gameObject);

                ProjectileSlow cing = controller.gameObject.AddComponent<ProjectileSlow>();
                cing.maxVelocityMagnitude = 3;
                cing.antiGravity = 1;

                //var effect = controller.gameObject.AddComponent<ProjectileEffectManager>();
                //effect.effect = WyattEffects.maidTriggerEffect;
            }
        }

        public void FixedUpdate()
        {
            if (boomer.boomerangState == BoomerangProjectile.BoomerangState.Transition && triggered == false)
            {
                triggered = true;
                pause = true;
                if(beamer)
                    beamer.enabled = true;
                boomer.enabled = false;
                animator.Play("Idle");
                return;
            }
            if (stop == true)
            {
                body.velocity = Vector3.zero;
            };
            if (pause == true)
            {
                stopwatch += Time.fixedDeltaTime;
                body.velocity = Vector3.zero;
                if (stopwatch >= 2)
                {
                    Unpause();
                }

            }
        }
        public bool Destroying = false;
        public void OnDestroy()
        {
            if (Destroying == false)
            {
                Destroying = true;
                owner.gameObject?.GetComponent<WyattMAIDManager>()?.GetMAID();

                owner.gameObject.GetComponent<WyattMAIDManager>().Invoke(stop, distance);

                //owner.gameObject?.GetComponent<SkillLocator>()?.special?.SetSkillOverride(this, Custodian.throwPrimary, GenericSkill.SkillOverridePriority.Contextual); ; }
            }
        }
        public void Unpause()
        {
            pause = false;

            if (beamer)
                beamer.enabled = false;
            boomer.enabled = true;

            animator.Play("Zoom");
            transform.LookAt(controller.owner.transform);
        }

        public void FullStop()
        {
            stop = true;
            distance = (base.transform.position - owner.gameObject.transform.position).normalized;// * 120f;
            boomer.enabled = false;
        }
    }

}
