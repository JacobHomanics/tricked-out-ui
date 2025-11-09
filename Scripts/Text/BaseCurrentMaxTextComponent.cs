using UnityEngine;
using TMPro;

namespace JacobHomanics.UI
{
    [System.Serializable]
    public class TextProperties
    {
        public TMP_Text text;

        public enum DisplayType
        {
            Current, Max, Difference
        }

        public DisplayType displayType;
        public string format = "#,##0";

        public bool clampAtMax;
        public bool clampAtZero;
    }

    public abstract class BaseCurrentMaxTextComponent : BaseCurrentMaxComponent
    {

        public void Display(TMP_Text text, TextProperties.DisplayType displayType, float current, float max, string format, bool clampAtZero, bool clampAtMax)
        {
            if (displayType == TextProperties.DisplayType.Current)
                Display(text, current, max, format, clampAtZero, clampAtMax);
            if (displayType == TextProperties.DisplayType.Max)
                Display(text, max, max, format, clampAtZero, clampAtMax);
            if (displayType == TextProperties.DisplayType.Difference)
                Display(text, max - current, max, format, clampAtZero, clampAtMax);
        }

        public void Display(TMP_Text text, float num, float max, string format, bool clampAtZero, bool clampAtMax)
        {
            float minValue = Mathf.NegativeInfinity;
            float maxValue = Mathf.Infinity;

            if (clampAtZero)
                minValue = 0;

            if (clampAtMax)
                maxValue = max;

            float finalValue = Mathf.Clamp(num, minValue, maxValue);

            text.text = finalValue.ToString(format);
        }
    }
}
