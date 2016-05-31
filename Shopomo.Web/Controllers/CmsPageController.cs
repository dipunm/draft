using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shopomo.ContentProvider;

namespace Shopomo.Web.Controllers
{
    public class CmsPageController : Controller
    {
        private readonly IContentProvider _contentProvider;

        public CmsPageController(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public async Task<ActionResult> CmsPage(string pageName)
        {
            bool mobile = false;
            if (pageName.EndsWith("Mobile", StringComparison.InvariantCultureIgnoreCase))
            {
                mobile = true;
                pageName = pageName.Substring(0, pageName.Length - 6);
            }

            var page = await _contentProvider.GetPageAsync(pageName);
            if (page == null)
                return HttpNotFound();

            if (mobile)
                return View("mobile", page);
            else
                return View("desktop", page);
        }
    }
}