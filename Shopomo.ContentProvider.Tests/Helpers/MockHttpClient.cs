using System.Collections.Generic;
using System.Net.Http;

namespace Shopomo.ContentProvider.Tests
{
    internal class MockHttpClient : HttpClient
    {
        public List<HttpRequestMessage> Actions { get; } = new List<HttpRequestMessage>();

        public MockHttpClient(HttpResponseMessage response) : this(new MockMessageHandler(response))
        {
        }

        private MockHttpClient(MockMessageHandler handler) : base(handler)
        {
            handler.SetActionList(Actions);
        }
    }
}