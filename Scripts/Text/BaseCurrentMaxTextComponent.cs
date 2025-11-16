using UnityEngine;
using TMPro;

namespace JacobHomanics.TrickedOutUI
{
    public class BaseCurrentMaxTextComponent : BaseCurrentMaxComponent
    {
        [SerializeField] private BaseValueComponent _valueComponent;
        [SerializeField] private BaseTextFeatureComponent[] _featureComponents = new BaseTextFeatureComponent[0];
        public TMP_Text text;

        public BaseValueComponent valueComponent
        {
            get => _valueComponent;
            set => _valueComponent = value;
        }

        public BaseTextFeatureComponent[] featureComponents
        {
            get => _featureComponents;
            set => _featureComponents = value;
        }

        protected float ProcessValue(float value, float max)
        {
            float minValue = Mathf.NegativeInfinity;
            float maxValue = Mathf.Infinity;
            float finalValue = value;

            // First pass: collect clamp bounds from clamp components
            foreach (var feature in featureComponents)
            {
                if (feature != null && (feature is ClampAtZeroComponent || feature is ClampAtMaxComponent))
                {
                    feature.ProcessValue(finalValue, max, ref minValue, ref maxValue);
                }
            }

            // Apply clamping if bounds were set
            if (minValue != Mathf.NegativeInfinity || maxValue != Mathf.Infinity)
            {
                finalValue = Mathf.Clamp(finalValue, minValue, maxValue);
            }

            // Second pass: apply other transformations (ceil, floor, etc.)
            foreach (var feature in featureComponents)
            {
                if (feature != null && !(feature is ClampAtZeroComponent) && !(feature is ClampAtMaxComponent))
                {
                    float tempMin = Mathf.NegativeInfinity;
                    float tempMax = Mathf.Infinity;
                    finalValue = feature.ProcessValue(finalValue, max, ref tempMin, ref tempMax);
                }
            }

            return finalValue;
        }

        protected void SetText(float finalValue)
        {
            // Get format from FormatFeatureComponent, or use default
            string format = "#,##0";
            FormatFeatureComponent formatComponent = System.Array.Find(featureComponents, f => f is FormatFeatureComponent) as FormatFeatureComponent;
            if (formatComponent != null)
            {
                format = formatComponent.format;
            }

            var finalValueStringRaw = finalValue.ToString(format);

            // Check if this is a percentage value component and handle formatting
            bool isPercentage = valueComponent is CurrentPercentageValueComponent;
            bool encloseInBraces = System.Array.Exists(featureComponents, f => f is EncloseInBracesComponent);

            string finalString = finalValueStringRaw;
            if (isPercentage)
            {
                finalString = finalValueStringRaw + "%";
            }

            if (encloseInBraces)
            {
                finalString = "(" + finalString + ")";
            }

            if (text != null)
            {
                text.text = finalString;
            }
        }

        void Update()
        {
            if (valueComponent == null || text == null)
                return;

            float value = valueComponent.GetValue(Current, Max);
            float processedValue = ProcessValue(value, Max);
            SetText(processedValue);
        }
    }
}
