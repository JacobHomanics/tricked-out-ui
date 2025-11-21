using UnityEngine;

namespace JacobHomanics.TrickedOutUI
{
    public abstract class BaseCurrentMaxComponent : MonoBehaviour
    {
        public BaseCurrentMaxConnector currentMaxComponent;

        public float Current => currentMaxComponent.CurrentNum;
        public float Max => currentMaxComponent.MaxNum;
    }
}
