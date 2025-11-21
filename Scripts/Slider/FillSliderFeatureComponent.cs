using UnityEngine.UI;

namespace JacobHomanics.TrickedOutUI
{
    public class FillSliderFeatureComponent : BaseCurrentMaxComponent
    {
        public Slider slider;
        void Update()
        {
            slider.value = X;
            slider.maxValue = Y;
        }
    }
}