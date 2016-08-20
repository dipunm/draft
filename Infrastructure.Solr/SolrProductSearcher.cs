using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.Projections;
using Shopomo.ProductSearcher.Domain.SearchMetas;
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
        private readonly ISolrReadOnlyOperations<Product> _solrClient;
        public SolrProductSearcher()
        {
            _solrClient = ServiceLocator.Current.GetInstance<ISolrReadOnlyOperations<Product>>();
        }

        public Task<ISearchResult<ProductSummary>> SearchAsync(SearchModel query, IEnumerable<ISearchMeta<object>> interests)
        {
            var solrQuery = string.IsNullOrEmpty(query.Query) ? SolrQuery.All : new SolrQuery(query.Query);


            var filterQueries = new List<ISolrQuery>();
            if (!string.IsNullOrEmpty(query.Filters.Department))
                filterQueries.Add(new SolrQueryByField("departmentpath", query.Filters.Department));
            if(query.Filters.PriceRange != null)
                filterQueries.Add(new SolrQueryByRange<decimal?>("price", query.Filters.PriceRange.Min, query.Filters.PriceRange.Max));
            if(query.Filters.Retailers.Any())
                filterQueries.Add(new SolrQueryInList("retailer", query.Filters.Retailers));
            if (query.Filters.Brands.Any())
                filterQueries.Add(new SolrQueryInList("brand", query.Filters.Brands));
            if(query.Filters.WithFreeDelivery.HasValue)
                filterQueries.Add(new SolrQueryByField("freedelivery", query.Filters.WithFreeDelivery.Value.ToString().ToLowerInvariant()));
            if(query.Filters.Sale != null)
                filterQueries.Add(new SolrQueryByRange<decimal?>("discountpercentage", query.Filters.Sale, null));


            QueryOptions searchOptions = new QueryOptions()
            {
                Start = query.Page.Start,
                Rows = query.Page.Size,
                OrderBy = CalculateOrder(query),
                FilterQueries = filterQueries
            };

            foreach (var interest in interests.OfType<Departments>())
            {
                searchOptions.Facet.Queries.Add(new SolrFacetFieldQuery("departmentpath") {Limit = interest.Limit});
            }

            foreach (var interest in interests.OfType<RelatedRetailers>())
            {
                searchOptions.Facet.Queries.Add(new SolrFacetFieldQuery("retailer") { Limit = interest.Limit });
            }

            foreach (var interest in interests.OfType<RelatedBrands>())
            {
                searchOptions.Facet.Queries.Add(new SolrFacetFieldQuery("brand") { Limit = interest.Limit });
            }

            if(interests.OfType<SpellingSuggestion>().Any())
            {
                searchOptions.SpellCheck = new SpellCheckingParameters() {Collate = true};
            }

            var solrResult = _solrClient.Query(solrQuery, searchOptions);

            ISearchResult<ProductSummary> searchResult = new ProductSearchResult(null, solrResult);
            return Task.FromResult(searchResult);
        }

        private ICollection<SortOrder> CalculateOrder(SearchModel query)
        {
            switch (query.Order)
            {
                case Sort.PriorityThenRandom:
                    return new[] {new SortOrder("prioritylisting", Order.DESC), new SortOrder("randomorder")};
                case Sort.RandomOrder:
                    return new[] {new SortOrder("randomorder")};
                case Sort.PriceAsc:
                    return new[] { new SortOrder("price", Order.ASC) };
                case Sort.PriceDesc:
                    return new[] { new SortOrder("price", Order.DESC) };
                case Sort.Relevance:
                default:
                    return new SortOrder[0];
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
}
