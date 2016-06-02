using System.Threading.Tasks;
using Shopomo.Searchers.QueryModels;

namespace Shopomo.Searchers
{
    public interface IProductSearcher
    {
        Task<ProductSearchResults> SearchAsync(ProductSearch productSearch);
    }
}