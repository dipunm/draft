using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Solr.ProductSearcher.Client;
using Infrastructure.Solr.ProductSearcher.Config;
using Moq;
using NUnit.Framework;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using Shopomo.ProductSearcher.Domain.Search;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.Tests.SolrProductSearcherTests
{
    [TestFixture]
    public partial class SolrProductSearcherTests
    {
        [SetUp]
        public void Setup()
        {
            Startup.Container.Clear();

            _solrClient = new Mock<ISolrReadOnlyOperations<DocumentModel>>();
            Startup.Container.Register(slocator => _solrClient.Object);

            _searcher = new SolrProductSearcher();
        }

        private SolrProductSearcher _searcher;
        private Mock<ISolrReadOnlyOperations<DocumentModel>> _solrClient;


        private static readonly object[] OrderCases =
        {
            new object[] {Sort.RandomOrder, new[] {new SortOrder(SolrFields.RandomOrder, Order.ASC)}},
            new object[]
            {
                Sort.PriorityThenRandom,
                new[]
                {new SortOrder(SolrFields.PriorityOrder, Order.DESC), new SortOrder(SolrFields.RandomOrder, Order.ASC)}
            },
            new object[] {Sort.PriceAsc, new[] {new SortOrder(SolrFields.PriceOrder, Order.ASC)}},
            new object[] {Sort.PriceDesc, new[] {new SortOrder(SolrFields.PriceOrder, Order.DESC)}}
        };

        [TestCaseSource(nameof(OrderCases))]
        public async Task ProductSearcher_GivenSort_ShouldSpecifySortInSolrQuery(Sort sort, SortOrder[] solrSort)
        {
            var search = new SearchModel
            {
                Order = sort
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.OrderBy.SequenceEqual(solrSort))));
        }

        [Test]
        public async Task ProductSearcher_GivenAQuery_ShouldSendItToSolr()
        {
            var search = new SearchModel
            {
                Query = "hello"
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.Is<SolrQuery>(q => q.Query == "hello"), It.IsAny<QueryOptions>()));
        }

        [Test]
        public async Task ProductSearcher_GivenNoQuery_ShouldSendWildcardQueryToSolr()
        {
            var search = new SearchModel
            {
                Query = null
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.Is<SolrQuery>(q => q.Query == "*:*"), It.IsAny<QueryOptions>()));
        }

        [Test]
        public async Task ProductSearcher_GivenPageInfo_ShouldLimitResultsByPage()
        {
            var search = new SearchModel();
            search.Page.Change(20, 10);

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Rows == 10 && o.Start == 20)));
        }

        [Test]
        public async Task ProductSearcher_GivenRelevanceOrder_ShouldNotSpecifySort()
        {
            var search = new SearchModel
            {
                Order = Sort.Relevance
            };

            await _searcher.SearchAsync(search, new IAdditionalData<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.OrderBy.Any() == false)));
        }
    }
}