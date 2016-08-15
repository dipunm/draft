    namespace Shopomo.ProductSearcher.Domain.SearchMetas
{
    public class Departments : FilterOptions, ISearchMeta<string[]>
    {
        public Departments(int limit) : base(limit)
        {
        }
    }
}