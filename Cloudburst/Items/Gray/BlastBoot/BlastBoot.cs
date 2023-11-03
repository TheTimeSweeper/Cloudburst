using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Gray.BlastBoot
{
    internal class BlastBoot
    {
        public static ItemDef blastBootItem;

        public static GameObject firework;

        public static void Setup()
        {
            blastBootItem = ScriptableObject.CreateInstance<ItemDef>();
            blastBootItem.tier = ItemTier.Tier1;
            blastBootItem.name = "itemsecondaryboost";
            blastBootItem.nameToken = "ITEM_SECONDARYBOOST_NAME";
            blastBootItem.descriptionToken = "ITEM_SECONDARYBOOST_DESCRIPTION";
            blastBootItem.loreToken = "ITEM_SECONDARYBOOST_LORE";
            blastBootItem.requiredExpansion = Cloudburst.cloudburstExpansion;

            CreateProjectile();

            ContentAddition.AddItemDef(blastBootItem);
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;

            Modules.Language.Add("ITEM_BARRIERONCRIT_NAME", "Blast Boot");
        }

        public static void CreateProjectile()
        {
            firework = Addressables.LoadAssetAsync<GameObject>(key: "5babd0aad4d1df745842603d90dd2036").WaitForCompletion().InstantiateClone("EasyFirework");
            GameObject orig = Addressables.LoadAssetAsync<GameObject>(key: "041fe0a68f990b843acd9652941f8f87").WaitForCompletion();

            ProjectileController controler = firework.GetComponent<ProjectileController>();
            controler.ghostPrefab = orig.GetComponent<ProjectileController>().ghostPrefab;

            foreach (var soundEvent in firework.GetComponents<AkEvent>())
            {
                GameObject.Destroy(soundEvent);
            }

            foreach (var originalSound in orig.GetComponents<AkEvent>())
            {
                CopyComponent<AkEvent>(originalSound, firework);
            }

            firework.GetComponent<ProjectileImpactExplosion>().impactEffect = orig.GetComponent<ProjectileImpactExplosion>().impactEffect;
            firework.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;

            
        }

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }

        private static void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {

            self.AddItemBehavior<BlastBootBehavior>(self.inventory.GetItemCount(blastBootItem));
            orig(self);
        }
    }
}
