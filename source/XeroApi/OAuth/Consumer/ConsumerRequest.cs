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
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Utility;

namespace DevDefined.OAuth.Consumer
{
    public class ConsumerRequest : IConsumerRequest
    {
        private readonly IOAuthConsumerContext _consumerContext;
        private readonly IOAuthContext _context;
        private readonly IOAuthSession _oauthSession;

        public ConsumerRequest(IOAuthSession oauthSession, IOAuthContext context, IOAuthConsumerContext consumerContext)
        {
            _oauthSession = oauthSession;
            _context = context;
            _consumerContext = consumerContext;
        }

        public IOAuthContext Context
        {
            get { return _context; }
        }

        public IConsumerResponse ToConsumerResponse()
        {
            return _oauthSession.RunConsumerRequest(this);
        }

        public virtual HttpWebRequest ToWebRequest()
        {
            RequestDescription description = GetRequestDescription();

            var request = (HttpWebRequest)WebRequest.Create(description.Url);
            request.Timeout = (int)TimeSpan.FromMinutes(3).TotalMilliseconds;
            request.Method = description.Method;
            request.UserAgent = _consumerContext.UserAgent;

            if (!string.IsNullOrEmpty(AcceptsType))
            {
                request.Accept = AcceptsType;
            }

            DateTime? ifModifiedSinceDate = ParseIfModifiedSince(Context);

            if (ifModifiedSinceDate.HasValue)
                request.IfModifiedSince = ifModifiedSinceDate.Value;

            if (ProxyServerUri != null)
            {
                request.Proxy = new WebProxy(ProxyServerUri, false);
            }

            if (description.ContentType == Parameters.HttpFormEncoded)
            {
                request.ContentType = description.ContentType;

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(description.Body);
                }
            }
            else if (!string.IsNullOrEmpty(description.Body))
            {
                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(description.Body);
                }
            }
            else if (description.RequestStream != null)
            {
                using (var requestStream = request.GetRequestStream())
                {
                    description.RequestStream.CopyTo(requestStream);
                }
            }

            if (description.Headers.Count > 0)
            {
                foreach (string key in description.Headers.AllKeys)
                {
                    request.Headers[key] = description.Headers[key];
                }
            }
            return request;
        }

        public RequestDescription GetRequestDescription()
        {
            if (string.IsNullOrEmpty(_context.Signature))
            {
                _consumerContext.SignContext(_context);
            }


            Uri uri = _context.GenerateUri();

            var description = new RequestDescription
            {
                Url = uri,
                Method = _context.RequestMethod
            };

            if ((_context.FormEncodedParameters != null) && (_context.FormEncodedParameters.Count > 0))
            {
                description.ContentType = Parameters.HttpFormEncoded;
                description.Body =
                    UriUtility.FormatQueryString(_context.FormEncodedParameters.ToQueryParametersExcludingTokenSecret());
            }
            else if (!string.IsNullOrEmpty(RequestBody))
            {
                description.Body = UriUtility.UrlEncode(RequestBody);
            }
            else if (RequestStream != null)
            {
                description.RequestStream = RequestStream;
            }

            if (_consumerContext.UseHeaderForOAuthParameters)
            {
                description.Headers[Parameters.OAuth_Authorization_Header] = _context.GenerateOAuthParametersForHeader();
            }

            return description;
        }

        [Obsolete("Prefer ToConsumerResponse instead as this has more error handling built in")]
        public HttpWebResponse ToWebResponse()
        {
            try
            {
                HttpWebRequest request = ToWebRequest();
                return (HttpWebResponse)request.GetResponse();
            }
            catch (WebException webEx)
            {
                OAuthException authException;

                if (WebExceptionHelper.TryWrapException(Context, webEx, out authException))
                {
                    throw authException;
                }

                throw;
            }
        }

        public IConsumerRequest SignWithoutToken()
        {
            EnsureRequestHasNotBeenSignedYet();
            _consumerContext.SignContext(_context);
            return this;
        }

        public IConsumerRequest SignWithToken()
        {
            var accessToken = _oauthSession.GetAccessToken();
            return SignWithToken(accessToken);
        }

        public IConsumerRequest SignWithToken(IToken token, bool checkForExistingSignature = true)
        {
            if (checkForExistingSignature)
                EnsureRequestHasNotBeenSignedYet();

            _consumerContext.SignContextWithToken(_context, token);
            return this;
        }

        public Uri ProxyServerUri { get; set; }

        public string AcceptsType { get; set; }

        public string RequestBody { get; set; }

        public Stream RequestStream { get; set; }

        public override string ToString()
        {
            return ToConsumerResponse().Content;
        }

        private void EnsureRequestHasNotBeenSignedYet()
        {
            if (!string.IsNullOrEmpty(_context.Signature))
            {
                throw Error.ThisConsumerRequestHasAlreadyBeenSigned();
            }
        }

        public DateTime? ParseIfModifiedSince(IOAuthContext context)
        {
            if (context.IfModifiedSince.HasValue)
            {
                AssertValidIfModifiedSinceDate(context.IfModifiedSince.Value);
                return context.IfModifiedSince.Value;
            }

            string ifModifiedSinceString = context.Headers["If-Modified-Since"];

            if (string.IsNullOrEmpty(ifModifiedSinceString))
                return null;

            DateTime ifModifiedSinceDate;

            if (DateTime.TryParse(ifModifiedSinceString, out ifModifiedSinceDate))
            {
                AssertValidIfModifiedSinceDate(ifModifiedSinceDate);
                return ifModifiedSinceDate;
            }

            return null;
        }

        public void AssertValidIfModifiedSinceDate(DateTime date)
        {
            if (date < new DateTime(1753, 01, 01))
            {
                throw Error.IfModifiedSinceHeaderOutOfRange(date);
            }
        }
    }
}