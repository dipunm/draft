using System.Threading.Tasks;

namespace Shopomo.ContentProvider
{
    public interface IContentProvider
    {
        Task<object> GetPageAsync(string pageName);
    }
}
