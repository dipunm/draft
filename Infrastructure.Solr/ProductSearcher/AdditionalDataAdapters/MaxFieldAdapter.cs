using System;
using Infrastructure.Solr.ProductSearcher.Client;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.ProductSearcher.AdditionalDataAdapters
{
    public class MaxFieldAdapter : ISolrAdditionalDataProvider
    {
        private readonly string _fieldname;

        public MaxFieldAdapter(string fieldname)
        {
            _fieldname = fieldname;
        }

        public bool CanManage(Type type)
        {
            return type == typeof (MaxAvailableSale);
        }

        public void ApplyRequest(QueryOptions options, IAdditionalData<object> interest)
        {
            options.Stats = options.Stats ?? new StatsParameters();
            options.Stats.AddField(_fieldname);
        }

        public object ExtractMeta<T>(ISearchResult<DocumentModel> source) where T : IAdditionalData<object>
        {
            throw new NotImplementedException();
        }
    }
}