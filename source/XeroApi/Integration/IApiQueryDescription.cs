using System;
using System.Collections.Specialized;

namespace XeroApi.Integration
{
    public interface IApiQueryDescription
    {
        string ElementName { get; }
        string ElementId { get; }
        DateTime? UpdatedSinceDate { get; }
        NameValueCollection QueryStringParams { get; }
    }
}
