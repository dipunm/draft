using System;
using Infrastructure.Solr.ProductSearcher.Client;
using Shopomo.ProductSearcher.Domain;
using Shopomo.ProductSearcher.Domain.AdditionalData;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace Infrastructure.Solr.ProductSearcher.AdditionalDataAdapters
{
    public class FilterOptionsAdapter<TMeta> : ISolrAdditionalDataProvider
        where TMeta : FilterOptions, IAdditionalData<object>
    {
        private readonly string _fieldname;

        public FilterOptionsAdapter(string fieldname)
        {
            _fieldname = fieldname;
        }

        public bool CanManage(Type type)
        {
            return typeof (TMeta) == type;
        }

        public void ApplyRequest(QueryOptions options, IAdditionalData<object> interest)
        {
            options.Facet.Queries.Add(new SolrFacetFieldQuery(_fieldname) {Limit = ((TMeta) interest).Limit});
        }

        public object ExtractMeta<T>(ISearchResult<DocumentModel> source) where T : IAdditionalData<object>
        {
            throw new NotImplementedException();
        }
    }
}