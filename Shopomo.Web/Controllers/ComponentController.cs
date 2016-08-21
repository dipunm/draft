using System.Web.Mvc;

namespace Shopomo.Web.Controllers
{
    [RoutePrefix("component")]
    public class ComponentController : Controller
    {
        [Route("searchbar")]
        public ActionResult Searchbar()
        {
            var model = new object(); //_commonService.DepartmentsMenu();
            return PartialView(model);
        }

        [Route("popup")]
        public ActionResult Popup()
        {
            return PartialView();
        }

        [ChildActionOnly]
        [Route("no-access")]
        public ActionResult DepartmentsMenu()
        {
            return Content("null");
        }
    }
}