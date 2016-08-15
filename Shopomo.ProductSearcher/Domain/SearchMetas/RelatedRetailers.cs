namespace Shopomo.ProductSearcher.Domain.SearchMetas
{
    public class RelatedRetailers : FilterOptions, ISearchMeta<string[]>
    {
        public RelatedRetailers(int limit) : base(limit)
        {
        }
    }
}