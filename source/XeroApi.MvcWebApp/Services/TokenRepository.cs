using System;
using System.Web;

using DevDefined.OAuth.Storage.Basic;
using Xero.ScreencastWeb.Services.Interfaces;

namespace Xero.ScreencastWeb.Services
{
    public class HttpSessionAccessTokenRepository : ITokenRepository<AccessToken>
    {
        private readonly HttpSessionStateBase _sessionState;

        public HttpSessionAccessTokenRepository(HttpSessionStateBase sessionState)
        {
            _sessionState = sessionState;
        }

        public AccessToken GetToken(string token)
        {
            return (_sessionState["access_token"] == null) ? null : (AccessToken)_sessionState["access_token"];
        }

        public void SaveToken(AccessToken token)
        {
            _sessionState["access_token"] = token;
        }
    }

    public class HttpSessionRequestTokenRepository : ITokenRepository<RequestToken>
    {
        private readonly HttpSessionStateBase _sessionState;

        public HttpSessionRequestTokenRepository(HttpSessionStateBase sessionState)
        {
            _sessionState = sessionState;
        }

        public RequestToken GetToken(string token)
        {
            return (_sessionState["request_token"] == null) ? null : (RequestToken)_sessionState["request_token"];
        }

        public void SaveToken(RequestToken token)
        {
            _sessionState["request_token"] = token;
        }
    }
}
