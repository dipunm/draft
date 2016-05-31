using System.Threading.Tasks;

namespace Shopomo.ContentProvider.Wordpress
{
    public class WordpressContentProvider : IContentProvider
    {
        public Task<object> GetPageAsync(string pageName)
        {
            throw new System.NotImplementedException();
        }
    }
}