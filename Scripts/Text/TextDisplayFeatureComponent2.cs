using UnityEngine;
using TMPro;

namespace JacobHomanics.UI
{
    public class TextDisplayFeatureComponent2 : BaseCurrentMaxTextComponent
    {
        public TextProperties leftProperties;
        public TextProperties rightProperties;


        void Update()
        {
            Display(leftProperties.text, leftProperties.displayType, Current, Max, leftProperties.format, leftProperties.clampAtZero, leftProperties.clampAtMax);
            Display(rightProperties.text, rightProperties.displayType, Current, Max, rightProperties.format, rightProperties.clampAtZero, rightProperties.clampAtMax);
        }

    }
}
