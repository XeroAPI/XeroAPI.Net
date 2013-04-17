using System;
using System.Collections.Generic;
using System.Linq;

using System.Linq.Expressions;
using System.Collections;

namespace XeroApi.Linq
{
    //public class ApiSingleQuery<T> : IQueryable<T>
    //{
    //    private readonly QueryProvider _provider;
    //    private readonly Expression _expression;

    //    public ApiSingleQuery(QueryProvider provider)
    //    {
    //        if (provider == null)
    //        {
    //            throw new ArgumentNullException("provider");
    //        }

    //        _provider = provider;
    //        _expression = Expression.Constant(this);
    //    }

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        return ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return ((IEnumerable)_provider.Execute(_expression)).GetEnumerator();
    //    }

    //    Expression IQueryable.Expression
    //    {
    //        get { return _expression; }
    //    }
        
    //    Type IQueryable.ElementType
    //    {
    //        get { return typeof(T); }
    //    }

    //    IQueryProvider IQueryable.Provider
    //    {
    //        get { return _provider; }
    //    }

    //    public override string ToString()
    //    {
    //        return _provider.GetQueryText(_expression);
    //    }
    //}

    public class ApiQuery<T> : IOrderedQueryable<T>
    {
        private readonly QueryProvider _provider;
        private readonly Expression _expression;

        public ApiQuery(QueryProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            _provider = provider;
            _expression = Expression.Constant(this);
        }

        public ApiQuery(QueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
            
            _provider = provider;
            _expression = expression;
        }

        Expression IQueryable.Expression
        {
            get { return _expression; }
        }

        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_provider.Execute(_expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_provider.Execute(_expression)).GetEnumerator();
        }

        public override string ToString()
        {
            return _provider.GetQueryText(_expression);
        }
    }
}
