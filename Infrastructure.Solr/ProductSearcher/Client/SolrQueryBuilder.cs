using System.Collections.Generic;
using System.Linq;
using Infrastructure.Solr.ProductSearcher.Config;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using Shopomo.ProductSearcher.Domain.Search;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.ProductSearcher.Client
{
    public class SolrQueryBuilder
    {
        public ISolrQuery BuildQuery(SearchModel search)
        {
            return string.IsNullOrEmpty(search.Query) ? SolrQuery.All : new SolrQuery(search.Query);
        }

        public QueryOptions BuildOptions(SearchModel search, ICollection<IAdditionalData<object>> interests)
        {
            var searchOptions = new QueryOptions
            {
                Start = search.Page.Start,
                Rows = search.Page.Size,
                OrderBy = SortConfig.SortQueries[search.Order],
                FilterQueries = FilterConfig.ToFilterQueries(search.Filters).ToArray()
            };

            foreach (var interest in interests)
            {
                AdditionalDataConfig.Providers.First(h => h.CanManage(interest.GetType()))
                    .ApplyRequest(searchOptions, interest);
            }

            return searchOptions;
        }
    }
}