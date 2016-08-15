namespace Shopomo.ProductSearcher.Domain.Projections
{
    public class ProductSummary
    {
        public string Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string ImageUrl { get; }
        public bool OnSale { get; }
        public bool FreeDelivery { get; }
        public int Rating { get; }
        public decimal OldPrice { get; }
    }
}