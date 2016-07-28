using Shopomo.ContentProvider.Models;

namespace Shopomo.ContentProvider.Wordpress
{
    public interface IResponseReader
    {
        IContent GetContent(string json);
    }
}