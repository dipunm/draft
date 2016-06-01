using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shopomo.ContentProvider;

namespace Shopomo.Web.Controllers
{
    //TODO: Caching

    [RoutePrefix("content")]
    public class CmsPageController : Controller
    {
        private readonly IContentProvider _contentProvider;

        public CmsPageController(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public CmsPageController()
        {
            
        }

        [Route("{pageName}")]
        public async Task<ActionResult> CmsPage(string pageName)
        {
            bool mobile = false;
            if (pageName.EndsWith("Mobile", StringComparison.InvariantCultureIgnoreCase))
            {
                mobile = true;
                pageName = pageName.Substring(0, pageName.Length - 6);
            }
            /*
            var page = await _contentProvider.GetPageAsync(pageName);
            if (page == null)
                return HttpNotFound();
                */

            var page = new MvcHtmlString("Hello world" + pageName);

            if (mobile)
                return View("mobile", page);
            else
                return View("desktop", page);
        }
    }
}