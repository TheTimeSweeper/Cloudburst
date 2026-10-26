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
        //starts charged so the boot is usable right after pickup or a new stage
        public float timer = BlastBoot.Cooldown;

        public void Awake()
        {
            base.enabled = false;
        }

        public void OnEnable()
        {
            if (body)
            {
                //authority, not server: the motor belongs to the owning client so velocity set on the host does nothing for clients
                body.onSkillActivatedAuthority += Body_onSkillActivatedAuthority;
            }
        }

        public void OnDisable()
        {
            if (body)
            {
                body.onSkillActivatedAuthority -= Body_onSkillActivatedAuthority;
            }
        }

        private void Body_onSkillActivatedAuthority(GenericSkill skill)
        {
            if (timer < BlastBoot.Cooldown || !body.skillLocator || body.skillLocator.secondary != skill)
                return;

            CharacterMotor motor = body.characterMotor;
            if (!motor || motor.isGrounded)
                return;

            timer = 0;

            //cancel the fall so the boost acts like a real double jump
            motor.velocity.y = Mathf.Max(motor.velocity.y, 0f);
            motor.Motor.ForceUnground();

            int fireworks = BlastBoot.BaseFireworks + stack;
            float damage = CCUtilities.GenericFlatStackingFloat(BlastBoot.BaseDamage, stack, BlastBoot.StackingDamage) * body.damage;
            bool crit = body.RollCrit();
            for (int i = 0; i < fireworks; i++)
            {
                motor.velocity.y += body.jumpPower * 0.3f;

                float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
                float spread = i * 0.3777f / 12f;
                Vector3 aim = Vector3.down + new Vector3(Mathf.Cos(theta) * spread, 0f, Mathf.Sin(theta) * spread);

                ProjectileManager.instance.FireProjectile(BlastBoot.fireworkPrefab,
                    body.footPosition,
                    Util.QuaternionSafeLookRotation(aim),
                    body.gameObject,
                    damage,
                    500f,
                    crit,
                    DamageColorIndex.Item,
                    null,
                    -1
                    );
            }
        }

        public void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
        }
    }
}
