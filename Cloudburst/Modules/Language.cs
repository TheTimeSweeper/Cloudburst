//using R2API;
using Zio.FileSystems;
using System.Linq;
using System;
using System.IO;
using BepInEx;

namespace Cloudburst.Modules {

    internal static class Language{

        public static string TokensOutput = "";

        public static bool printingEnabled => false;

        public static PluginInfo Info;

        internal static void Init(PluginInfo info)
        {
            Info = info;
        }

        public static void PrintOutput(string preface = "")
        {
            if (!printingEnabled) return;
            string strings = $"{{\n    strings:\n    {{{TokensOutput}\n    }}\n}}";
            Log.Warning($"{preface}: \n{strings}");

            if (!string.IsNullOrEmpty(preface))
            {
                string path = Path.Combine(Directory.GetParent(Info.Location).FullName, "Language", "en", preface);
                File.WriteAllText(path, strings);
            }
            
            TokensOutput = "";
        }

        public static void Add(string token, string text)
        {
            if (!printingEnabled) return;

            TokensOutput += $"\n    \"{token}\" : \"{text.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n")}\",";
        }
    }
}