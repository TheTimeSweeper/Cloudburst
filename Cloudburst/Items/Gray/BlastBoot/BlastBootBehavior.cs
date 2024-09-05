using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Items.Gray.BlastBoot
{
    public class BlastBootBehavior : CharacterBody.ItemBehavior
    {
        public float timer = 3;

        public SkillLocator skillLocator;


        public void Awake()
        {
            base.enabled = false;
        }

        public void OnEnable()
        {
            if (body)
            {
                skillLocator = body.skillLocator;
                body.onSkillActivatedServer += Body_onSkillActivatedServer;
            }
        }

        public void OnDisable()
        {
            skillLocator = null;
            if (body)
            {
                body.onSkillActivatedServer -= Body_onSkillActivatedServer;
            }
        }
        private void Body_onSkillActivatedServer(GenericSkill skill)
        {
            if (((skillLocator != null) ? skillLocator.secondary : null) == skill && body.characterMotor && !body.characterMotor.isGrounded && timer >= 3)
            {
                Vector3 aimer = Vector3.down;
                for (int j = 0; j < 3 + (stack * 1); j++)
                {
                    body.characterMotor.velocity.y += body.jumpPower * .3f;
                    body.characterMotor.Motor.ForceUnground();

                    float theta = UnityEngine.Random.Range(0.0f, 6.28f);
                    float x = Mathf.Cos(theta);
                    float z = Mathf.Sin(theta);
                    float c = j * 0.3777f;
                    c *= (1f / 12f);
                    aimer.x += c * x;
                    aimer.z += c * z;
                    float damage = CCUtilities.GenericFlatStackingFloat(1f, stack, 0.5f);
                    ProjectileManager.instance.FireProjectile(BlastBoot.fireworkPrefab,
                        base.transform.position,
                        Util.QuaternionSafeLookRotation(aimer),
                        body.gameObject,
                        damage * body.damage,
                        500f,
                        body.RollCrit(),
                        DamageColorIndex.Item,
                        null,
                        -1
                        );
                    aimer = Vector3.down;
                    timer = 0;
                }
            }
        }

        public void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
        }
    }
}
