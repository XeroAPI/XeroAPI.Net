using System;
using System.Diagnostics;
using System.Web.Mvc;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Storage.Basic;

using Xero.ScreencastWeb.Services;

using XeroApi;
using XeroApi.OAuth;

namespace Xero.ScreencastWeb.Controllers
{
    [InitServiceProvider]
    public class ConnectController : ControllerBase
    {

        //
        // GET: /Connect/Index - Main OAuth Connection Endpoint
        
        public ActionResult Index()
        {
            Debug.Write("Processing: /Connect/Index");
            
            IOAuthSession oauthSession = ServiceProvider.GetCurrentSession();
            CoreRepository repository = ServiceProvider.GetCurrentRepository();
            
            // Can we already access an organisation??
            if (oauthSession.HasValidAccessToken && repository != null && repository.Organisation != null)
            {
                return new RedirectResult("~/");
            }
            
            if (oauthSession is XeroApiPrivateSession)
            {
                throw new ApplicationException("The current private session cannot access the authorised organisation. Has the access token been revoked?");
            }


            // Determine the callback uri to use - this must match the domain used when the application was registerd on http://api.xero.com
            var callbackUri = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port, Url.Action("Callback"));
            

            // Call: GET /oauth/RequestToken
            RequestToken requestToken = oauthSession.GetRequestToken(callbackUri.Uri);
            
            Trace.WriteLine("OAuth Request Token: " + requestToken.Token);
            Trace.WriteLine("OAuth Request Secret: " + requestToken.TokenSecret);

            string authorisationUrl = oauthSession.GetUserAuthorizationUrl();
            Trace.WriteLine("Redirecting browser to user authorisation uri:" + authorisationUrl);
            return new RedirectResult(authorisationUrl);
        }


        //
        // GET: /Connect/Callback - Callback url from the api.xero.com authorisation endpoint

        public ActionResult Callback()
        {
            Debug.Write("Processing: /Connect/Callback");

            IOAuthSession oauthSession = ServiceProvider.GetCurrentSession();
            
            
            string verificationCode = Request.Params["oauth_verifier"];
            AccessToken accessToken = oauthSession.ExchangeRequestTokenForAccessToken(verificationCode);

            Trace.WriteLine("OAuth Access Token: " + accessToken.Token);
            Trace.WriteLine("OAuth Access Secret: " + accessToken.TokenSecret);

            GetAndStoreAuthorisedOrganisationName();

            return new RedirectResult("~/");
        }


        //
        // GET /Connect/RefreshAccessToken

        public ActionResult RefreshAccessToken()
        {
            Debug.Write("Processing: /Connect/RefreshAccessToken");

            IOAuthSession session = ServiceProvider.GetCurrentSession();
            
            var newAccessToken = session.RenewAccessToken();

            Trace.WriteLine("New OAuth Access Token: " + newAccessToken.Token);
            Trace.WriteLine("New OAuth Access Secret: " + newAccessToken.TokenSecret);
            
            GetAndStoreAuthorisedOrganisationName();

            return new RedirectResult("~/");
        }

        // GET /Connect/Disconnect

        public ActionResult Disconnect()
        {
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        private void GetAndStoreAuthorisedOrganisationName()
        {
            var repository = ServiceProvider.GetCurrentRepository();

            // Get the organisation name from the api
            var currentOrganisation = repository.Organisation;

            if (currentOrganisation != null)
            {
                Session["xero_organisation_name"] = currentOrganisation.Name;
                Session["xero_connection_time"] = DateTime.Now;
            }
        }

    }
}
