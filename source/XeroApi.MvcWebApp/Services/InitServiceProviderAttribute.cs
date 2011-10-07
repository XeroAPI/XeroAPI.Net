using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xero.ScreencastWeb.Services
{
    /// <summary>
    /// Initialises the <c ref="ServiceProvider"/> class for controllers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InitServiceProviderAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ServiceProvider.CurrentTokenRepository = new SessionStateTokenRepository(filterContext.HttpContext.Session);
        }

    }
}