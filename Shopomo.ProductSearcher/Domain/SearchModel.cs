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
        Relevance = 0, PriceAsc, PriceDesc,
        RandomOrder, PriorityThenRandom
    }
}