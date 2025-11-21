using UnityEngine;
using UnityEngine.UI;

namespace JacobHomanics.TrickedOutUI
{
    /// <summary>
    /// MonoBehaviour component that handles animated background fill based on value changes.
    /// </summary>
    public class AnimatedImageFill : AnimatedFill
    {
        public Image image;


        void Start()
        {
            image.fillAmount = Current / Max;
        }

        void Update()
        {
            image.fillAmount = HandleValueChange(Current, image.fillAmount, ref previousValue, Max, properties.delay, properties.animationDuration);
            image.fillAmount = UpdateAnimation(image.fillAmount, Max);
        }

        public void Reset()
        {
            if (!image)
                image = GetComponent<Image>();
        }
    }
}

