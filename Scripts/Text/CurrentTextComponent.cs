namespace JacobHomanics.TrickedOutUI
{
    public class CurrentTextComponent : BaseCurrentMaxTextComponent
    {
        public TextProperties properties;

        void Update()
        {
            float num = Current;
            float processedValue = ProcessValue(num, Max, properties);
            SetText(processedValue, properties);
        }
    }
}

