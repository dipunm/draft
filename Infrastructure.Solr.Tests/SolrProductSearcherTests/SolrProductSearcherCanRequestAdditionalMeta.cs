using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.SearchMetas;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.Tests.SolrProductSearcherTests
{
    public partial class SolrProductSearcherTests
    {
        [Test]
        public async Task ProductSearcher_GivenRelatedBrandsRequest_ShouldRequestBrandFacets()
        {
            var interests = new ISearchMeta<object>[] { new RelatedBrands(5) };

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Facet.Queries.HasOne<SolrFacetFieldQuery>(q =>
                    q.Field == "brand" &&
                    q.Limit == 5))));
        }

        [Test]
        public async Task ProductSearcher_GivenRelatedRetailersRequest_ShouldRequestRetailerFacets()
        {
            var interests = new ISearchMeta<object>[] { new RelatedRetailers(5), };

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Facet.Queries.HasOne<SolrFacetFieldQuery>(q =>
                    q.Field == "retailer" &&
                    q.Limit == 5))));
        }

        [Test]
        public async Task ProductSearcher_GivenDepartmentsRequest_ShouldRequestDepartmentPathFacet()
        {
            var interests = new ISearchMeta<object>[] { new Departments(10) };

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Facet.Queries.HasOne<SolrFacetFieldQuery>(q =>
                    q.Field == "departmentpath" &&
                    q.Limit == 10))));
        }

        [Test]
        public async Task ProductSearcher_GivenSpellingRequest_ShouldRequestSpellingSuggestions()
        {
            var interests = new ISearchMeta<object>[] { new SpellingSuggestion() };

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.SpellCheck.Collate == true)));
        }

        [Test]
        public async Task ProductSearcher_GivenMaxSaleRequest_ShouldRequestStatsForDiscountPercentageField()
        {
            var interests = new ISearchMeta<object>[] { new MaxAvailableSale() };

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Stats.FieldsWithFacets.ContainsKey("discountpercentage"))));
        }
        
    }

}
