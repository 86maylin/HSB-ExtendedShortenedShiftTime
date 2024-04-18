using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
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
        public static ConfigEntry<float> Config_shiftTime { get; private set; }
        public static ConfigEntry<float> Config_maxShiftTime { get; private set; }
        public static ManualLogSource LoggerInstance;

        void Awake()
        {
            LoggerInstance = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            SetConfigs();
            if (Config_enabled.Value)
            {
                harmony.PatchAll();
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
            Config_shiftTime = Config.Bind("Config", "ShiftTime", 20f, "Shift time in minutes.");
            Config_maxShiftTime = Config.Bind("Config", "MaxShiftTime", 45f, "Max shift time(for slider) in minutes. If this is too large then slider might not be precise.");
        }

        public static void SetConfigShiftTime(float value)
        {
            Config_shiftTime.Value = value;
        }
    }
}
