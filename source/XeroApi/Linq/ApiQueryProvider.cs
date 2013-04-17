using System;
using System.Linq.Expressions;

using XeroApi.Integration;
using XeroApi.Model;
using XeroApi.Model.Serialize;

namespace XeroApi.Linq
{
    public class ApiQueryProvider<TResponse> : QueryProvider
        where TResponse : ResponseBase
    {
        private readonly IIntegrationProxy _proxy;
        private readonly IModelSerializer _serializer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiQueryProvider{TResponse}"/> class.
        /// </summary>
        /// <param name="proxy">The integration proxy to use.</param>
        /// <param name="serializer"></param>
        public ApiQueryProvider(IIntegrationProxy proxy, IModelSerializer serializer)
        {
            _proxy = proxy;
            _serializer = serializer;
        }

        public override string GetQueryText(Expression expression)
        {
            return Translate(expression).ToString();
        }

        public override object Execute(Expression expression)
        {
            LinqQueryDescription queryDescription = Translate(expression);

            // Call the API..
            string data = _proxy.FindElements(queryDescription);

            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var response = _serializer.DeserializeTo<TResponse>(data);

            // The payroll API has some return types which are not collections
            if (queryDescription.ElementListType == queryDescription.ElementType)
            {
                return response == null
                    ? Activator.CreateInstance(queryDescription.ElementType)
                    : response.GetSingleTypedProperty(queryDescription.ElementType);
            }

            // Guard against an empty response..
            IModelList elementCollection = (response == null)
                ? (IModelList)Activator.CreateInstance(queryDescription.ElementListType)        // TODO: too much going on here, needs tidying up
                : response.GetTypedProperty(queryDescription.ElementListType);

            if (queryDescription.ClientSideExpression == null)
            {
                return elementCollection;
            }
            
            switch (queryDescription.ClientSideExpression)
            {
                case "FirstOrDefault":

                    return elementCollection.Count == 0 
                        ? null 
                        : elementCollection[0];

                case "First":

                    if (elementCollection.Count == 0)
                        throw new InvalidOperationException("The ModelList contains no items");
                    
                    return elementCollection[0];

                case "SingleOrDefault":

                    if (elementCollection.Count > 1)
                        throw new InvalidOperationException("The ModelList contains no items");

                    if (elementCollection.Count == 0)
                        return null;

                    return elementCollection[0];

                case "Single":

                    if (elementCollection.Count != 1)
                        throw new InvalidOperationException("The ModelList contains no items");
                    
                    return elementCollection[0];

                case "Count":

                    return elementCollection.Count;

                default:

                    throw new NotImplementedException(string.Format("The client side aggregator {0} cannot currently be performed", queryDescription.ClientSideExpression));
            }
        }

        private static LinqQueryDescription Translate(Expression expression)
        {
            return new ApiQueryTranslator().Translate(expression);
        }
    }
}
