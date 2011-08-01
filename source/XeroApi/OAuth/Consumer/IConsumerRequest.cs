using System;
using System.IO;
using System.Net;

using DevDefined.OAuth.Framework;

namespace DevDefined.OAuth.Consumer
{
    public interface IConsumerRequest
    {
        IOAuthContext Context { get; }
        
        HttpWebResponse ToWebResponse();
        IConsumerResponse ToConsumerResponse();
        
        RequestDescription GetRequestDescription();
        IConsumerRequest SignWithoutToken();
        IConsumerRequest SignWithToken();
        IConsumerRequest SignWithToken(IToken token);

        Uri ProxyServerUri { get; set; }
        string AcceptsType { get; set; }
        string RequestBody { get; set; }
        Stream RequestStream { get; set; }
    }
}