using System;
using Infrastructure.Solr.ProductSearcher.Client;
using SolrNet;

namespace Infrastructure.Solr
{
    public static class SolrBootstrapper
    {
        public static void InitProductSearcher(Uri solrUrl)
        {
            Startup.Init<DocumentModel>(new UriBuilder(solrUrl) {Path = "/solr/products"}.Uri.AbsoluteUri);
        }
    }
}