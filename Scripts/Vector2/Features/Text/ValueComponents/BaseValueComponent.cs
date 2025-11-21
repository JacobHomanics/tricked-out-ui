namespace JacobHomanics.TrickedOutUI
{
    public abstract class BaseValueComponent : BaseVector2Component
    {
        public abstract float GetValue(float current, float max);
    }
}

