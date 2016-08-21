using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using Shopomo.ProductSearcher.Domain.Projections;
using Shopomo.ProductSearcher.Domain.Search;
using SolrNet;

namespace Infrastructure.Solr.ProductSearcher.Client
{
    public class SolrProductSearcher : IProductSearcher
    {
        private readonly SolrQueryBuilder _queryBuilder;
        private readonly ISolrReadOnlyOperations<DocumentModel> _solrClient;

        public SolrProductSearcher()
        {
            _solrClient = ServiceLocator.Current.GetInstance<ISolrReadOnlyOperations<DocumentModel>>();
            _queryBuilder = new SolrQueryBuilder();
        }

        public Task<ISearchResult<ProductSummary>> SearchAsync(SearchModel search,
            ICollection<IAdditionalData<object>> interests)
        {
            var solrQuery = _queryBuilder.BuildQuery(search);
            var solrOptions = _queryBuilder.BuildOptions(search, interests);

            var solrResult = _solrClient.Query(solrQuery, solrOptions);

            var searchResult = new SolrProductSearchResult(null, solrResult);
            return Task.FromResult<ISearchResult<ProductSummary>>(searchResult);
        }
    }
}