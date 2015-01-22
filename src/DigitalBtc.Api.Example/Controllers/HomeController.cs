using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalBtc.Api.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(object foo)
        {
            var res = MvcApplication.ApiProxy.Price();
            return Content(res.JsonResponse);
        }
    }
}
