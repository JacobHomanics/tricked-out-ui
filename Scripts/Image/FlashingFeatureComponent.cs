using System;
using UnityEngine;
using UnityEngine.UI;

namespace JacobHomanics.TrickedOutUI
{
    /// <summary>
    /// MonoBehaviour component that creates a flashing effect when a value falls below a threshold.
    /// </summary>
    public class FlashingFeatureComponent : BaseCurrentMaxComponent
    {
        public Image flashImage;
        public float thresholdPercent = 0.2f;
        public Color flashColor1 = Color.red;
        public Color flashColor2 = Color.white;
        public float flashSpeed = 15f;

        public enum ThresholdType
        {
            below, above
        }

        public ThresholdType thresholdType;

        void Update()
        {
            // var result = FlashingFeatureCommand(thresholdPercent, flashColor1, flashColor2, flashSpeed, Current, Max, thresholdType);
            flashImage.enabled = IsEnabled(thresholdType, Current, Max, thresholdPercent);
            flashImage.color = CalcColor(flashSpeed, flashColor1, flashColor2);
            flashImage.fillAmount = Something(Current, Max);
        }

        // public static (bool, Color, float) FlashingFeatureCommand(float thresholdPercent, Color flashColor1, Color flashColor2, float flashSpeed, float current, float max, ThresholdType thresholdType)
        // {
        //     float healthPercent;
        //     healthPercent = current / max;

        //     bool condition = false;
        //     if (thresholdType == ThresholdType.below)
        //         condition = healthPercent <= thresholdPercent;
        //     if (thresholdType == ThresholdType.above)
        //         condition = healthPercent >= thresholdPercent;

        //     float flashValue = Mathf.Sin(Time.time * flashSpeed) * 0.5f + 0.5f;
        //     Color flashColor = Color.Lerp(flashColor1, flashColor2, flashValue);

        //     return (condition, flashColor, current / max);
        // }

        public static bool IsEnabled(ThresholdType thresholdType, float current, float max, float thresholdPercent)
        {
            float healthPercent;
            healthPercent = current / max;

            bool condition = false;
            if (thresholdType == ThresholdType.below)
                condition = healthPercent <= thresholdPercent;
            if (thresholdType == ThresholdType.above)
                condition = healthPercent >= thresholdPercent;

            return condition;
        }

        public static Color CalcColor(float flashSpeed, Color flashColor1, Color flashColor2)
        {
            float flashValue = Mathf.Sin(Time.time * flashSpeed) * 0.5f + 0.5f;
            Color flashColor = Color.Lerp(flashColor1, flashColor2, flashValue);
            return flashColor;
        }

        public static float Something(float value1, float value2)
        {
            return value1 / value2;
        }
    }
}

