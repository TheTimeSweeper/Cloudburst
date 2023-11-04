using RoR2;
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
            itemDef = ScriptableObject.CreateInstance<ItemDef>();
            (itemDef as ScriptableObject).name = name;


            itemDef.AutoPopulateTokens();
        }

        public ItemDef Return()
        {
            return itemDef;
        }
    }
}
