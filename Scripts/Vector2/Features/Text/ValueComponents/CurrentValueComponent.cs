using UnityEngine;

namespace JacobHomanics.TrickedOutUI
{
    public class CurrentValueComponent : BaseValueComponent
    {
        public override float GetValue(Vector2 vector)
        {
            return vector.x;
        }
    }
}

