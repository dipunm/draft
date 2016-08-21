namespace Shopomo.ProductSearcher.Domain.Search
{
    public class SaleOption
    {
        private readonly decimal _value;

        private SaleOption(int value, string displayText)
        {
            _value = value;
            DisplayText = displayText;
        }

        public string DisplayText { get; }

        public static implicit operator decimal?(SaleOption opt)
        {
            return opt?._value;
        }

        public static implicit operator SaleOption(string alias)
        {
            switch (alias)
            {
                case "all":
                    return new SaleOption(0, "All on sale");
                case "10off":
                    return new SaleOption(10, "10% or more");
                case "25off":
                    return new SaleOption(25, "25% or more");
                case "50off":
                    return new SaleOption(50, "50% or more");
                default:
                    return null;
            }
        }
    }
}