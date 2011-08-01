#region License

// The MIT License
//
// Copyright (c) 2006-2008 DevDefined Limited.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;


namespace DevDefined.OAuth.Consumer
{
  [Serializable]
  public class OAuthSession : IOAuthSession
  {
    readonly NameValueCollection _cookies = new NameValueCollection();
    readonly NameValueCollection _formParameters = new NameValueCollection();
    readonly NameValueCollection _headers = new NameValueCollection();
    readonly NameValueCollection _queryParameters = new NameValueCollection();
    IConsumerRequestFactory _consumerRequestFactory = DefaultConsumerRequestFactory.Instance;


    public OAuthSession(IOAuthConsumerContext consumerContext)
    {
      ConsumerContext = consumerContext;
    }


    public IConsumerRequestFactory ConsumerRequestFactory
    {
      get { return _consumerRequestFactory; }
      set
      {
        if (_consumerRequestFactory == null) throw new ArgumentNullException("value");
        _consumerRequestFactory = value;
      }
    }

    public bool CallbackMustBeConfirmed { get; set; }
    public IOAuthConsumerContext ConsumerContext { get; set; }
    public IToken AccessToken { get; set; }

    public IMessageLogger MessageLogger { get; set; }

    public IConsumerResponse LogMessage(IConsumerRequest request, IConsumerResponse response)
    {
        if (MessageLogger != null)
        {
            MessageLogger.LogMessage(request, response);
        }

        return response;
    }

    public virtual IConsumerRequest Request(IToken accessToken)
    {
      var context = new OAuthContext
        {
          UseAuthorizationHeader = ConsumerContext.UseHeaderForOAuthParameters
        };

      context.Cookies.Add(_cookies);
      context.FormEncodedParameters.Add(_formParameters);
      context.Headers.Add(_headers);
      context.QueryParameters.Add(_queryParameters);

      return _consumerRequestFactory.CreateConsumerRequest(this, context, ConsumerContext, accessToken);
    }

    public virtual IConsumerRequest Request()
    {
      var context = new OAuthContext
      {
        UseAuthorizationHeader = ConsumerContext.UseHeaderForOAuthParameters
      };

      context.Cookies.Add(_cookies);
      context.FormEncodedParameters.Add(_formParameters);
      context.Headers.Add(_headers);
      context.QueryParameters.Add(_queryParameters);

      return _consumerRequestFactory.CreateConsumerRequest(this, context, ConsumerContext, AccessToken);
    }

    public RequestToken GetRequestToken()
    {
      return GetRequestToken(null);
    }

    public RequestToken GetRequestToken(Uri callbackUri)
    {
        var request = Request()
          .ForMethod("GET")
          .AlterContext(context => context.CallbackUrl = (callbackUri == null) ? "oob" : callbackUri.ToString())
          .AlterContext(context => context.Token = null)
          .ForUri(ConsumerContext.RequestTokenUri)
          .SignWithoutToken();

        var results = request.Select(collection =>
        new
        {
            ConsumerContext.ConsumerKey,
            Token = ParseResponseParameter(collection, Parameters.OAuth_Token),
            TokenSecret = ParseResponseParameter(collection, Parameters.OAuth_Token_Secret),
            CallackConfirmed = WasCallbackConfimed(collection)
        });

        if (!results.CallackConfirmed && CallbackMustBeConfirmed)
        {
            throw Error.CallbackWasNotConfirmed();
        }

        return new RequestToken
        {
            ConsumerKey = results.ConsumerKey,
            Token = results.Token,
            TokenSecret = results.TokenSecret
        };
    }

    public AccessToken ExchangeRequestTokenForAccessToken(IToken requestToken)
    {
      return ExchangeRequestTokenForAccessToken(requestToken, null);
    }
      
