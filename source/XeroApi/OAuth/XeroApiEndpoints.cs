using System;

namespace XeroApi.OAuth
{
    public class XeroApiEndpoints
    {
        public static Uri PublicRequestTokenUri = new Uri("https://api.xero.com/oauth/RequestToken");
        public static Uri PartnerRequestTokenUri = new Uri("https://api.xero.com/oauth/RequestToken");

        public static Uri UserAuthorizeUri = new Uri("https://api.xero.com/oauth/Authorize");

        public static Uri PublicAccessTokenUri = new Uri("https://api.xero.com/oauth/AccessToken");
        public static Uri PartnerAccessTokenUri = new Uri("https://api.xero.com/oauth/AccessToken");

        public static Uri PublicBaseEndpointUri = new Uri("https://api.xero.com/api.xro/2.0/");
        public static Uri PartnerBaseEndpointUri = new Uri("https://api.xero.com/api.xro/2.0/");
    }
    
}
