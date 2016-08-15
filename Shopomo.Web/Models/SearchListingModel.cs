using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shopomo.ProductSearcher;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.Projections;
using Shopomo.ProductSearcher.Domain.SearchMetas;

namespace Shopomo.Web.Models
{
    public class SearchListingModel
    {
        public SearchListingModel(ISearchResult<ProductSummary> result)
        {
            Products = result.Products;
            BrandFilters = result.Get<RelatedBrands, string[]>().Select(b => new DisplayableValue(b));
            DepartmentFilters = result.Get<Departments, string[]>().Select(d => new DisplayableValue(d));
            RetailerFilters = result.Get<RelatedRetailers, string[]>().Select(r => new DisplayableValue(r));
            DidYouMean = result.Get<SpellingSuggestion, string>();
            Total = result.Total;
        }

        public string DidYouMean { get; set; }

        public IEnumerable<ProductSummary> Products { get; set; }

        public IEnumerable<DisplayableValue> DepartmentFilters { get; set; }

        public IEnumerable<DisplayableValue> BrandFilters { get; set; }

        public IEnumerable<DisplayableValue> RetailerFilters { get; set; }

        public int Total { get; set; }

        public ContextFilter Context { get; set; }
    }

    public class ContextFilter
    {
        public string FilterType { get; set; }
        public DisplayableValue FilterValue { get; set; }
    }
}