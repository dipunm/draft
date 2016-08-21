using System.Threading.Tasks;
using Infrastructure.Solr.ProductSearcher.Config;
using Infrastructure.Solr.Tests.Helpers;
using Moq;
using NUnit.Framework;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using Shopomo.ProductSearcher.Domain.Search;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.Tests.SolrProductSearcherTests
{
    public partial class SolrProductSearcherTests
    {
        [Test]
        public async Task ProductSearcher_GivenDepartmentsRequest_ShouldRequestDepartmentPathFacet()
        {
            var interests = new IAdditionalData<object>[] {new Departments(10)};

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Facet.Queries.HasOne<SolrFacetFieldQuery>(q =>
                    q.Field == SolrFields.DepartmentFacet &&
                    q.Limit == 10))));
        }

        [Test]
        public async Task ProductSearcher_GivenMaxSaleRequest_ShouldRequestStatsForDiscountPercentageField()
        {
            var interests = new IAdditionalData<object>[] {new MaxAvailableSale()};

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Stats.FieldsWithFacets.ContainsKey(SolrFields.DiscountMax))));
        }

        [Test]
        public async Task ProductSearcher_GivenRelatedBrandsRequest_ShouldRequestBrandFacets()
        {
            var interests = new IAdditionalData<object>[] {new RelatedBrands(5)};

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Facet.Queries.HasOne<SolrFacetFieldQuery>(q =>
                    q.Field == SolrFields.BrandFacet &&
                    q.Limit == 5))));
        }

        [Test]
        public async Task ProductSearcher_GivenRelatedRetailersRequest_ShouldRequestRetailerFacets()
        {
            var interests = new IAdditionalData<object>[] {new RelatedRetailers(5)};

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Facet.Queries.HasOne<SolrFacetFieldQuery>(q =>
                    q.Field == SolrFields.RetailerFacet &&
                    q.Limit == 5))));
        }

        [Test]
        public async Task ProductSearcher_GivenSpellingRequest_ShouldRequestSpellingSuggestions()
        {
            var interests = new IAdditionalData<object>[] {new SpellingSuggestion()};

            await _searcher.SearchAsync(new SearchModel(), interests);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.SpellCheck.Collate == true)));
        }
    }
}