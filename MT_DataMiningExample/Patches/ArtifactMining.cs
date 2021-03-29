using HarmonyLib;
using MT_DataMiningExample.Managers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MT_DataMiningExample.Patches
{
    [HarmonyPatch(typeof(MainMenuScreen))]
    [HarmonyPatch("Initialize")]
    class ArtifactMining
    {
        // Insert a path here
        static readonly string csvPath = @"";
        static readonly string csvFilename = "relicDatas.csv";

        static bool ran = false;

        static readonly Dictionary<string, string> tokenReplacements = new Dictionary<string, string>
        {
            { "<nobr>", "" },
            { "</nobr>", "" },
            { "<b>", "" },
            { "</b>", "" },
            { "<br>", "" },
        };

        static string ReplaceTokens(string text)
        {
            foreach (var entry in tokenReplacements)
            {
                text = text.Replace(entry.Key, entry.Value);
            }

            return text;
        }

        static void Postfix()
        {
            if (!ran)
            {
                var saveManager = ProviderManager.SaveManager;

                if (saveManager != null)
                {
                    var data = ProviderManager.SaveManager?.GetAllGameData().GetAllCollectableRelicData().OrderBy(r => r.GetID());

                    var csvStringBuilder = new StringBuilder();
                    csvStringBuilder.AppendLine("Id;Name;Description");

                    foreach (var relicData in data)
                    {
                        var state = new RelicState(relicData);
                        var desc = ReplaceTokens(state.GetDescription(saveManager.RelicManager));
                        csvStringBuilder.AppendLine($"{relicData.GetID()};{relicData.GetNameEnglish()};{desc}");
                    }

                    File.WriteAllText(Path.Combine(csvPath, csvFilename), csvStringBuilder.ToString());

                    ran = true;
                }
            }
        }
    }
}
