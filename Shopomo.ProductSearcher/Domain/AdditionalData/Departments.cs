namespace Shopomo.ProductSearcher.Domain.AdditionalData
{
    public class Departments : FilterOptions, IAdditionalData<string[]>
    {
        public Departments(int limit) : base(limit)
        {
        }
    }
}