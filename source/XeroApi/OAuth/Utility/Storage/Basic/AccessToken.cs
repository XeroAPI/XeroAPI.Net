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
using DevDefined.OAuth.Framework;

namespace DevDefined.OAuth.Storage.Basic
{
  /// <summary>
  /// Simple access token model, this would hold information required to enforce policies such as expiration, and association
  /// with a user accout or other information regarding the information the consumer has been granted access to.
  /// </summary>
  public class AccessToken : TokenBase
  {
	[Obsolete("This parameter is not used", true)]
    public string UserName { get; set; }

	[Obsolete("This parameter is not used", true)]
    public string[] Roles { get; set; }

    public string ExpiresIn { get; set; }

    public string SessionHandle { get; set; }

    public string SessionExpiresIn { get; set; }

    public DateTime CreatedDateUtc { get; set; }
      
    public TimeSpan TokenTimespan
    {
        get
        {
            return string.IsNullOrEmpty(ExpiresIn) 
                ? TimeSpan.Zero 
                : TimeSpan.FromSeconds(double.Parse(ExpiresIn));
        }
    }

      public TimeSpan SessionTimespan
      {
          get
          {
              return string.IsNullOrEmpty(SessionExpiresIn)
                  ? TimeSpan.Zero
                  : TimeSpan.FromSeconds(double.Parse(SessionExpiresIn));              
          }
      }

      public DateTime? ExpiryDateUtc
      {
          get
          {
              if (string.IsNullOrEmpty(ExpiresIn))
              {
                  return null;
              }

              double expiresInSeconds = double.Parse(ExpiresIn);
              return CreatedDateUtc.AddSeconds(expiresInSeconds);
          }
      }

      public DateTime? SessionExpiryDateUtc
      {
          get
          {
              if (string.IsNullOrEmpty(SessionExpiresIn))
              {
                  return null;
              }

              double expiresInSeconds = double.Parse(SessionExpiresIn);
              return CreatedDateUtc.AddSeconds(expiresInSeconds);              
          }
      }

      public bool? HasExpired()
      {
          // By default, the access token should have more than 15 seconds before it has 'expired'.
          return HasExpired(new TimeSpan(0, 0, 15));
      }

      public bool? HasExpired(TimeSpan safeMarginTimespan)
      {
          // If we don't have an expiry date, we can't determine if the AccessToken has expired or not
          if (!ExpiryDateUtc.HasValue)
          {
              return null;
          }

          // Take off the safety margin to the token expiry date.
          DateTime safeExpiryDateUtc = ExpiryDateUtc.Value.Subtract(safeMarginTimespan);

          return (DateTime.UtcNow > safeExpiryDateUtc);
      }

      public bool CanRefresh
      {
          get { return !string.IsNullOrEmpty(SessionHandle); }
      }
  }
}