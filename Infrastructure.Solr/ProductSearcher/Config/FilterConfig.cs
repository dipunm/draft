using System.Collections.Generic;
using System.Linq;
using Shopomo.ProductSearcher.Domain.Search;
using SolrNet;

namespace Infrastructure.Solr.ProductSearcher.Config
{
    public static class FilterConfig
    {
        public static IEnumerable<ISolrQuery> ToFilterQueries(SearchFilters filters)
        {
            if (!string.IsNullOrEmpty(filters.Department))
                yield return new SolrQueryByField(SolrFields.DepartmentFilter, filters.Department);

            if (filters.PriceRange != null)
                yield return new SolrQueryByRange<decimal?>(SolrFields.PriceFilter, filters.PriceRange.Min,
                     filters.PriceRange.Max);

            if (filters.Retailers.Any())
                yield return new SolrQueryInList(SolrFields.RetailerFilter, filters.Retailers);

            if (filters.Brands.Any())
                yield return new SolrQueryInList(SolrFields.BrandFilter, filters.Brands);

            if (filters.WithFreeDelivery.HasValue)
                yield return new SolrQueryByField(SolrFields.DeliveryFilter,
                        filters.WithFreeDelivery.Value.ToString().ToLowerInvariant());

            if (filters.Sale != null)
                yield return new SolrQueryByRange<decimal?>(SolrFields.DiscountFilter, filters.Sale, null);

        }
    }
}