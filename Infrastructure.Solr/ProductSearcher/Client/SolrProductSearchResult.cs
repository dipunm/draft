using System;
using System.Collections.Generic;
using System.Linq;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using Shopomo.ProductSearcher.Domain.Projections;
using SolrNet;

namespace Infrastructure.Solr.ProductSearcher.Client
{
    public class SolrProductSearchResult : ISearchResult<ProductSummary>
    {
        private readonly SolrQueryResults<DocumentModel> _solrResult;

        public SolrProductSearchResult(IEnumerable<object> handlers, SolrQueryResults<DocumentModel> solrResult)
        {
            _solrResult = solrResult;
        }

        public IEnumerable<ProductSummary> Products => _solrResult.Select(p => new ProductSummary());
        public int Total => _solrResult.NumFound;

        public TOut Get<TRequest, TOut>() where TRequest : IAdditionalData<TOut>
        {
            throw new NotImplementedException();
        }
    }
}