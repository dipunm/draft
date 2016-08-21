using System.Collections.Generic;
using System.Linq;
using Shopomo.ProductSearcher.Domain.Search;
using SolrNet;

namespace Infrastructure.Solr.ProductSearcher.Config
{
    public static class FilterConfig
    {
        public static ICollection<ISolrQuery> ToFilterQueries(SearchFilters filters)
        {
            var filterQueries = new List<ISolrQuery>();
            if (!string.IsNullOrEmpty(filters.Department))
                filterQueries.Add(new SolrQueryByField(SolrFields.DepartmentFilter, filters.Department));
            if (filters.PriceRange != null)
                filterQueries.Add(new SolrQueryByRange<decimal?>(SolrFields.PriceFilter, filters.PriceRange.Min,
                    filters.PriceRange.Max));
            if (filters.Retailers.Any())
                filterQueries.Add(new SolrQueryInList(SolrFields.RetailerFilter, filters.Retailers));
            if (filters.Brands.Any())
                filterQueries.Add(new SolrQueryInList(SolrFields.BrandFilter, filters.Brands));
            if (filters.WithFreeDelivery.HasValue)
                filterQueries.Add(new SolrQueryByField(SolrFields.DeliveryFilter,
                    filters.WithFreeDelivery.Value.ToString().ToLowerInvariant()));
            if (filters.Sale != null)
                filterQueries.Add(new SolrQueryByRange<decimal?>(SolrFields.DiscountFilter, filters.Sale, null));
            return filterQueries;
        }
    }
}