using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shopomo.Web.Controllers
{
    //TODO: Caching

    public class LandingPageController : Controller
    {
        [Route("")]
        public Task<ActionResult> Home()
        {
            return Task.FromResult<ActionResult>(View());
        }

        [Route("d/{id}/{slug}")]
        public Task<ActionResult> Department(string id)
        {
            return Task.FromResult<ActionResult>(View());
        }

        [Route("brands")]
        public Task<ActionResult> Brands(string id)
        {
            return Task.FromResult<ActionResult>(View());
        }
    }
}