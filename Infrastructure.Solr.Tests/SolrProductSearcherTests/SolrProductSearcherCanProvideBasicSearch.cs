using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shopomo.ProductSearcher.Domain;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.Tests.SolrProductSearcherTests
{
    [TestFixture]
    public partial class SolrProductSearcherTests
    {
        private Solr.SolrProductSearcher _searcher;
        private Mock<ISolrReadOnlyOperations<Product>> _solrClient;


        [SetUp]
        public void Setup()
        {
            Startup.Container.Clear();

            _solrClient = new Mock<ISolrReadOnlyOperations<Product>>();
            Startup.Container.Register(slocator => _solrClient.Object);

            _searcher = new Solr.SolrProductSearcher();
        }

        [Test]
        public async Task ProductSearcher_GivenAQuery_ShouldSendItToSolr()
        {
            var search = new SearchModel()
            {
                Query = "hello"
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.Is<SolrQuery>(q => q.Query == "hello"), It.IsAny<QueryOptions>()));
        }

        [Test]
        public async Task ProductSearcher_GivenNoQuery_ShouldSendWildcardQueryToSolr()
        {
            var search = new SearchModel()
            {
                Query = null
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.Is<SolrQuery>(q => q.Query == "*:*"), It.IsAny<QueryOptions>()));
        }

        [Test]
        public async Task ProductSearcher_GivenPageInfo_ShouldLimitResultsByPage()
        {
            var search = new SearchModel()
            {
                Page = new PageModel { Start = 20, Size = 10 }
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.Rows == 10 && o.Start == 20)));
        }

        [Test]
        public async Task ProductSearcher_GivenRelevanceOrder_ShouldNotSpecifySort()
        {
            var search = new SearchModel()
            {
                Order = Sort.Relevance
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.OrderBy.Any() == false)));
        }


        private static readonly object[] OrderCases =
        {
            new object[] {Sort.RandomOrder, new [] {new SortOrder("randomorder", Order.ASC) }},
            new object[] {Sort.PriorityThenRandom, new [] {new SortOrder("prioritylisting", Order.DESC), new SortOrder("randomorder", Order.ASC) }},
            new object[] {Sort.PriceAsc, new [] {new SortOrder("price", Order.ASC) }},
            new object[] {Sort.PriceDesc, new [] {new SortOrder("price", Order.DESC) }},
        };
        [TestCaseSource(nameof(OrderCases))]
        public async Task ProductSearcher_GivenSort_ShouldSpecifySortInSolrQuery(Sort sort, SortOrder[] solrSort)
        {
            var search = new SearchModel()
            {
                Order = sort
            };

            await _searcher.SearchAsync(search, new ISearchMeta<object>[0]);

            _solrClient.Verify(c => c.Query(It.IsAny<ISolrQuery>(), It.Is<QueryOptions>(
                o => o.OrderBy.SequenceEqual(solrSort))));
        }
    }

  

  
    public static class TestExtensions
    {
        public static bool HasOne<T>(this ICollection<ISolrQuery> queries, Func<T, bool> predicate) where T : ISolrQuery
        {
            return queries.OfType<T>().SingleOrDefault(predicate) != null;
        }

        public static bool HasOne<T>(this ICollection<ISolrFacetQuery> queries, Func<T, bool> predicate) where T : ISolrFacetQuery
        {
            return queries.OfType<T>().SingleOrDefault(predicate) != null;
        }
    }

}
