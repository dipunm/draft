using System;
using Infrastructure.Solr.ProductSearcher.Client;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.ProductSearcher.AdditionalDataAdapters
{
    public class SpellingSuggestionAdapter : ISolrAdditionalDataProvider
    {
        public bool CanManage(Type type)
        {
            return type == typeof (SpellingSuggestion);
        }

        public void ApplyRequest(QueryOptions options, IAdditionalData<object> interest)
        {
            options.SpellCheck = new SpellCheckingParameters {Collate = true};
        }

        public object ExtractMeta<T>(ISearchResult<DocumentModel> source) where T : IAdditionalData<object>
        {
            throw new NotImplementedException();
        }
    }
}