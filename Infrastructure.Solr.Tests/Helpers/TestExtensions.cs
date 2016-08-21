using System;
using System.Collections.Generic;
using System.Linq;
using SolrNet;

namespace Infrastructure.Solr.Tests.Helpers
{
    public static class TestExtensions
    {
        public static bool HasOne<T>(this ICollection<ISolrQuery> queries, Func<T, bool> predicate) where T : ISolrQuery
        {
            return queries.OfType<T>().SingleOrDefault(predicate) != null;
        }

        public static bool HasNone<T>(this ICollection<ISolrQuery> queries, Func<T, bool> predicate)
            where T : ISolrQuery
        {
            return !queries.OfType<T>().Any(predicate);
        }

        public static bool HasOne<T>(this ICollection<ISolrFacetQuery> queries, Func<T, bool> predicate)
            where T : ISolrFacetQuery
        {
            return queries.OfType<T>().SingleOrDefault(predicate) != null;
        }
    }
}