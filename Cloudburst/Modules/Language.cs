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

        public static void Add(string token, string text)
        {
            if (!printingEnabled) return;

            //add a token formatted to language file
            TokensOutput += $"\n    \"{token}\" : \"{text.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n")}\",";
        }

        public static void PrintOutput(string fileName = "")
        {
            if (!printingEnabled) return;

            //wrap all tokens in a properly formatted language file
            string strings = $"{{\n    strings:\n    {{{TokensOutput}\n    }}\n}}";

            //spit out language dump in console for copy paste if you want
            Log.Warning($"{fileName}: \n{strings}");

            //write a language file txt next to your mod. must have a folder called Language next to your mod dll.
            if (!string.IsNullOrEmpty(fileName))
            {
                string path = Path.Combine(Directory.GetParent(Info.Location).FullName, "Language", "en", fileName);
                File.WriteAllText(path, strings);
            }
            
            //empty the output each time this is printed, so you can print multiple language files
            TokensOutput = "";
        }
    }
}