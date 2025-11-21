using UnityEngine;
using UnityEngine.UI;

namespace JacobHomanics.TrickedOutUI
{
    /// <summary>
    /// MonoBehaviour component that handles animated background fill based on value changes.
    /// </summary>
    public abstract class BackgroundFillFeatureComponent : BaseCurrentMaxComponent
    {
        [System.Serializable]
        public class BackgroundFillFeature
        {
            public bool keepSizeConsistent = true;
            [Tooltip("Duration in seconds for the animation to complete")]
            public float animationDuration = 1f;
            public float delay = 0.5f;
        }

        public BackgroundFillFeature backgroundFillFeature;

        protected float previousValue;

        protected bool isAnimating = false;
        protected float animationFromValue;
        protected float animationToValue;
        protected float animationElapsed;
        protected float animationDuration;
        protected float animationDelayRemaining;

        public float HandleValueChange(float newValue, float fillAmount, bool keepSizeConsistent, ref float previousValue, float max, float delay, float duration)
        {
            if (Mathf.Abs(newValue - previousValue) < 0.001f)
                return fillAmount;

            // Get the current background fill value
            float currentFillValue = GetFillValue(fillAmount, max);

            // Check if background fill needs initialization (is at or near 0, indicating uninitialized)
            // Only initialize if it's truly uninitialized, not just different
            if (currentFillValue < 0.01f * max)
            {
                // Background fill appears uninitialized, initialize it to previousValue
                fillAmount = Normalize(previousValue, max);
                currentFillValue = previousValue;
            }

            // Get the starting value based on whether we want to keep size consistent
            float startValue;
            if (keepSizeConsistent)
            {
                // Use current background fill position (continues from where it is)
                startValue = currentFillValue;
            }
            else
            {
                // Reset to previous value (starts from previous slider value)
                startValue = previousValue;
                fillAmount = Normalize(previousValue, max);
            }

            // Set up animation state
            var fa = fillAmount;
            StartAnimation(startValue, newValue, max, delay, duration, ref fa);
            fillAmount = fa;

            previousValue = newValue;
            return fillAmount;
        }
        public void StartAnimation(float fromValue, float toValue, float max, float delay, float duration, ref float fillAmount)
        {
            float valueDifference = Mathf.Abs(fromValue - toValue);
            if (valueDifference < 0.001f)
            {
                fillAmount = Normalize(toValue, max);
                isAnimating = false;
                return;
            }

            // Initialize animation state
            // Even if duration is 0, we still need to set up animation to handle delay
            isAnimating = true;
            animationFromValue = fromValue;
            animationToValue = toValue;
            animationElapsed = 0f;
            animationDelayRemaining = delay;
            animationDuration = Mathf.Max(0f, duration); // Ensure non-negative
        }

        public float UpdateAnimation(float fillAmount, float max)
        {
            if (!isAnimating)
                return fillAmount;

            // Handle delay before animation starts
            if (animationDelayRemaining > 0f)
            {
                animationDelayRemaining -= Time.deltaTime;
                return fillAmount;
            }

            // If duration is 0, complete animation immediately after delay
            if (animationDuration <= 0f)
            {
                fillAmount = Normalize(animationToValue, max);
                isAnimating = false;
                return fillAmount;
            }

            // Update animation
            animationElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(animationElapsed / animationDuration);
            float currentValue = Mathf.Lerp(animationFromValue, animationToValue, t);
            fillAmount = Normalize(currentValue, max);


            // Check if animation is complete
            if (animationElapsed >= animationDuration)
            {
                fillAmount = Normalize(animationToValue, max);
                isAnimating = false;
            }

            return fillAmount;
        }

        public float Normalize(float amount, float max)
        {
            return amount / max;
        }

        public static float GetFillValue(float fillAmount, float max)
        {
            return fillAmount * max;
        }
    }
}

