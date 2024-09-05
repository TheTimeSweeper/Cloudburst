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

        public static GameObject fireworkPrefab;

        public static void Setup()
        {
            blastBootItem = ScriptableObject.CreateInstance<ItemDef>();
            blastBootItem.tier = ItemTier.Tier1;
            blastBootItem.deprecatedTier = ItemTier.Tier1;
            blastBootItem.name = "Blast Boot";
            blastBootItem.pickupIconSprite = Cloudburst.OldCloudburstAssets.LoadAsset<Sprite>("texBlastBoot");
            //blastBootItem.pickupModelPrefab = Cloudburst.OldCloudburstAssets.LoadAsset<GameObject>("IMDLBoot");
            blastBootItem.nameToken = "ITEM_SECONDARYBOOST_NAME";
            blastBootItem.descriptionToken = "ITEM_SECONDARYBOOST_DESCRIPTION";
            blastBootItem.loreToken = "ITEM_SECONDARYBOOST_LORE";
            blastBootItem.requiredExpansion = Cloudburst.cloudburstExpansion;
            blastBootItem.tags = new ItemTag[]
            {
                ItemTag.Damage,
                ItemTag.Utility
            };


            CreateProjectile();

            ContentAddition.AddItemDef(blastBootItem);
            Hooks(blastBootItem);

            Modules.Language.Add("ITEM_SECONDARYBOOST_NAME", "Blast Boot");
            Modules.Language.Add("ITEM_SECONDARYBOOST_PICKUP", "Firework-powered double jump upon Secondary Skill activation.");
            Modules.Language.Add("ITEM_SECONDARYBOOST_DESC", "Activating your Secondary skill also blasts you through the air with a flurry of 4 <style=cStack>(+1)</style> <style=cIsDamage>fireworks that deal 100% <style=cStack>(+50%)</style> damage</style>. This effect has a cooldown of 3 seconds.");
            Modules.Language.Add("ITEM_SECONDARYBOOST_LORE", "");
        }

        public static void CreateProjectile()
        {
            GameObject firework = Addressables.LoadAssetAsync<GameObject>(key: "5babd0aad4d1df745842603d90dd2036").WaitForCompletion().InstantiateClone("EasyFirework");
            GameObject orig = Addressables.LoadAssetAsync<GameObject>(key: "041fe0a68f990b843acd9652941f8f87").WaitForCompletion();

            ProjectileController controler = firework.GetComponent<ProjectileController>();
            controler.ghostPrefab = orig.GetComponent<ProjectileController>().ghostPrefab;

            foreach (var soundEvents in firework.GetComponents<AkEvent>())
            {
                Cloudburst.Destroy(soundEvents);
            }

            foreach (var originalSound in orig.GetComponents<AkEvent>())
            {
                CCUtilities.CopyComponent<AkEvent>(originalSound, firework);
            }

            firework.GetComponent<ProjectileImpactExplosion>().impactEffect = orig.GetComponent<ProjectileImpactExplosion>().impactEffect;
            firework.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;

            fireworkPrefab = firework;
            ProjectileRegisterHelper.Projectiles.RegisterProjectile(fireworkPrefab);
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

        private static void Hooks(ItemDef itemDefToHooks)
        {
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }
    }
}
