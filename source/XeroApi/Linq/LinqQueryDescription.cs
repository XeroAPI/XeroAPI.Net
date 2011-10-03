using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using XeroApi.Integration;
using XeroApi.Model;

namespace XeroApi.Linq
{
    public enum ApiQuerystringName
    {
        Unknown,
        Where,
        OrderBy
    }

    
    public class LinqQueryDescription : IApiQueryDescription
    {
        private readonly StringBuilder _orderQuery = new StringBuilder();
        private readonly StringBuilder _whereQuery = new StringBuilder();

        private NameValueCollection _queryStringParams;

        public Type ElementType 
        { 
            get; 
            internal set; 
        }

        internal string ClientSideExpression
        {
            get; 
            set;
        }

        internal Type ElementListType
        {
            get
            {
                if (ElementType == null)
                {
                    throw new MissingFieldException("The ElementType property has not been set. The ElementListType property cannot be read.");
                }

                return ModelTypeHelper.GetElementCollectionType(ElementType);
            }
        }
        
        public string ElementName 
        { 
            get 
            {
                return ElementType == null ? null : ElementType.Name;
            }
        }

        public PropertyInfo ElementIdProperty
        {
            get
            {
                return ElementType == null
                    ? null
                    : ElementType.GetProperties().FirstOrDefault(prop => prop.HasAttribute(typeof(ItemIdAttribute)));
            }
        }

        public PropertyInfo ElementNumberProperty
        {
            get
            {
                return ElementType == null 
                    ? null 
                    : ElementType.GetProperties().FirstOrDefault(prop => prop.HasAttribute(typeof (ItemNumberAttribute)));
            }
        }

        public PropertyInfo ElementUpdatedDateProperty
        {
            get
            {
                return ElementType == null
                    ? null
                    : ElementType.GetProperties().FirstOrDefault(prop => prop.HasAttribute(typeof(ItemUpdatedDateAttribute)));    
            }
        }

        public string ElementId
        {
            get; 
            set;
        }

        public DateTime? UpdatedSinceDate
        {
            get; 
            set;
        }
        
        public string Where
        {
            get { return _whereQuery.ToString(); }
        }
        
        public string Order 
        {
            get { return _orderQuery.ToString(); }
        }

        public NameValueCollection QueryStringParams
        {
            get 
            {
                NameValueCollection collectionToReturn = new NameValueCollection();

                if (_queryStringParams != null && _queryStringParams.Count > 0)
                    collectionToReturn.Add(_queryStringParams);

                if (!string.IsNullOrEmpty(Where))
                    collectionToReturn.Add("WHERE", Where);

                if (!string.IsNullOrEmpty(Order))
                    collectionToReturn.Add("ORDER", Order);

                return collectionToReturn; 
            }
        }
        

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(ElementName))
                sb.Append("Name:" + ElementName + " ");

            if (!string.IsNullOrEmpty(ElementId))
                sb.Append("Id:" + ElementId + " ");

            if (!string.IsNullOrEmpty(Where))
                sb.Append("Where:" + Where + " ");

            if (!string.IsNullOrEmpty(Order))
                sb.Append("Order:" + Order + " ");

            if (UpdatedSinceDate.HasValue)
                sb.Append("After:" + UpdatedSinceDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") + " ");

            return sb.ToString().Trim();
        }

        public void AppendTerm(string term, ApiQuerystringName querystringName)
        {
            switch (querystringName)
            {
                case ApiQuerystringName.Where:

                    if (term == "(" && _whereQuery.Length > 0)
                        _whereQuery.Append(" AND ");

                    _whereQuery.Append(term);
                    
                    break;

                case ApiQuerystringName.OrderBy:

                    if (_orderQuery.Length > 0 && term.Trim() != "DESC")
                        _orderQuery.Append(", ");

                    _orderQuery.Append(term);
                    break;
                    
                case ApiQuerystringName.Unknown:
                    throw new NotImplementedException("Not sure what to do with: " + term);

            }
        }
    }
}
