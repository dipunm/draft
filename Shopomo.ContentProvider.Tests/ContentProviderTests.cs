using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shopomo.ContentProvider.Wordpress;
using Shopomo.ContentProvider.Wordpress.Models;
using Shouldly;

namespace Shopomo.ContentProvider.Tests
{
    [TestFixture]
    public class ContentProviderShouldUseSettingsAndHttpClientToGetWordpressContent
    {
        private MockHttpClient _httpClient;
        private Mock<IResponseReader> _mockReader;

        [SetUp]
        public void Setup()
        {
            _mockReader = new Mock<IResponseReader>();
        }

        [Test]
        public void WhenLoadingAnUndefinedPage_ShouldThrowArgumentException()
        {
            var settings = WordpressSettingsBuilder.BuildWithPage("terms", "1234");
            var provider = CreateContentProvider(settings);
            Assert.ThrowsAsync<ArgumentException>(async () => await provider.GetPageAsync("undefined"));
        }

        [Test]
        public async Task WhenPageIdFound_ShouldMakeAnHttpRequestToWordpress()
        {
            var settings = WordpressSettingsBuilder.BuildWithPage("terms", "1234");
            var provider = CreateContentProvider(settings);
            await provider.GetPageAsync("terms");
            _httpClient.Actions.Single().Method.ShouldBe(HttpMethod.Get);
        }

        [Test]
        public async Task WhenHttpRequestMadeForPage_ShouldUseSettingsToDetermineWordpressUrl()
        {
            var settings = WordpressSettingsBuilder.BuildWithUrlAndPage(new Uri("http://fail-fast-test.com"), "terms", "1234");
            var provider = CreateContentProvider(settings);
            await provider.GetPageAsync("terms");
            _httpClient.Actions.Single().RequestUri.GetLeftPart(UriPartial.Path).ShouldBe("http://fail-fast-test.com/");
        }

        [Test]
        public async Task WhenHttpRequestMadeForPage_ShouldRequestSpecificPageByIdInQuerystring()
        {
            var settings = WordpressSettingsBuilder.BuildWithPage("terms", "1234");
            var provider = CreateContentProvider(settings);
            await provider.GetPageAsync("terms");
            _httpClient.Actions.Single().RequestUri.Query.ShouldBe("?json=get_page&id=1234");
        }

        [Test]
        public async Task WhenPageLoadedFromWordpress_ShouldReturnModelOfPageUsingResponseReader()
        {
            var settings = WordpressSettingsBuilder.BuildWithPage("terms", "1234");
            var mockResponseText = "{'wordpress': ['page', 'json']}";
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(mockResponseText)};
            var provider = CreateContentProvider(settings, mockResponse);
            var mockPage = new WordpressContent();
            _mockReader.Setup(b => b.GetContent(mockResponseText))
                .Returns(mockPage);

            var page = await provider.GetPageAsync("terms");

            page.ShouldBe(mockPage);
        }

        [Test]
        public async Task WhenWordpressReturnsNoContent_ShouldReturnNull()
        {
            var settings = WordpressSettingsBuilder.BuildWithPage("terms", "1234");
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = null };
            var provider = CreateContentProvider(settings, mockResponse);
            var page = await provider.GetPageAsync("terms");

            page.ShouldBeNull();
        }

        [Test]
        public async Task WhenWordpressReturns404_ShouldReturnNull()
        {
            var settings = WordpressSettingsBuilder.BuildWithPage("terms", "1234");
            var mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            var provider = CreateContentProvider(settings, mockResponse);
            var page = await provider.GetPageAsync("terms");

            page.ShouldBeNull();
        }

        private IContentProvider CreateContentProvider(WordpressSettings settings, HttpResponseMessage response = null)
        {
            return new WordpressContentProvider(
                _httpClient = new MockHttpClient(response), 
                _mockReader.Object,
                settings);
        }
    }
}
