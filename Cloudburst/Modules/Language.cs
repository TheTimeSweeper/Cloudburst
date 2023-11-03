//using R2API;
using Zio.FileSystems;
using System.Linq;
using System;

namespace Cloudburst.Modules {

    internal static class Language{

        public static string TokensOutput = "";

        public static bool printingEnabled => false;

        public static void PrintOutput(string preface = "")
        {
            if (!printingEnabled) return;

            Log.Warning($"{preface}\n{{\n    strings:\n    {{{TokensOutput}\n    }}\n}}");
            TokensOutput = "";
        }

        public static void Add(string token, string text)
        {
            if (!printingEnabled) return;

            TokensOutput += $"\n    \"{token}\" : \"{text.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n")}\",";
        }
    }
}