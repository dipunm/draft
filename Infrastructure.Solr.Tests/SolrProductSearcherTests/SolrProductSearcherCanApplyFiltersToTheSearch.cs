using System.Linq;
using System.Threading.Tasks;
using Library.Core;
using Moq;
using NUnit.Framework;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.Search;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.Tests.SolrProductSearcherTests
{
    public partial class SolrProductSearcherTests
    {
        #region filter by department

        [Test]
        public async Task ProductSearcher_GivenADepartmentFilter_ShouldContainADepartmentFilterQuery()
        {
            var search = new SearchModel()
            {
                Filters = { Department = "/This/Is/A Department/With An & Symbol" }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByField>(f =>
                    f.Quoted &&
                    f.FieldName == "departmentpath" &&
                    f.FieldValue == "/This/Is/A Department/With An & Symbol"))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoDepartmentFilter_ShouldNotFilterByDepartment()
        {
            var search = new SearchModel()
            {
                Filters = { Department = null }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryByField>(f =>
                    f.FieldName == "departmentpath"))));
        }

        #endregion
        #region filter by price

        [Test]
        public async Task ProductSearcher_GivenPriceFilter_ShouldAddAnInclusiveRangeFilterQuery()
        {
            var search = new SearchModel()
            {
                Filters = { PriceRange = PriceRange.Range(20, 200) }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == "price" &&
                    f.Inclusive &&
                    f.From == 20 &&
                    f.To == 200
                ))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoPriceFilter_ShouldNotFIlterByPrice()
        {
            var search = new SearchModel()
            {
                Filters = { PriceRange = null }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == "price"
                ))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoMinValueInPriceFilter_ShouldQueryWithMinRangeAsZero()
        {
            var search = new SearchModel()
            {
                Filters = { PriceRange = PriceRange.Range(null, 200) }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == "price" &&
                    f.From == null
                ))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoMaxValueInPriceFilter_ShouldQueryWithMaxRangeAsInfinite()
        {
            var search = new SearchModel()
            {
                Filters = { PriceRange = PriceRange.Range(20, null) }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == "price" &&
                    f.To == null
                ))));
        }

        #endregion
        #region filter by brands / retailers

        [Test]
        public async Task ProductSearcher_GivenMultipleBrands_ShouldUseMultipleValueFilter()
        {
            var search = new SearchModel();
            search.Filters.Brands.Reset(new[] {"Nike", "Audio-technica", "B&Q"});

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryInList>(q =>
                    q.FieldName == "brand" &&
                    q.List.SequenceEqual(new[] { "Nike", "Audio-technica", "B&Q" })))));
        }

        [Test]
        public async Task ProductSearcher_GivenMultipleEmptyListOfBrands_ShouldNotFilterByBrand()
        {
            var search = new SearchModel();
            search.Filters.Brands.Clear();

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryInList>(q =>
                    q.FieldName == "brand"))));
        }

        [Test]
        public async Task ProductSearcher_GivenMultipleRetailers_ShouldUseMultipleValueFilter()
        {
            var search = new SearchModel();
            search.Filters.Retailers.Reset(new[] { "Asda Direct", "Amazon", "B&Q" });

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryInList>(q =>
                    q.FieldName == "retailer" &&
                    q.List.SequenceEqual(new[] { "Asda Direct", "Amazon", "B&Q" })))));
        }

        [Test]
        public async Task ProductSearcher_GivenMultipleEmptyListOfRetailers_ShouldNotFilterByBrand()
        {
            var search = new SearchModel();
            search.Filters.Retailers.Clear();

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryInList>(q =>
                    q.FieldName == "brand"))));
        }

        #endregion
        #region filter by freedelivery

        [TestCase(true)]
        [TestCase(false)]
        public async Task ProductSearcher_GivenPreferenceForFreeDelivery_ShouldFilterByPreference(bool freedelivery)
        {
            var search = new SearchModel
            {
                Filters = { WithFreeDelivery = freedelivery }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByField>(f =>
                    f.FieldName == "freedelivery" &&
                    f.FieldValue == (freedelivery ? "true" : "false")))));
        }

        [Test]
        public async Task ProductSearcher_GivenNoPreferenceForFreeDelivery_ShouldNotFilterByFreeDelivery()
        {
            var search = new SearchModel
            {
                Filters = { WithFreeDelivery = null }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryByField>(f =>
                    f.FieldName == "freedelivery"))));
        }

        #endregion
        #region filter by sale

        [TestCase("50off", 50)]
        [TestCase("25off", 25)]
        [TestCase("10off", 10)]
        [TestCase("all", 0)]
        public async Task ProductSearcher_GivenSaleOption_ShouldFilterByDiscountPercentage(string saleOption, int discountValue)
        {
            var search = new SearchModel
            {
                Filters = { Sale = saleOption }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == "discountpercentage" &&
                    f.Inclusive &&
                    f.From == discountValue))));
        }

        [TestCase(null)]
        [TestCase("hello")]
        public async Task ProductSearcher_GivenInvalidSaleOption_ShouldNotFilterByDiscountPercentage(string saleOption)
        {
            var search = new SearchModel
            {
                Filters = { Sale = saleOption }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => !o.FilterQueries.HasOne<SolrQueryByRange<decimal?>>(f =>
                    f.FieldName == "discountpercentage"))));
        }

        #endregion

    }
}
