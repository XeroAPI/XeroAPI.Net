using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevDefined.OAuth.Consumer;

namespace Xero.ScreencastWeb.Models
{
    public class ApiGetRequest<TModel>
        where TModel : ModelBase, new()
    {

        public string ResourceName
        {
            get
            {
                TModel model = new TModel();
                return model.ApiEndpointName;
            }
        }

        public string ResourceId
        {
            get; 
            set;
        }

        public string WhereClause
        {
            get; 
            set;
        }

        public string OrderByClause
        {
            get; 
            set;
        }

        public DateTime? ModifiedSinceDate
        {
            get; 
            set;
        }

        public bool RequiresQuerystring
        {
            get
            {
                return !string.IsNullOrEmpty(WhereClause) || !string.IsNullOrEmpty(OrderByClause);
            }
        }

        public string ToQuerystring()
        {
            Dictionary<string, string> querystringItems = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(WhereClause))
            {
                querystringItems.Add("where", WhereClause);
            }

            if (!string.IsNullOrEmpty(OrderByClause))
            {
                querystringItems.Add("order", OrderByClause);
            }

            return querystringItems
                .Select(pair => UrlEncode(pair.Key) + "=" + UrlEncode(pair.Value))
                .Aggregate((p1, p2) => p1 + "&" + p2);
        }

        public string UrlEncode(string input)
        {
            return HttpUtility.UrlEncode(input);
        }

        public void ApplyModifiedSinceDate(IConsumerRequest consumerRequest)
        {
            // Set the If-Modified-Since http header - if specified
            if (ModifiedSinceDate != null)
            {
                string modifiedSinceString = ModifiedSinceDate.Value.ToString("u");
                consumerRequest.Context.Headers.Add("If-Modified-Since", modifiedSinceString);
            }
        }

    }
}
