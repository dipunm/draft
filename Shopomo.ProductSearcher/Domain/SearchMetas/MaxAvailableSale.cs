namespace Shopomo.ProductSearcher.Domain.SearchMetas
{
    public class MaxAvailableSale : ISearchMeta<SalePercentage>
    {
        
    }

    public class SalePercentage
    {
        private SalePercentage(decimal amt)
        {
            Amount = amt;
        }

        public static implicit operator decimal(SalePercentage amt)
        {
            return amt.Amount;
        }

        public static implicit operator SalePercentage(decimal amt)
        {
            return new SalePercentage(amt);
        }

        public decimal Amount { get; }
    }
}