using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xero.ScreencastWeb.Services;

namespace Xero.ScreencastWeb.Controllers
{
    [InitServiceProvider]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
