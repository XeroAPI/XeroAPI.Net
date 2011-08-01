using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace XeroApi.Linq
{
    public abstract class QueryProvider : IQueryProvider
    {

        IQueryable<T> IQueryProvider.CreateQuery<T>(Expression expression)
        {
            return new ApiQuery<T>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);

            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(ApiQuery<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        T IQueryProvider.Execute<T>(Expression expression)
        {
            return (T)this.Execute(expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        public abstract string GetQueryText(Expression expression);

        public abstract object Execute(Expression expression);
                
    }
}
