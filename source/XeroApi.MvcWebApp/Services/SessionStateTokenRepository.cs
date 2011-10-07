using System;
using System.Web;

using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Storage.Basic;

namespace Xero.ScreencastWeb.Services
{
    public class SessionStateTokenRepository : ITokenRepository
    {
        private readonly HttpSessionStateBase _sessionState;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStateTokenRepository"/> class.
        /// </summary>
        /// <param name="sessionState">State of the session.</param>
        public SessionStateTokenRepository(HttpSessionStateBase sessionState)
        {
            _sessionState = sessionState;
        }

        public RequestToken GetRequestToken()
        {
            return (_sessionState["request_token"] == null) ? null : (RequestToken)_sessionState["request_token"];
        }

        public void SaveRequestToken(RequestToken requestToken)
        {
            _sessionState["request_token"] = requestToken;
        }

        public AccessToken GetAccessToken()
        {
            return (_sessionState["access_token"] == null) ? null : (AccessToken)_sessionState["access_token"];
        }

        public void SaveAccessToken(AccessToken accessToken)
        {
            _sessionState["access_token"] = accessToken;
        }
    }
}
