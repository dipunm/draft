using Shopomo.ContentProvider.Models;

namespace Shopomo.ContentProvider.Wordpress.Models
{
    public class WordpressContent : IContent
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}