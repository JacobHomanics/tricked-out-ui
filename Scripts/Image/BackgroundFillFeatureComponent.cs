using UnityEngine;
using UnityEngine.UI;

namespace JacobHomanics.TrickedOutUI
{
    /// <summary>
    /// MonoBehaviour component that handles animated background fill based on value changes.
    /// </summary>
    public class BackgroundFillFeatureComponent : BaseCurrentMaxComponent
    {
        [System.Serializable]
        public class BackgroundFillFeature
        {
            public Image backgroundFill;
            public bool keepSizeConsistent = true;
            public float animationSpeed = 10;
            public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0f, 0.3f, 1f, 16f);
            public float delay = 1f;
        }

        public BackgroundFillFeature backgroundFillFeature;

        private float previousValue;

        private bool isAnimating = false;
        private float animationFromValue;
        private float animationToValue;
        private float animationElapsed;
        private float animationDuration;
        private float animationDelayRemaining;

        void Update()
        {
            HandleValueChange(Current, backgroundFillFeature, ref previousValue, Max);
            var fa = backgroundFillFeature.backgroundFill.fillAmount;
            backgroundFillFeature.backgroundFill.fillAmount = UpdateBackgroundFillAnimation(ref fa, Max);
            backgroundFillFeature.backgroundFill.fillAmount = fa;
        }

        public void HandleValueChange(float newValue, BackgroundFillFeature bgFeature, ref float previousValue, float max)
        {
            if (Mathf.Abs(newValue - previousValue) < 0.001f)
                return;

            if (bgFeature == null || bgFeature.backgroundFill == null)
            {
                previousValue = newValue;
                return;
            }


            // Get the current background fill value
            float currentFillValue = GetBackgroundFillValue(bgFeature, max);

            // Check if background fill needs initialization (is at or near 0, indicating uninitialized)
            // Only initialize if it's truly uninitialized, not just different
            if (currentFillValue < 0.01f * max)
            {
                // Background fill appears uninitialized, initialize it to previousValue
                bgFeature.backgroundFill.fillAmount = Normalize(previousValue, max);
                currentFillValue = previousValue;
            }

            // Get the starting value based on whether we want to keep size consistent
            float startValue;
            if (bgFeature.keepSizeConsistent)
            {
                // Use current background fill position (continues from where it is)
                startValue = currentFillValue;
            }
            else
            {
                // Reset to previous value (starts from previous slider value)
                startValue = previousValue;
                bgFeature.backgroundFill.fillAmount = Normalize(previousValue, max);
            }

            // If new value is greater than start position, immediately snap to it
            if (newValue > startValue)
            {
                // Stop any ongoing animation
                isAnimating = false;
                // Immediately set to new value
                bgFeature.backgroundFill.fillAmount = Normalize(newValue, max);
            }
            else
            {
                // HP goes down or stays same - animate from start position
                // Set up animation state
                var fa = bgFeature.backgroundFill.fillAmount;
                StartBackgroundFillAnimation(startValue, newValue, max, bgFeature.delay, bgFeature.speedCurve, bgFeature.animationSpeed, ref fa);
                bgFeature.backgroundFill.fillAmount = fa;
            }

            previousValue = newValue;
        }
        public void StartBackgroundFillAnimation(float fromValue, float toValue, float max, float delay, AnimationCurve curve, float speed, ref float fillAmount)
        {
            float valueDifference = Mathf.Abs(fromValue - toValue);
            if (valueDifference < 0.001f)
            {
                fillAmount = Normalize(toValue, max);
                isAnimating = false;
                return;
            }

            // Initialize animation state
            isAnimating = true;
            animationFromValue = fromValue;
            animationToValue = toValue;
            animationElapsed = 0f;
            animationDelayRemaining = delay;

            // Calculate dynamic animation speed based on difference
            float normalizedDifference = valueDifference / max;
            float speedMultiplier = curve.Evaluate(normalizedDifference);
            float dynamicSpeed = speed * speedMultiplier * Time.deltaTime;
            animationDuration = valueDifference / dynamicSpeed;
        }

        public float UpdateBackgroundFillAnimation(ref float fillAmount, float max)
        {
            if (!isAnimating)
                return fillAmount;

            // Handle delay before animation starts
            if (animationDelayRemaining > 0f)
            {
                animationDelayRemaining -= Time.deltaTime;
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

        // public static void SetBackgroundFillAmount(BackgroundFillFeature bgFeature, float amount, float max)
        // {
        //     if (bgFeature != null && bgFeature.backgroundFill != null)
        //     {
        //         bgFeature.backgroundFill.fillAmount = SetBackgroundFillAmount(amount, max);
        //     }
        // }

        public float Normalize(float amount, float max)
        {
            return amount / max;
        }

        public static float GetBackgroundFillValue(BackgroundFillFeature bgFeature, float max)
        {
            return bgFeature.backgroundFill.fillAmount * max;
        }
    }
}

