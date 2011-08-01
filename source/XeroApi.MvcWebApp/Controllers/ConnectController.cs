using System;
using System.Diagnostics;
using System.Web.Mvc;

using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;

using Xero.ScreencastWeb.Models;
using Xero.ScreencastWeb.Services;
using Xero.ScreencastWeb.Services.Interfaces;

namespace Xero.ScreencastWeb.Controllers
{
    public class ConnectController : ControllerBase
    {

        //
        // GET: /Connect/Index - Main OAuth Connection Endpoint
        
        public ActionResult Index()
        {
            Debug.Write("Processing: /Connect/Index");

            ApiRepository apiRepository = new ApiRepository();
            ITokenRepository<AccessToken> accessTokenRepository = new HttpSessionAccessTokenRepository(Session);
            ITokenRepository<RequestToken> requestTokenRepository = new HttpSessionRequestTokenRepository(Session);

            // Do we already have a session token in sessionstate? - is it still usable?
            if (accessTokenRepository.GetToken("") != null)
            {
                if (apiRepository.TestConnectionToXeroApi(accessTokenRepository))
                {
                    return new RedirectResult("~/");
                }

                // The current session token+secret doesn't work - probably due to it expiring in 30mins.
                accessTokenRepository.SaveToken(null);
            }
            

            // Call api.xero.com/oauth/AccessToken
            IOAuthSession oauthSession = apiRepository.GetOAuthSession();
            RequestToken requestToken = oauthSession.GetRequestToken();

            requestTokenRepository.SaveToken(requestToken);

            Trace.WriteLine("OAuth Request Token: " + requestToken.Token);
            Trace.WriteLine("OAuth Request Secret: " + requestToken.TokenSecret);

            string authorisationUrl = oauthSession.GetUserAuthorizationUrlForToken(requestToken);

            Trace.WriteLine("Redirecting browser to user authorisation uri:" + authorisationUrl);

            return new RedirectResult(authorisationUrl);
        }

        //
        // GET: /Connect/Callback - Callback url from the api.xero.com authorisation endpoint

        public ActionResult Callback()
        {
            Debug.Write("Processing: /Connect/Callback");

            ApiRepository apiRepository = new ApiRepository();

            ITokenRepository<AccessToken> accessTokenRepository = new HttpSessionAccessTokenRepository(Session);
            ITokenRepository<RequestToken> requestTokenRepository = new HttpSessionRequestTokenRepository(Session);

            string verificationCode = Request.Params["oauth_verifier"];

            // Call api.xero.com/oauth/AccessToken
            IOAuthSession oauthSession = apiRepository.GetOAuthSession();

            IToken requestToken = requestTokenRepository.GetToken("");

            if (requestToken == null)
            {
                throw new ApplicationException("The request token could not be retrived from the current http session. Is session state and cookies enabled?");
            }

            AccessToken accessToken = oauthSession.ExchangeRequestTokenForAccessToken(requestToken, verificationCode);

            accessTokenRepository.SaveToken(accessToken);

            GetAndStoreAuthorisedOrganisationName(apiRepository);

            return new RedirectResult("~/");
        }

        public ActionResult RefreshAccessToken()
        {
            Debug.Write("Processing: /Connect/RefreshAccessToken");

            ApiRepository apiRepository = new ApiRepository();
            ITokenRepository<AccessToken> accessTokenRepository = new HttpSessionAccessTokenRepository(Session);

            // This will take the existing access token from sessionState, call to API to repace the access token for a new one, and write the new access token back to session state.
            apiRepository.RenewAccessToken(accessTokenRepository);

            GetAndStoreAuthorisedOrganisationName(apiRepository);

            return new RedirectResult("~/");
        }

        private void GetAndStoreAuthorisedOrganisationName(ApiRepository apiRepository)
        {
            // Get the organisation name from the api
            Response organisationResponse = apiRepository.GetItemByIdOrCode<Organisation>(Session, null);
            if (organisationResponse.Organisations != null && organisationResponse.Organisations.Count > 0)
            {
                Session["xero_organisation_name"] = organisationResponse.Organisations[0].Name;
                Session["xero_connection_time"] = DateTime.Now;
            }
        }

    }
}
