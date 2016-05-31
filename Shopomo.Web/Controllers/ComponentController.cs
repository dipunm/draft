using System.Web.Mvc;

namespace Shopomo.Web.Controllers
{
    public class ComponentController : Controller
    {
        public ActionResult Searchbar()
        {
            var model = new object();//_commonService.DepartmentsMenu();
            return PartialView(model);
        }
        public ActionResult Popup()
        {
            return PartialView();
        }

        public ActionResult DepartmentsMenu()
        {

        }

        public ActionResult SearchbarLocal()
        {

        }


    }
}