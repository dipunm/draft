using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Shopomo.ContentProvider.Tests.Helpers
{
    internal class MockMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        private List<HttpRequestMessage> _actionsList;

        public MockMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        public void SetActionList(List<HttpRequestMessage> actionsList)
        {
            _actionsList = actionsList;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_actionsList == null)
                throw new InvalidOperationException("Call SetActionList first.");

            _actionsList.Add(request);
            return Task.FromResult(_response ?? new HttpResponseMessage());
        }
    }
}