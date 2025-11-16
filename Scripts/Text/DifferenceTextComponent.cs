namespace JacobHomanics.TrickedOutUI
{
    public class DifferenceTextComponent : BaseCurrentMaxTextComponent
    {
        public TextProperties properties;

        void Update()
        {
            float num = Max - Current;
            float processedValue = ProcessValue(num, Max, properties);
            SetText(processedValue, properties);
        }
    }
}

