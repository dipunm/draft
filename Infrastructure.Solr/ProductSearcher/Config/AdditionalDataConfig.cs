using Infrastructure.Solr.ProductSearcher.AdditionalDataAdapters;
using Shopomo.ProductSearcher.Domain.AdditionalData;

namespace Infrastructure.Solr.ProductSearcher.Config
{
    public static class AdditionalDataConfig
    {
        public static readonly ISolrAdditionalDataProvider[] Providers =
        {
            new FilterOptionsAdapter<Departments>(SolrFields.DepartmentFacet),
            new FilterOptionsAdapter<RelatedBrands>(SolrFields.BrandFacet),
            new FilterOptionsAdapter<RelatedRetailers>(SolrFields.RetailerFacet),
            new SpellingSuggestionAdapter(),
            new MaxFieldAdapter(SolrFields.DiscountMax)
        };
    }
}