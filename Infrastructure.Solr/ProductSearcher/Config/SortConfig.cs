using System.Collections.Generic;
using Shopomo.ProductSearcher.Domain.Search;
using SolrNet;

namespace Infrastructure.Solr.ProductSearcher.Config
{
    public static class SortConfig
    {
        public static Dictionary<Sort, ICollection<SortOrder>> SortQueries { get; } = new Dictionary
            <Sort, ICollection<SortOrder>>
        {
            {
                Sort.PriorityThenRandom,
                new[] {new SortOrder(SolrFields.PriorityOrder, Order.DESC), new SortOrder(SolrFields.RandomOrder)}
            },
            {Sort.RandomOrder, new[] {new SortOrder(SolrFields.RandomOrder)}},
            {Sort.PriceAsc, new[] {new SortOrder(SolrFields.PriceOrder, Order.ASC)}},
            {Sort.PriceDesc, new[] {new SortOrder(SolrFields.PriceOrder, Order.DESC)}},
            {Sort.Relevance, new SortOrder[0]}
        };
    }
}