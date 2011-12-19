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
        OrderBy,
        Skip
    }

    
    public class LinqQueryDescription : IApiQueryDescription
    {
        private readonly StringBuilder _orderQuery = new StringBuilder();
        private readonly StringBuilder _whereQuery = new StringBuilder();
        private readonly StringBuilder _skipQuery = new StringBuilder();

        private string _lastWhereTerm;


        public Type ElementType 
        { 
            get; 
            internal set; 
        }

        public string ClientSideExpression
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

        /// <summary>
        /// Gets the element name, used to construct the request url
        /// </summary>
        /// <value>The name of the element.</value>
        public string ElementName 
        { 
            get 
            {
                return ElementType == null ? null : ElementType.Name;
            }
        }

        /// <summary>
        /// Gets the property from the <c ref="ElementType"/> which is marked as <c ref="ItemIdAttribute"/>.
        /// </summary>
        /// <value>The element id property.</value>
        public PropertyInfo ElementIdProperty
        {
            get
            {
                return ElementType == null
                    ? null
                    : ElementType.GetProperties().FirstOrDefault(prop => prop.HasAttribute(typeof(ItemIdAttribute)));
            }
        }

        /// <summary>
        /// Gets the property from the <c ref="ElementType"/> which is marked as <c ref="ItemNumberAttribute"/>.
        /// </summary>
        /// <value>The element number property.</value>
        public PropertyInfo ElementNumberProperty
        {
            get
            {
                return ElementType == null 
                    ? null 
                    : ElementType.GetProperties().FirstOrDefault(prop => prop.HasAttribute(typeof (ItemNumberAttribute)));
            }
        }

        /// <summary>
        /// Gets the property from the <c ref="ElementType"/> which is marked as <c ref="ItemUpdatedDateAttribute"/>.
        /// </summary>
        /// <value>The element updated date property.</value>
        public PropertyInfo ElementUpdatedDateProperty
        {
            get
            {
                return ElementType == null
                    ? null
                    : ElementType.GetProperties().FirstOrDefault(prop => prop.HasAttribute(typeof(ItemUpdatedDateAttribute)));    
            }
        }

        /// <summary>
        /// Gets the element id, used to construct the request url
        /// </summary>
        /// <value>The element id.</value>
        public string ElementId
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets the date used to populate the If-Modified-Since http header.
        /// </summary>
        /// <value>The updated since date.</value>
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

        public string Offset
        {
            get { return _skipQuery.ToString(); }
        }

        /// <summary>
        /// Gets the query string parameter collection.
        /// </summary>
        /// <value>The query string params.</value>

        public NameValueCollection QueryStringParams
        {
            get 
            {
                NameValueCollection collectionToReturn = new NameValueCollection();
                
                if (!string.IsNullOrEmpty(Where))
                    collectionToReturn.Add("WHERE", Where);

                if (!string.IsNullOrEmpty(Order))
                    collectionToReturn.Add("ORDER", Order);

                if (!string.IsNullOrEmpty(Offset))
                    collectionToReturn.Add("offset", Offset);

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

            if (!string.IsNullOrEmpty(Offset))
                sb.Append("Offset:" + Offset + " ");

            if (UpdatedSinceDate.HasValue)
                sb.Append("After:" + UpdatedSinceDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") + " ");

            return sb.ToString().Trim();
        }



        /// <summary>
        /// Appends the <c ref="term"/> to either the <c ref="Where"/> or <c ref="Order"/> clause.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="querystringName">Name of the querystring.</param>
        public void AppendTerm(string term, ApiQuerystringName querystringName)
        {
            switch (querystringName)
            {
                case ApiQuerystringName.Where:

                    if (_lastWhereTerm == ")" && term == "(")
                        _whereQuery.Append(" AND ");

                    _whereQuery.Append(term);

                    _lastWhereTerm = term;

                    break;

                case ApiQuerystringName.OrderBy:

                    if (_orderQuery.Length > 0 && term.Trim() != "DESC")
                        _orderQuery.Append(", ");

                    _orderQuery.Append(term);
                    
                    break;

                case ApiQuerystringName.Skip:
                    _skipQuery.Remove(0, _skipQuery.Length);
                    _skipQuery/*.Append("offset=")*/.Append(term);
                    break;
                    
                case ApiQuerystringName.Unknown:
                    throw new NotImplementedException("Not sure what to do with: " + term);

            }
        }
    }
}
