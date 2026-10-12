namespace Test.Models
{
    public class Variant
    {
        public string Attribute { get; set; } // e.g., "Size"
        public string Value { get; set; }     // e.g., "M"
        public bool isSelected { get; set; }
        public Variant(string attribute, string value, bool isselected)
        {
            Attribute = attribute;
            Value = value;
            isSelected = isselected;
        }
    }
}
