namespace Shopomo.ProductSearcher.Domain.Search
{
    public enum Sort
    {
        Relevance = 0, PriceAsc, PriceDesc,
        RandomOrder, PriorityThenRandom
    }
}