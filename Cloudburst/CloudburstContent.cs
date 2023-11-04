using Cloudburst.Items;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RoR2.DLC1Content;

namespace Cloudburst
{
    public class CloudburstContent : IContentPackProvider
    {
        private static readonly string addressablesLabel = "ContentPack:Cbt:Cloudburst";
        public string identifier => "Cbt:Cloudburst";
        private ContentPack contentPack = new ContentPack();

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(contentPack, args.output);
            yield break;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Member Access", "Publicizer001:Accessing a member that was not originally public", Justification = "<Pending>")]
        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            contentPack.identifier = identifier;

            AddressablesLoadHelper loadHelper = AddressablesLoadHelper.CreateUsingDefaultResourceLocator(addressablesLabel);
            loadHelper.AddContentPackLoadOperation(contentPack);
            loadHelper.AddGenericOperation(delegate
            {
                contentPack.itemDefs.Add(new ItemDef[]
                {
                    new FragileCritDamageItem().Return(),
                    new FragileCritDamageConsumedItem().Return(),
                }.Where(x => x != null).ToArray());
            });
            loadHelper.AddGenericOperation(delegate
            {
                ContentLoadHelper.PopulateTypeFields(typeof(Items), contentPack.itemDefs);
            }, 0.05f);

            while (loadHelper.coroutine.MoveNext())
            {
                args.ReportProgress(loadHelper.progress.value);
                yield return loadHelper.coroutine.Current;
            }
        }

        public static class Items
        {
            public static ItemDef FragileCritDamage;
            public static ItemDef FragileCritDamageConsumed;

        }
    }
}
