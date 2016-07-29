using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Shopomo.ContentProvider;
using Shopomo.ContentProvider.Models;
using Shopomo.Web.Controllers;
using Shouldly;

namespace Shopomo.Web.Tests
{
    [TestFixture]
    public class CuratedPageControllerLoadsContentFromCmsDynamically
    {
        private List<string> _knownPages;
        private CuratedPageController _controller;
        private Mock<IContentProvider> _contentProvider;

        [SetUp]
        public void Setup()
        {
            _contentProvider = new Mock<IContentProvider>();
            _knownPages = new List<string>();
            _controller = new CuratedPageController(_contentProvider.Object, _knownPages);
        }

        [Test]
        public async Task Controller_GivenAnUnknownPageName_ShouldRespondNotFound()
        {
            _knownPages.Add("known1");

            var result = await _controller.Desktop("unknown");

            result.ShouldBeOfType<HttpNotFoundResult>();
        }

        [Test]
        public async Task Controller_GivenKnownPageName_ShouldPresentDataFromContentProvider()
        {
            _knownPages.Add("known1");
            var mockModel = new Mock<IContent>().Object;
            _contentProvider.Setup(p => p.GetPageAsync("known1"))
                .ReturnsAsync(mockModel);

            var result = await _controller.Desktop("known1");

            result.ShouldBeOfType<ViewResult>();
            (result as ViewResult).Model.ShouldBe(mockModel);
        }

        [Test]
        public async Task Controller_WhenLoadingMobilePage_ShouldUseMobilePage()
        {
            _knownPages.Add("known1");
            var mockModel = new Mock<IContent>().Object;
            _contentProvider.Setup(p => p.GetPageAsync("known1"))
                .ReturnsAsync(mockModel);

            var result = await _controller.Mobile("known1");

            result.ShouldBeOfType<ViewResult>();
            (result as ViewResult).ViewName.ShouldBe("mobile");
        }

        [Test]
        public async Task Controller_WhenContentProviderReturnsNoContent_ShouldRespondServerError()
        {
            _knownPages.Add("known1");
            _contentProvider.Setup(p => p.GetPageAsync("known1")).ReturnsAsync(null);
            
            var result = await _controller.Desktop("known1");

            result.ShouldBeOfType<HttpStatusCodeResult>();
            (result as HttpStatusCodeResult).StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }

        //TODO:???
        public void Controller_WhenContentProviderReturnsNoContent_ShouldLogError()
        {

        }


    }
}
