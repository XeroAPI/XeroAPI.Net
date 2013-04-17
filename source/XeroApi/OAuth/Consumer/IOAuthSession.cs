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
using DevDefined.OAuth.Storage.Basic;

namespace DevDefined.OAuth.Consumer
{
    public interface IOAuthSession
    {
        IOAuthConsumerContext ConsumerContext { get; set; }
        ITokenRepository TokenRepository { get; }

        IConsumerRequest Request();

        RequestToken GetRequestToken();
        RequestToken GetRequestToken(Uri callbackUri);

        AccessToken GetAccessToken();
        bool HasValidAccessToken { get; }

        IMessageLogger MessageLogger { get; set; }
        IConsumerResponse LogMessage(IConsumerRequest request, IConsumerResponse response);

        AccessToken ExchangeRequestTokenForAccessToken(string verificationCode);

        string GetUserAuthorizationUrl();

        IOAuthSession WithFormParameters(IDictionary<string, string> dictionary);
        IOAuthSession WithQueryParameters(IDictionary<string, string> dictionary);
        IOAuthSession WithCookies(IDictionary<string, string> dictionary);
        IOAuthSession WithHeaders(IDictionary<string, string> dictionary);

        AccessToken RenewAccessToken();

        IConsumerResponse RunConsumerRequest(IConsumerRequest consumerRequest);
    }
}