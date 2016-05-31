using System.Web.Mvc;

namespace Shopomo.Web.Controllers
{
    public class MarketingPageController : Controller
    {
        [Route("diamond")]
        public ActionResult DiamondCampaign()
        {
            return RedirectPermanent("http://features.shopomo.com/competitions/win-a-diamond/");
        }
    }
}