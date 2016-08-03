using System.Collections.Generic;

namespace Shopomo.Web.Models
{
    public class SearchModel
    {
        public string Query { get; set; }
        public SearchFilters Filters { get; set; }
        public Sort Order { get; set; }
    }

    public class SearchFilters
    {
        public PriceRange PriceRange { get; set; }
        public string Department { get; set; }
        public IEnumerable<string> Brands { get; set; }
        public IEnumerable<string> Retailers { get; set; }
        public bool? OnSale { get; set; }
        public bool? WithFreeDelivery { get; set; }
    }

    public class PriceRange
    {
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
    }

    public enum Sort
    {
        Relevance = 0, PriceAsc, PriceDesc
    }
}