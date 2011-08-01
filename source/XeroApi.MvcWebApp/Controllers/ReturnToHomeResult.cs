using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Xero.ScreencastWeb.Controllers
{
    public class ReturnToHomeResult : ActionResult
    {
        private string _homepageMessage = string.Empty;

        public ReturnToHomeResult()
        {
            
        }

        public ReturnToHomeResult(string homepageMessage)
        {
            _homepageMessage = homepageMessage;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            RouteValueDictionary routeDictionary = new RouteValueDictionary {{"controller", "Home"}, {"action", "Index"}};

            if (!string.IsNullOrEmpty(_homepageMessage))
            {
                routeDictionary.Add("message", _homepageMessage);
            }

            RedirectToRouteResult redirectResult = new RedirectToRouteResult(routeDictionary);
            redirectResult.ExecuteResult(context);
        }
    }
}