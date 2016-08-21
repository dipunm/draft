using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shopomo.ContentProvider.Models;

namespace Shopomo.ContentProvider.Wordpress
{
    public class WordpressContentProvider : IContentProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IResponseReader _responseReader;
        private readonly WordpressSettings _settings;

        public WordpressContentProvider(HttpClient httpClient, IResponseReader responseReader,
            WordpressSettings settings)
        {
            _httpClient = httpClient;
            _responseReader = responseReader;
            _settings = settings;
        }

        public async Task<IContent> GetPageAsync(string pageName)
        {
            if (!_settings.Pages.ContainsKey(pageName))
                throw new ArgumentException(
                    $"unknown page: '{pageName}'. Check that all expected pages have been configured in the {nameof(WordpressSettings)}",
                    nameof(pageName));

            var id = _settings.Pages[pageName];
            var requestUrl = new UriBuilder(_settings.WordpressUrl)
            {
                Query = $"json=get_page&id={id}"
            }.Uri;
            var response = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false);

            var content = response?.Content;
            return content != null
                ? _responseReader.GetContent(await content.ReadAsStringAsync().ConfigureAwait(false))
                : null;
        }
    }
}