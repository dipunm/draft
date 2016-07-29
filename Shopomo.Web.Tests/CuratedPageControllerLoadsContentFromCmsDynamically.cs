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
        [Test]
        public async Task Controller_GivenAnUnknownPageName_ShouldRespondNotFound()
        {
            var knownPages = new[] {"known1", "known2"};
            var controller = new CuratedPageController(null, knownPages);

            var result = await controller.Desktop("unknown");

            result.ShouldBeOfType<HttpNotFoundResult>();
        }

        [Test]
        public async Task Controller_GivenKnownPageName_ShouldPresentDataFromContentProvider()
        {
            var knownPages = new[] { "known1", "known2" };
            var contentProvider = new Mock<IContentProvider>();
            var mockModel = new Mock<IContent>().Object;
            contentProvider.Setup(p => p.GetPageAsync("known1"))
                .ReturnsAsync(mockModel);
            var controller = new CuratedPageController(contentProvider.Object, knownPages);

            var result = await controller.Desktop("known1");

            result.ShouldBeOfType<ViewResult>();
            (result as ViewResult).Model.ShouldBe(mockModel);
        }

        [Test]
        public async Task Controller_WhenLoadingMobilePage_ShouldUseMobilePage()
        {
            var knownPages = new[] { "known1", "known2" };
            var contentProvider = new Mock<IContentProvider>();
            var mockModel = new Mock<IContent>().Object;
            contentProvider.Setup(p => p.GetPageAsync("known1"))
                .ReturnsAsync(mockModel);
            var controller = new CuratedPageController(contentProvider.Object, knownPages);

            var result = await controller.Mobile("known1");

            result.ShouldBeOfType<ViewResult>();
            (result as ViewResult).ViewName.ShouldBe("mobile");
        }

        [Test]
        public async Task Controller_WhenContentProviderReturnsNoContent_ShouldRespondServerError()
        {
            var knownPages = new[] { "known1", "known2" };
            var contentProvider = new Mock<IContentProvider>();
            contentProvider.SetReturnsDefault(Task.FromResult<IContent>(null));
            var controller = new CuratedPageController(contentProvider.Object, knownPages);

            var result = await controller.Desktop("known1");

            result.ShouldBeOfType<HttpStatusCodeResult>();
            (result as HttpStatusCodeResult).StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }

        //TODO:???
        public void Controller_WhenContentProviderReturnsNoContent_ShouldLogError()
        {

        }


    }
}
