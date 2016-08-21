using SolrNet.Attributes;

namespace Infrastructure.Solr.ProductSearcher.Client
{
    public class DocumentModel
    {
        [SolrUniqueKey("id")]
        public string Id { get; set; }

        [SolrField("name")]
        public string Name { get; set; }

        //...
    }
}