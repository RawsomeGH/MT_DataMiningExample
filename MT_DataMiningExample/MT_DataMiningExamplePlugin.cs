using BepInEx;
using HarmonyLib;
using MT_DataMiningExample.Managers;

namespace MT_DataMiningExample
{
    [BepInPlugin(GUID, "Data Mining Example", "1.0.0.0")]
    [BepInProcess("MonsterTrain.exe")]
    public class MT_DataMiningExamplePlugin : BaseUnityPlugin
    {
        public const string GUID = "rawsome.modster-train.data-mining-example";

        void Awake()
        {
            DepInjector.AddClient(new ProviderManager());

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }
    }
}
