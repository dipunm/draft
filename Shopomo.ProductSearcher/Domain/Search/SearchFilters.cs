using System.Collections.Generic;

namespace Shopomo.ProductSearcher.Domain.Search
{
    public class SearchFilters
    {
        public PriceRange PriceRange { get; set; }
        public string Department { get; set; }
        public ICollection<string> Brands { get; } = new List<string>();
        public ICollection<string> Retailers { get; } = new List<string>();
        public SaleOption Sale { get; set; }
        public bool? WithFreeDelivery { get; set; }
    }
}