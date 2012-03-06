using System;
using System.IO;
using System.Net;

using DevDefined.OAuth.Framework;

namespace DevDefined.OAuth.Consumer
{
    public interface IConsumerRequest
    {
        IOAuthContext Context { get; }

        [Obsolete("Prefer ToConsumerResponse instead as this has more error handling built in")]
        HttpWebResponse ToWebResponse();
        HttpWebRequest ToWebRequest();

        IConsumerResponse ToConsumerResponse();
        
        RequestDescription GetRequestDescription();
        IConsumerRequest SignWithoutToken();
        IConsumerRequest SignWithToken();
        IConsumerRequest SignWithToken(IToken token, bool checkForExistingSignature = true);

        Uri ProxyServerUri { get; set; }
        string AcceptsType { get; set; }
        string RequestBody { get; set; }
        Stream RequestStream { get; set; }
    }
}