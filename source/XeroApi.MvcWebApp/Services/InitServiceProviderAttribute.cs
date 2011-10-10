using System;
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
            // Use the current session state to hold request+access tokens for the current user
            ServiceProvider.CurrentTokenRepository = new SessionStateTokenRepository(filterContext.HttpContext.Session);
        }

    }
}