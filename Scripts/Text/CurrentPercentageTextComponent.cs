namespace JacobHomanics.TrickedOutUI
{
    public class CurrentPercentageTextComponent : BaseCurrentMaxTextComponent
    {
        public TextProperties properties;
        public bool encloseInBraces;

        void Update()
        {
            float num = Current;
            float max = Max;

            // Calculate percentage using original value (matching original behavior)
            float percentageValue = (num / max) * 100f;
            float processedValue = ProcessValue(percentageValue, max, properties);

            var finalValueStringRaw = processedValue.ToString(properties.format);
            var finalString = encloseInBraces ? "(" + finalValueStringRaw + "%)" : finalValueStringRaw + "%";
            properties.text.text = finalString;
        }
    }
}

