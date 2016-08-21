namespace Shopomo.ProductSearcher.Domain.AdditionalData
{
    public class RelatedRetailers : FilterOptions, IAdditionalData<string[]>
    {
        public RelatedRetailers(int limit) : base(limit)
        {
        }
    }
}