    public AccessToken ExchangeRequestTokenForAccessToken(IToken requestToken, string verificationCode)
    {
      AccessToken token = BuildExchangeRequestTokenForAccessTokenContext(requestToken, verificationCode)
        .Select(collection =>
                new AccessToken
                  {
                    ConsumerKey = requestToken.ConsumerKey,
                    Token = ParseResponseParameter(collection,Parameters.OAuth_Token),
                    TokenSecret = ParseResponseParameter(collection,Parameters.OAuth_Token_Secret),
                    SessionHandle = ParseResponseParameter(collection, Parameters.OAuth_Session_Handle),
                    ExpiresIn = ParseResponseParameter(collection, Parameters.OAuth_Expires_In),
                    CreatedDateUtc = DateTime.UtcNow
                  });

      AccessToken = token;

      return token;
    }

    public AccessToken RenewAccessToken(IToken accessToken, string sessionHandle)
    {
        AccessToken token = BuildRenewAccessTokenContext(accessToken, sessionHandle)
          .Select(collection =>
                  new AccessToken
                  {
                      ConsumerKey = accessToken.ConsumerKey,
                      Token = ParseResponseParameter(collection, Parameters. OAuth_Token),
                      TokenSecret = ParseResponseParameter(collection,Parameters.OAuth_Token_Secret),
                      SessionHandle = ParseResponseParameter(collection, Parameters.OAuth_Session_Handle),
                      ExpiresIn = ParseResponseParameter(collection, Parameters.OAuth_Expires_In),
                      CreatedDateUtc = DateTime.UtcNow
                  });

        AccessToken = token;

        return token;
    }

    public IConsumerRequest BuildExchangeRequestTokenForAccessTokenContext(IToken requestToken, string verificationCode)
    {
      return Request()
        .ForMethod("GET")
        .AlterContext(context => context.Verifier = verificationCode)
        .ForUri(ConsumerContext.AccessTokenUri)
        .SignWithToken(requestToken);
    }

    public IConsumerRequest BuildRenewAccessTokenContext(IToken requestToken, string sessionHandle)
    {
        return Request()
          .ForMethod("GET")
          .AlterContext(context => context.SessionHandle = sessionHandle)
          .ForUri(ConsumerContext.AccessTokenUri)
          .SignWithToken(requestToken);
    }

    public string GetUserAuthorizationUrlForToken(IToken token)
    {
      return GetUserAuthorizationUrlForToken(token, null);
    }

    public string GetUserAuthorizationUrlForToken(IToken token, string callbackUrl)
    {
      var builder = new UriBuilder(ConsumerContext.UserAuthorizeUri);

      var collection = new NameValueCollection();

      if (builder.Query != null)
      {
        collection.Add(HttpUtility.ParseQueryString(builder.Query));
      }

      if (_queryParameters != null) collection.Add(_queryParameters);

      collection[Parameters.OAuth_Token] = token.Token;

      if (!string.IsNullOrEmpty(callbackUrl))
      {
        collection[Parameters.OAuth_Callback] = callbackUrl;
      }

      builder.Query = "";

      return builder.Uri + "?" + UriUtility.FormatQueryString(collection);
    }

    public IOAuthSession WithFormParameters(IDictionary<string, string> dictionary)
    {
      return AddItems(_formParameters, dictionary);
    }

    public IOAuthSession WithQueryParameters(IDictionary<string, string> dictionary)
    {
      return AddItems(_queryParameters, dictionary);
    }

    public IOAuthSession WithCookies(IDictionary<string, string> dictionary)
    {
      return AddItems(_cookies, dictionary);
    }

    public IOAuthSession WithHeaders(IDictionary<string, string> dictionary)
    {
      return AddItems(_headers, dictionary);
    }
      
    public IOAuthSession RequiresCallbackConfirmation()
    {
      CallbackMustBeConfirmed = true;
      return this;
    }

    static bool WasCallbackConfimed(NameValueCollection parameters)
    {
      string value = ParseResponseParameter(parameters, Parameters.OAuth_Callback_Confirmed);
      return (value == "true");
    }

    static string ParseResponseParameter(NameValueCollection collection, string parameter)
    {
      string value = (collection[parameter] ?? "").Trim();
      return (value.Length > 0) ? value : null;
    }

    OAuthSession AddItems(NameValueCollection destination, IDictionary<string, string> additions)
    {
      foreach (string parameter in additions.Keys)
      {
        destination[parameter] = additions[parameter];
      }

      return this;
    }
  }
}