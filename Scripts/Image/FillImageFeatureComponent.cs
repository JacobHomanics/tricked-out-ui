using UnityEngine;
using UnityEngine.UI;

namespace JacobHomanics.TrickedOutUI
{
    public class FillImageFeatureComponent : BaseCurrentMaxComponent
    {
        [SerializeField] private Image image;

        public Image Image
        {
            get => image;
        }

        void Update()
        {
            UpdateImage(Image, Current, Max);
        }

        private void UpdateImage(Image image, float current, float max)
        {
            image.fillAmount = UpdateValue(current, max);
        }

        private float UpdateValue(float current, float max)
        {
            return current / max;
        }
    }

}
