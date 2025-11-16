using UnityEngine;
using TMPro;

namespace JacobHomanics.TrickedOutUI
{
    [System.Serializable]
    public class TextProperties
    {
        public TMP_Text text;
        public string format = "#,##0";

        public bool clampAtMax;
        public bool clampAtZero;
        public bool ceil;

        public bool floor;
    }

    public abstract class BaseCurrentMaxTextComponent : BaseCurrentMaxComponent
    {
        protected float ProcessValue(float value, float max, TextProperties properties)
        {
            float minValue = Mathf.NegativeInfinity;
            float maxValue = Mathf.Infinity;

            if (properties.clampAtZero)
                minValue = 0;

            if (properties.clampAtMax)
                maxValue = max;

            float finalValue = Mathf.Clamp(value, minValue, maxValue);

            if (properties.ceil)
            {
                finalValue = Mathf.Ceil(finalValue);
            }

            if (properties.floor)
            {
                finalValue = Mathf.Floor(finalValue);
            }

            return finalValue;
        }

        protected void SetText(float finalValue, TextProperties properties)
        {
            var finalValueStringRaw = finalValue.ToString(properties.format);
            properties.text.text = finalValueStringRaw;
        }
    }
}
