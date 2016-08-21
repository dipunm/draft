namespace Shopomo.ProductSearcher.Domain.AdditionalData
{
    public class RelatedBrands : FilterOptions, IAdditionalData<string[]>
    {
        public RelatedBrands(int limit) : base(limit)
        {
        }
    }
}