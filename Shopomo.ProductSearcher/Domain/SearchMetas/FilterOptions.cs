namespace Shopomo.ProductSearcher.Domain.SearchMetas
{
    public abstract class FilterOptions
    {
        protected FilterOptions(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; }
    }
}