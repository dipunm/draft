namespace Shopomo.ProductSearcher.Domain.AdditionalData
{
    public class MaxAvailableSale : IAdditionalData<SalePercentage>
    {
    }

    public class SalePercentage
    {
        private SalePercentage(decimal amt)
        {
            Amount = amt;
        }

        public decimal Amount { get; }

        public static implicit operator decimal(SalePercentage amt)
        {
            return amt.Amount;
        }

        public static implicit operator SalePercentage(decimal amt)
        {
            return new SalePercentage(amt);
        }
    }
}