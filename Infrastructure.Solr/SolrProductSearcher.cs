using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.Projections;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr
{
    public static class SolrBootstrapper
    {
        public static void InitProductSearcher(Uri solrUrl)
        {
            Startup.Init<Product>(new UriBuilder(solrUrl) { Path = "/solr/products"}.Uri.AbsoluteUri);
        }
    }

    internal static class QueryHelpers
    {
        public static ICollection<SortOrder> ToSortOrders(this Sort sort)
        {
            switch (sort)
            {
                case Sort.PriceAsc:
                    return new[] { new SortOrder("price", Order.ASC) };
                case Sort.PriceDesc:
                    return new[] { new SortOrder("price", Order.DESC) };
                default:
                    return new SortOrder[0];
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Concat(new[] { item });
        }
    }

    public class Product
    {

    }

    public class SolrProductSearcher : IProductSearcher
    {
        private readonly ISolrFilterBuilder _solrFilterBuilder;
        private readonly ISolrReadOnlyOperations<Product> _solrClient;
        public SolrProductSearcher(ISolrFilterBuilder solrFilterBuilder)
        {
            _solrFilterBuilder = solrFilterBuilder;
            _solrClient = ServiceLocator.Current.GetInstance<ISolrReadOnlyOperations<Product>>();
        }

        public Task<ISearchResult<ProductSummary>> SearchAsync(SearchModel query, IEnumerable<ISearchMeta<object>> interests)
        {
            var solrQuery = string.IsNullOrEmpty(query.Query) ? SolrQuery.All : new SolrQuery(query.Query);
            var searchOptions = new QueryOptions()
            {
                Start = query.Page.Start,
                Rows = query.Page.Size,
                OrderBy = CalculateOrder(query),
                //                Fields = new []{"","","","","","","",""},
                FilterQueries = _solrFilterBuilder.CreateFilterQueries(query.Filters),
                //                ExtraParams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("qt", "/get")}
            };

            var metaQueryHandler = new MetaQueryHandler(null, interests);
            metaQueryHandler.AppendMetaToSearch(searchOptions);

            var solrResult = _solrClient.Query(solrQuery, searchOptions);

            ISearchResult<ProductSummary> searchResult = new ProductSearchResult(null, solrResult);
            return Task.FromResult(searchResult);
        }

        private ICollection<SortOrder> CalculateOrder(SearchModel query)
        {
            var userSorts = query.Order.ToSortOrders();
            if (userSorts.Any())
                return userSorts.ToArray();

            if (string.IsNullOrEmpty(query.Query))
            {
                var sorts = new List<SortOrder>();
                if (!string.IsNullOrEmpty(query.Filters.Department))
                    sorts.Add(new SortOrder("listingpriority", Order.DESC));

                sorts.Add(new SortOrder("randomorder"));
                return sorts;
            }
            return new SortOrder[0];
        }

        public interface ISolrFilterBuilder
        {
            ICollection<ISolrQuery> CreateFilterQueries(SearchFilters filters);
        }

        public class SolrFilterBuilder : ISolrFilterBuilder
        {
            public ICollection<ISolrQuery> CreateFilterQueries(SearchFilters filters)
            {
                var filterQueries = Enumerable.Empty<ISolrQuery>();

                //delivery
                //sale
                //retailer
                //brand
                //price


                return filterQueries.ToArray();

            }
        }
    }

    public class ProductSearchResult : ISearchResult<ProductSummary>
    {
        private readonly IEnumerable<object> _handlers;
        private readonly SolrQueryResults<Product> _solrResult;

        public ProductSearchResult(IEnumerable<object> handlers, SolrQueryResults<Product> solrResult)
        {
            _handlers = handlers;
            _solrResult = solrResult;
        }

        public IEnumerable<ProductSummary> Products => _solrResult.Select(p => new ProductSummary() { });
        public int Total => _solrResult.NumFound;
        public TOut Get<TRequest, TOut>() where TRequest : ISearchMeta<TOut>
        {
            var handler =  _handlers.OfType<ISearchMetaHandler<TRequest, TOut>>().SingleOrDefault();
            if (handler == null)
                throw new InvalidOperationException($"Could not find handler for SearchMeta '{typeof(TRequest).Name}'");

            return handler.Read(_solrResult);
        }
    }

    public interface ISearchMetaHandler<in TRequest, out TOut>
    {
        TOut Read(SolrQueryResults<Product> result);
        void Write(TRequest request, QueryOptions options);
    }

    public class MetaQueryHandler
    {
        public MetaQueryHandler(IEnumerable<object> handlers, IEnumerable<ISearchMeta<object>> interests)
        {

        }

        public void AppendMetaToSearch(QueryOptions searchOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}
