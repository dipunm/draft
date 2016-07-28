using System;
using System.Security.Policy;
using Shopomo.ContentProvider.Wordpress;

namespace Shopomo.ContentProvider.Tests
{
    internal static class WordpressSettingsBuilder
    {
        private static readonly Uri Url = new Uri("http://fail-fast.com/");
        public static WordpressSettings BuildValidSettings()
        {
            return new WordpressSettings(Url);
        }

        public static WordpressSettings BuildWithPage(string page, string id)
        {
            return new WordpressSettings(Url)
            {
                Pages = { { page, id } }
            };
        }

        public static WordpressSettings BuildWithUrlAndPage(Uri url, string page, string id)
        {
            return new WordpressSettings(url)
            {
                Pages = { { page, id } }
            };
        }
    }
}