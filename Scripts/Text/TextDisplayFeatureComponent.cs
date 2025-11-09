using UnityEngine;
using TMPro;

namespace JacobHomanics.UI
{
    public class TextDisplayFeatureComponent : BaseCurrentMaxTextComponent
    {
        public TextProperties properties;

        void Update()
        {
            Display(properties.text, properties.displayType, Current, Max, properties.format, properties.clampAtZero, properties.clampAtMax);
        }
    }
}
