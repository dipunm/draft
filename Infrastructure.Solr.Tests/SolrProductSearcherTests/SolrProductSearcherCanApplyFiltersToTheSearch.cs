using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Solr.ProductSearcher.Config;
using Infrastructure.Solr.Tests.Helpers;
using Library.Core;
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
        [TestCase(true)]
        [TestCase(false)]
        public async Task ProductSearcher_GivenPreferenceForFreeDelivery_ShouldFilterByPreference(bool freedelivery)
        {
            var search = new SearchModel
            {
                Filters = {WithFreeDelivery = freedelivery}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByField>(f =>
                    f.FieldName == SolrFields.DeliveryFilter &&
                    f.FieldValue == (freedelivery ? "true" : "false")))));
        }

        [TestCase("50off", 50)]
        [TestCase("25off", 25)]
        [TestCase("10off", 10)]
        [TestCase("all", 0)]
        public async Task ProductSearcher_GivenSaleOption_ShouldFilterByDiscountPercentage(string saleOption,
            int discountValue)
        {
            var search = new SearchModel
            {
                Filters = {Sale = saleOption}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == SolrFields.DiscountFilter &&
                    f.Inclusive &&
                    f.From == discountValue))));
        }

        [TestCase(null)]
        [TestCase("hello")]
        public async Task ProductSearcher_GivenInvalidSaleOption_ShouldNotFilterByDiscountPercentage(string saleOption)
        {
            var search = new SearchModel
            {
                Filters = {Sale = saleOption}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == SolrFields.DiscountFilter))));
        }

        [Test]
        public async Task ProductSearcher_GivenADepartmentFilter_ShouldContainADepartmentFilterQuery()
        {
            var search = new SearchModel
            {
                Filters = {Department = "/This/Is/A Department/With An & Symbol"}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByField>(f =>
                    f.Quoted &&
                    f.FieldName == SolrFields.DepartmentFilter &&
                    f.FieldValue == "/This/Is/A Department/With An & Symbol"))));
        }

        [Test]
        public async Task ProductSearcher_GivenMultipleBrands_ShouldUseMultipleValueFilter()
        {
            var search = new SearchModel();
            search.Filters.Brands.Reset(new[] {"Nike", "Audio-technica", "B&Q"});

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryInList>(q =>
                    q.FieldName == SolrFields.BrandFilter &&
                    q.List.SequenceEqual(new[] {"Nike", "Audio-technica", "B&Q"})))));
        }

        [Test]
        public async Task ProductSearcher_GivenMultipleEmptyListOfBrands_ShouldNotFilterByBrand()
        {
            var search = new SearchModel();
            search.Filters.Brands.Clear();

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasNone<SolrQueryInList>(q =>
                    q.FieldName == SolrFields.BrandFilter))));
        }

        [Test]
        public async Task ProductSearcher_GivenMultipleEmptyListOfRetailers_ShouldNotFilterByBrand()
        {
            var search = new SearchModel();
            search.Filters.Retailers.Clear();

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasNone<SolrQueryInList>(q =>
                    q.FieldName == SolrFields.RetailerFilter))));
        }

        [Test]
        public async Task ProductSearcher_GivenMultipleRetailers_ShouldUseMultipleValueFilter()
        {
            var search = new SearchModel();
            search.Filters.Retailers.Reset(new[] {"Asda Direct", "Amazon", "B&Q"});

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryInList>(q =>
                    q.FieldName == SolrFields.RetailerFilter &&
                    q.List.SequenceEqual(new[] {"Asda Direct", "Amazon", "B&Q"})))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoDepartmentFilter_ShouldNotFilterByDepartment()
        {
            var search = new SearchModel
            {
                Filters = {Department = null}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryByField>(f =>
                    f.FieldName == SolrFields.DepartmentFilter))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoMaxValueInPriceFilter_ShouldQueryWithMaxRangeAsInfinite()
        {
            var search = new SearchModel
            {
                Filters = {PriceRange = PriceRange.Range(20, null)}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == SolrFields.PriceFilter &&
                    f.To == null
                    ))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoMinValueInPriceFilter_ShouldQueryWithMinRangeAsZero()
        {
            var search = new SearchModel
            {
                Filters = {PriceRange = PriceRange.Range(null, 200)}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == SolrFields.PriceFilter &&
                    f.From == null
                    ))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoPreferenceForFreeDelivery_ShouldNotFilterByFreeDelivery()
        {
            var search = new SearchModel
            {
                Filters = {WithFreeDelivery = null}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryByField>(f =>
                    f.FieldName == SolrFields.DeliveryFilter))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoPriceFilter_ShouldNotFIlterByPrice()
        {
            var search = new SearchModel
            {
                Filters = {PriceRange = null}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasNone<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == SolrFields.PriceFilter
                    ))));
        }

        [Test]
        public async Task ProductSearcher_GivenPriceFilter_ShouldAddAnInclusiveRangeFilterQuery()
        {
            var search = new SearchModel
            {
                Filters = {PriceRange = PriceRange.Range(20, 200)}
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == SolrFields.PriceFilter &&
                    f.Inclusive &&
                    f.From == 20 &&
                    f.To == 200
                    ))));
        }
    }
}