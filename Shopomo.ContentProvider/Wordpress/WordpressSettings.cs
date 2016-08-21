using System;
using System.Collections.Generic;

namespace Shopomo.ContentProvider.Wordpress
{
    public class WordpressSettings
    {
        public WordpressSettings(Uri wordpressUrl)
        {
            if (wordpressUrl == null) throw new ArgumentNullException(nameof(wordpressUrl));
            if (!wordpressUrl.IsAbsoluteUri) throw new ArgumentException("url must be absolute", nameof(wordpressUrl));

            Pages = new Dictionary<string, string>();
            WordpressUrl = wordpressUrl;
        }

        public Dictionary<string, string> Pages { get; }
        public Uri WordpressUrl { get; }
    }
}