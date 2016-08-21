using System;
using Infrastructure.Solr.ProductSearcher.Client;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.ProductSearcher.AdditionalDataAdapters
{
    public interface ISolrAdditionalDataProvider
    {
        bool CanManage(Type type);

        void ApplyRequest(QueryOptions options, IAdditionalData<object> interest);

        object ExtractMeta<T>(ISearchResult<DocumentModel> source) where T : IAdditionalData<object>;
    }
}