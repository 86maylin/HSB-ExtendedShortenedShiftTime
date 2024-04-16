using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ExtendShortenShiftTime.Patches;
using HarmonyLib;

namespace ExtendShortenShiftTime
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class ExtendShortenShiftTime : BaseUnityPlugin
    {
        private const string modGUID = "86maylin.extend_shorten_shift_time";
        private const string modName = "Extend/Shorten Shift Time";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        public static ConfigEntry<bool> Config_enabled { get; private set; }
        public static ConfigEntry<int> Config_shiftTime { get; private set; }
        public static ConfigEntry<bool> Config_ShiftTimeInSeconds { get; private set; }
        public static ManualLogSource LoggerInstance;

        void Awake()
        {
            LoggerInstance = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            SetConfigs();
            if (Config_enabled.Value)
            {
                harmony.PatchAll(typeof(ExtendShortenShiftTime));
                harmony.PatchAll(typeof(GameSessionTimerDataPatch));
                LoggerInstance.LogInfo("ExtendShortenShiftTime patched.");
            }
            else
            {
                LoggerInstance.LogInfo("ExtendShortenShiftTime disabled.");
            }
        }

        private void SetConfigs()
        {
            Config_enabled = Config.Bind("Config", "Enabled", true, "Whether to enable the plugin or not.");
            Config_shiftTime = Config.Bind("Config", "ShiftTime", 20, "Shift time in minutes(if ShiftTimeInSeconds is true then it will be in seconds).");
            Config_ShiftTimeInSeconds = Config.Bind("Config", "ShiftTimeInSeconds", false, "Whether to specify ShiftTime in seconds or not(to allow finer tunning).");
        }
    }
}
