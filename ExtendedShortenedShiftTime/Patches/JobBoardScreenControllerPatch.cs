using BBI.Unity.Game;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

namespace ExtendShortenShiftTime.Patches
{
    [HarmonyPatch(typeof(JobBoardScreenController))]
    internal class JobBoardScreenControllerPatch
    {
        static GameObject optionSlider;
        static LynxSlider shiftTimeSlider;
        static TextMeshProUGUI sliderValueText;

        [HarmonyPatch(nameof(JobBoardScreenController.ShowJobBoard))]
        [HarmonyPostfix]
        static void ShowJobBoard_Postfix()
        {
            if (optionSlider == null)
            {
                SetupSlider();
            }
        }

        static void SetupSlider()
        {
            optionSlider = Object.Instantiate(Object.FindObjectOfType<OptionEntrySlider>(true), GameObject.Find("PRF_UI_JobBoard").transform).gameObject;
            optionSlider.name = "Shift Time";
            Object.Destroy(optionSlider.GetComponent<OptionEntrySlider>());
            optionSlider.transform.localPosition = new Vector2(270, -432);
            ((RectTransform)optionSlider.transform).sizeDelta = new Vector2(500, 50);
            foreach (var image in optionSlider.GetComponentsInChildren<Image>())
            {
                if (image.name == "Background")
                {
                    if (image.GetComponent<LayoutElement>() != null)
                    {
                        image.color = Color.black;
                        break;
                    }
                }
            }
            foreach (var layoutGroup in optionSlider.GetComponentsInChildren<HorizontalLayoutGroup>())
            {
                if (layoutGroup.name == "Option Text")
                {
                    var text = layoutGroup.transform.GetChild(0);
                    Object.Destroy(text.GetComponent<LocalizedTextMeshProUGUI>());
                    text.GetComponent<TextMeshProUGUI>().text = "Shift Time";
                }
                else if (layoutGroup.name == "Option Control")
                {
                    sliderValueText = layoutGroup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                }
            }
            shiftTimeSlider = optionSlider.GetComponentInChildren<LynxSlider>();
            shiftTimeSlider.onValueChanged.RemoveAllListeners();
            shiftTimeSlider.name = "ShiftTimeSlider";
            shiftTimeSlider.wholeNumbers = true;
            if (ExtendShortenShiftTime.Config_maxShiftTime.Value >= ExtendShortenShiftTime.Config_shiftTime.Value)
            {
                shiftTimeSlider.maxValue = ExtendShortenShiftTime.Config_maxShiftTime.Value * 2;
            }
            else
            {
                shiftTimeSlider.maxValue = ExtendShortenShiftTime.Config_shiftTime.Value * 2;
            }
            shiftTimeSlider.minValue = 1f;
            sliderValueText.text = ExtendShortenShiftTime.Config_shiftTime.Value.ToString();
            shiftTimeSlider.SetValueWithoutNotify(ExtendShortenShiftTime.Config_shiftTime.Value * 2);
            shiftTimeSlider.onValueChanged.AddListener(new UnityAction<float>(OnShiftTimeSliderChanged));
            ExtendShortenShiftTime.LoggerInstance.LogInfo("ShiftTimeSlider Instantiated");
        }

        static void OnShiftTimeSliderChanged(float value)
        {
            value *= 0.5f;
            sliderValueText.text = value.ToString();
            ExtendShortenShiftTime.SetConfigShiftTime(value);
        }
    }
}
