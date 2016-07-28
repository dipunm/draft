using System.Threading.Tasks;
using Shopomo.ContentProvider.Models;

namespace Shopomo.ContentProvider
{
    public interface IContentProvider
    {
        Task<IContent> GetPageAsync(string pageName);
    }
}
