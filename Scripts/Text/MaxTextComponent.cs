namespace JacobHomanics.TrickedOutUI
{
    public class MaxTextComponent : BaseCurrentMaxTextComponent
    {
        public TextProperties properties;

        void Update()
        {
            float num = Max;
            float processedValue = ProcessValue(num, Max, properties);
            SetText(processedValue, properties);
        }
    }
}

