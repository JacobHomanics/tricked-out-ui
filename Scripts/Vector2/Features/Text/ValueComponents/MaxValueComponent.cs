using UnityEngine;

namespace JacobHomanics.TrickedOutUI
{
    public class MaxValueComponent : BaseValueComponent
    {
        public override float GetValue(Vector2 vector)
        {
            return vector.y;
        }
    }
}

