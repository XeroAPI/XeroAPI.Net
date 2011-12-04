using System;
using System.Collections.Specialized;

namespace XeroApi.Integration
{
    public interface IApiQueryDescription
    {
        /// <summary>
        /// Gets the element name, used to construct the request url
        /// </summary>
        /// <value>The name of the element.</value>
        string ElementName { get; }

        /// <summary>
        /// Gets the element id, used to construct the request url
        /// </summary>
        /// <value>The element id.</value>
        string ElementId { get; }

        /// <summary>
        /// Gets the date used to populate the If-Modified-Since http header.
        /// </summary>
        /// <value>The updated since date.</value>
        DateTime? UpdatedSinceDate { get; }

        /// <summary>
        /// Gets the query string parameter collection.
        /// </summary>
        /// <value>The query string params.</value>
        NameValueCollection QueryStringParams { get; }
    }
}
