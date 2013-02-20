﻿#region License

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
using DevDefined.OAuth.Framework;

namespace DevDefined.OAuth.Storage.Basic
{
    /// <summary>
    /// Simple request token model, this provides information about a request token which has been issued, including
    /// who it was issued to, if the token has been used up (a request token should only be presented once), and 
    /// the associated access token (if a user has granted access to a consumer i.e. given them access).
    /// </summary>
    public class RequestToken : TokenBase
    {
        [Obsolete("This parameter is not used", true)]
        public bool AccessDenied { get; set; }

		[Obsolete("This parameter is not used", true)]
        public bool UsedUp { get; set; }

        public AccessToken AccessToken { get; set; }

        public string CallbackUrl { get; set; }

        public string Verifier { get; set; }

        public override string ToString()
        {
            string formattedToken = base.ToString();

            formattedToken += "&" + Parameters.OAuth_Callback_Confirmed + "=true";

            return formattedToken;
        }
    }
}