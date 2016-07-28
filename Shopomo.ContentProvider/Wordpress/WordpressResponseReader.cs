using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shopomo.ContentProvider.Models;
using Shopomo.ContentProvider.Wordpress.Models;

namespace Shopomo.ContentProvider.Wordpress
{
    public class WordpressResponseReader : IResponseReader
    {
        public IContent GetContent(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<WordpressContent>(JObject.Parse(json)["page"].ToString());
        }
    }
}