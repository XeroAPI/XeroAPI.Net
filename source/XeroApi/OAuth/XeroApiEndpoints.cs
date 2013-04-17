using System;

namespace XeroApi.OAuth
{
    public class XeroApiEndpoints
    {
        /*
        public static Uri UserAuthorizeUri = new Uri("https://api.xero.com/oauth/Authorize");

        public static Uri PublicRequestTokenUri = new Uri("https://api.xero.com/oauth/RequestToken");
        public static Uri PartnerRequestTokenUri = new Uri("https://api-partner.network.xero.com/oauth/RequestToken");

        public static Uri PublicAccessTokenUri = new Uri("https://api.xero.com/oauth/AccessToken");
        public static Uri PartnerAccessTokenUri = new Uri("https://api-partner.network.xero.com/oauth/AccessToken");

        public static Uri PublicBaseEndpointUri = new Uri("https://api.xero.com/");
        public static Uri PartnerBaseEndpointUri = new Uri("https://api-partner.network.xero.com/");*/

        public static Uri UserAuthorizeUri = new Uri("http://api.web/oauth/Authorize");
        
        public static Uri PublicRequestTokenUri = new Uri("http://api.web/oauth/RequestToken");
        public static Uri PublicAccessTokenUri = new Uri("http://api.web/oauth/AccessToken");        
        public static Uri PublicBaseEndpointUri = new Uri("http://api.web/");

        public static Uri PartnerRequestTokenUri = new Uri("http://api-partner.web/oauth/RequestToken");
        public static Uri PartnerAccessTokenUri = new Uri("http://api-partner.web/oauth/AccessToken");
        public static Uri PartnerBaseEndpointUri = new Uri("http://api-partner.web/");
    }
}
