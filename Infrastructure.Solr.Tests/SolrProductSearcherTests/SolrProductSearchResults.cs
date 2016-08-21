using System.Collections.Generic;
using Infrastructure.Solr.ProductSearcher.Client;
using Moq;
using NUnit.Framework;
using SolrNet;

namespace Infrastructure.Solr.Tests.SolrProductSearcherTests
{
    [TestFixture]
    public class SolrProductSearchResultsTests
    {
        private Mock<SolrQueryResults<DocumentModel>> _solrResults;
        private SolrProductSearchResult _searchResult  ;

        [SetUp]
        public void Setup()
        {
            _solrResults = new Mock<SolrQueryResults<DocumentModel>>();
            _searchResult = new SolrProductSearchResult(null, _solrResults.Object);
        }

        [Test]
        public void Works()
        {
            var products = new List<DocumentModel>();
         

        }
    }

}