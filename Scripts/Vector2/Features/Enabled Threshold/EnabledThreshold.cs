using UnityEngine;

namespace JacobHomanics.TrickedOutUI
{
    /// <summary>
    /// MonoBehaviour component that creates a flashing effect when a value falls below a threshold.
    /// </summary>
    public class EnabledThreshold : BaseEnabledThreshold
    {
        public MonoBehaviour monoBehaviour;

        void Update()
        {
            monoBehaviour.enabled = IsEnabled();
        }
    }
}

