namespace Shopomo.ProductSearcher.Domain.SearchMetas
{
    public class RelatedBrands : FilterOptions, ISearchMeta<string[]>
    {
        public RelatedBrands(int limit) : base(limit)
        {
        }
    }
}