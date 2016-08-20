using System.Collections.Generic;

namespace Shopomo.ProductSearcher.Domain
{
    public class SearchModel
    {
        public SearchModel()
        {
            Filters = new SearchFilters();
            Page = new PageModel();
        }
        public string Query { get; set; }
        public SearchFilters Filters { get; set; }
        public Sort Order { get; set; }
        public PageModel Page { get; set; }
    }

    public class PageModel
    {
        public int Start { get; set; }
        public int Size { get; set; }
    }

    public class SearchFilters
    {
        public SearchFilters()
        {
            PriceRange = new PriceRange();
        }
        public PriceRange PriceRange { get; set; }
        public string Department { get; set; }
        public IEnumerable<string> Brands { get; set; } = new List<string>();
        public IEnumerable<string> Retailers { get; set; } = new List<string>();
        public SaleOption Sale { get; set; }
        public bool? WithFreeDelivery { get; set; }
    }

    public class PriceRange
    {
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
    }

    public class SaleOption
    {
        private SaleOption(int value, string displayText)
        {
            _value = value;
            DisplayText = displayText;
        }

        private readonly decimal _value;
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

    public enum Sort
    {
        Relevance = 0, PriceAsc, PriceDesc,
        RandomOrder, PriorityThenRandom
    }
}