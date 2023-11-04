using HarmonyLib;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cloudburst.Items
{
    public abstract class ItemBase
    {
        public virtual string name => "";
        protected ItemDef itemDef {  get; private set; }
        protected ItemIndex itemIndex { get; private set; }

        public ItemBase() 
        {
            if (Modules.Config.BindAndOptions<bool>("Items", $"Enable {name}", true, $"Determines whether or not the {name} item should be enabled.").Value) return;

            itemDef = ScriptableObject.CreateInstance<ItemDef>();
            (itemDef as ScriptableObject).name = name;


            itemDef.AutoPopulateTokens();
        }

        public void Return(ContentPack contentPack)
        {
            if(itemDef)
            {
                AddToContentPack(contentPack);
            }
        }

        protected virtual void AddToContentPack(ContentPack contentPack)
        {
            contentPack.itemDefs.AddItem(itemDef);
        }
    }
}
