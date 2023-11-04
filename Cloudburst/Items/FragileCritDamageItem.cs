using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items
{
    public class FragileCritDamageItem : ItemBase
    {
        public override string name => "FragileCritDamage";

        private Material matShatteredGlass = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matShatteredGlass.mat").WaitForCompletion();
        private Material matShatteredHarvesterGlass;

        private GameObject BrittleDeath = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/BrittleDeath.prefab").WaitForCompletion();
        private GameObject FragileCritDamageBreak;

        public FragileCritDamageItem()
        {
            matShatteredHarvesterGlass = GameObject.Instantiate<Material>(matShatteredGlass);
            matShatteredHarvesterGlass.name = "matShatteredHarvesterGlass";
            matShatteredHarvesterGlass.SetColor("_TintColor", new Color(1, 1, 0.4f));
            matShatteredHarvesterGlass.DisableKeyword("_EMISSION");

            FragileCritDamageBreak = BrittleDeath.InstantiateClone("FragileCritDamageBreak");
            FragileCritDamageBreak.GetComponent<ParticleSystemRenderer>().material = matShatteredHarvesterGlass;
            GameObject.Destroy(FragileCritDamageBreak.transform.GetChild(0).gameObject);
            GameObject.Destroy(FragileCritDamageBreak.transform.GetChild(0).gameObject);

            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            IL.RoR2.HealthComponent.UpdateLastHitTime += HealthComponent_UpdateLastHitTime;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.inventory)
            {
                int itemCount = sender.inventory.GetItemCount(CloudburstContent.Items.FragileCritDamage);
                args.critAdd += itemCount > 0 ? 5 : 0;
                args.critDamageMultAdd += itemCount > 0 ? itemCount * 0.30f + 0.10f : 0;
            }
        }

        private void HealthComponent_UpdateLastHitTime(MonoMod.Cil.ILContext il)
        {
            ILCursor ilCursor = new ILCursor(il);
            if (ilCursor.TryGotoNext(MoveType.Before,
                inst => inst.MatchBrfalse(out _)))
            {
                ilCursor.Index += 3;
                ilCursor.Emit(OpCodes.Ldarg_0);
                ilCursor.EmitDelegate<Action<HealthComponent>>((self) => {
                    int itemCount = self.body.inventory.GetItemCount(CloudburstContent.Items.FragileCritDamage);

                    if (itemCount > 0 && self.isHealthLow)
                    {
                        self.body.inventory.RemoveItem(CloudburstContent.Items.FragileCritDamage, itemCount);
                        self.body.inventory.GiveItem(CloudburstContent.Items.FragileCritDamageConsumed, itemCount);

                        CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, CloudburstContent.Items.FragileCritDamage.itemIndex, CloudburstContent.Items.FragileCritDamageConsumed.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                    }

                    // apply experience one by one so new levels get proper amount
                    while (itemCount > 0 && self.isHealthLow)
                    {
                        itemCount--;
                        if (self.body.teamComponent)
                        {
                            TeamIndex teamIndex = self.body.teamComponent.teamIndex;
                            ulong expNeed = TeamManager.instance.GetTeamNextLevelExperience(teamIndex) - TeamManager.instance.GetTeamCurrentLevelExperience(teamIndex);
                            expNeed = (ulong)(expNeed * 0.15f);
                            TeamManager.instance.GiveTeamExperience(teamIndex, expNeed);
                        }
                    }
                });
            }
            else
            {
                Log.Warning("Failed to apply hook for FragileCritDamageItem");
            }
        }

        protected override void AddToContentPack(ContentPack contentPack)
        {
            base.AddToContentPack(contentPack);
            contentPack.effectDefs.AddItem(new EffectDef(FragileCritDamageBreak));
        }

    }
}
