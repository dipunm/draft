namespace Shopomo.Web.Models
{
    public struct DisplayableValue
    {
        public object Value { get; }
        public string DisplayName { get; }

        public DisplayableValue(object value) : this(value, value.ToString())
        { }
        public DisplayableValue(object value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}