using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shopomo.ContentProvider;
using Shopomo.ContentProvider.Models;

namespace Shopomo.Web.Controllers
{
    //TODO: Caching

    [RoutePrefix("content")]
    public class CuratedPageController : Controller
    {
        private readonly IContentProvider _contentProvider;
        private readonly IEnumerable<string> _supportedPages;

        public CuratedPageController(IContentProvider contentProvider, string[] knownPages)
        {
            _contentProvider = contentProvider;
            _supportedPages = knownPages;
        }

        //TODO: implement DI and remove this controller
        public CuratedPageController()
        {
            _contentProvider = new DummyProvider();
            _supportedPages = new List<string>()
            {
                "pagea", "pageb", "AboutUs"
            };
        }

        [Route("{pageName}mobile")]
        public Task<ActionResult> Mobile(string pageName) => LoadPage(pageName, "mobile");

        [Route("{pageName}")]
        public Task<ActionResult> Desktop(string pageName) => LoadPage(pageName, "desktop");

        private async Task<ActionResult> LoadPage(string pageName, string viewName)
        {
            if (!_supportedPages.Contains(pageName))
                return HttpNotFound();

            var page = await _contentProvider.GetPageAsync(pageName);
            if (page == null)
            {
                //TODO: logging?
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Content provider was unable to provide content.");
            }
            return View(viewName, page);
        }
    }


    //TODO: Remove this class
    public class DummyProvider : IContentProvider
    {
        public Task<IContent> GetPageAsync(string pageName)
        {
            return Task.FromResult<IContent>(new DummyContent("pageName", pageName));
        }

        public class DummyContent : IContent
        {
            public DummyContent(string title, string content)
            {
                Title = title;
                Content = content;
            }

            public string Title { get; }
            public string Content { get; }
        }
    }
}