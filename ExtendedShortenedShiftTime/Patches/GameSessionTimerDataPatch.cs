using BBI.Unity.Game;
using HarmonyLib;

namespace ExtendShortenShiftTime.Patches
{
    [HarmonyPatch(typeof(GameSessionTimerData))]
    internal class GameSessionTimerDataPatch
    {

        [HarmonyPatch(nameof(GameSessionTimerData.FromLevelAndDifficultyData))]
        [HarmonyPostfix]
        static void FromLevelAndDifficultyData_Postfix(LevelAsset.LevelData levelData, DifficultyModeAsset difficultyMode, ref GameSessionTimerData __result)
        {
            ExtendShortenShiftTime.LoggerInstance.LogInfo("FromLevelAndDifficultyData_Postfix");
            bool enabled = false;
            bool active = false;
            float completionTimeInSeconds = levelData.CompletionTimeInSeconds;
            bool flag = (difficultyMode != null) ? difficultyMode.TimerCountsUp : levelData.TimerCountsUp;
            if (flag)
            {
                enabled = true;
                active = true;
            }
            else if (completionTimeInSeconds > 0f)
            {
                enabled = true;
                active = true;
            }
            if (levelData.SessionType == GameSession.SessionType.Career && levelData.CompletionTimeInSeconds != 0)
            {
                float value;
                if (ExtendShortenShiftTime.Config_ShiftTimeInSeconds.Value)
                {
                    value = ExtendShortenShiftTime.Config_shiftTime.Value;
                }
                else
                {
                    value = ExtendShortenShiftTime.Config_shiftTime.Value * 60;
                }
                ExtendShortenShiftTime.LoggerInstance.LogInfo("Set shift time to: " + value);
                __result = new GameSessionTimerData(flag, value, value, enabled, active);
            }
            else
            {
                __result = new GameSessionTimerData(flag, flag ? 0f : completionTimeInSeconds, flag ? 36000f : completionTimeInSeconds, enabled, active);
            }
        }
    }
}
