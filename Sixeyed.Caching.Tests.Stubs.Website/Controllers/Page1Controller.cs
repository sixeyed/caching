using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Sixeyed.Caching.Tests.Stubs.Website.Controllers
{
    public class Page1Controller : Controller
    {
        [OutputCache(CacheProfile = "Disk")]
        public ActionResult Index()
        {
            Thread.Sleep(2000);
            return View();
        }
    }
}
