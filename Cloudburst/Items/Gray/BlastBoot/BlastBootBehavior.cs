using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Items.Gray.BlastBoot
{
    internal class BlastBootBehavior : CharacterBody.ItemBehavior
    {
        public float timer = 3;

        public SkillLocator skillLocator;

        public void OnEnable()
        {
            skillLocator = body.skillLocator;
            
            body.onSkillActivatedServer += Body_onSkillActivatedServer;
        }

        public void OnDisable()
        {
            body.onSkillActivatedServer -= Body_onSkillActivatedServer;
        }
        private void Body_onSkillActivatedServer(GenericSkill obj)
        {
            if(skillLocator.secondary == obj)
            {
                if(body.characterMotor && body.characterMotor.isGrounded)
                {
                    body.characterMotor.ApplyForce(new Vector3(0, body.jumpPower * 600f, 0), true);
                    body.characterMotor.Motor.ForceUnground();

                    Vector3 aimDirection = Vector3.down;

                    for (int i = 0; i < 3; i++)
                    {
                        float theta = UnityEngine.Random.Range(0, 6.28f);
                        float x = Mathf.Cos(theta);
                        float z = Mathf.Sin(theta);
                        float c = i * 0.3777f;
                        c *= (1f / 12f);
                        aimDirection.x += c * x;
                        aimDirection.z += c * z;

                        ProjectileManager.instance.FireProjectile(BlastBoot.firework, transform.position, Util.QuaternionSafeLookRotation(aimDirection),
                            gameObject, 0.5f + (0.5f * stack), 500, body.RollCrit(), DamageColorIndex.Item);
                    }
                }
            }
        }
    }
}
