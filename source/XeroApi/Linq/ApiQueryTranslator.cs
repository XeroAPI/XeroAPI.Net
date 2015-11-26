using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace XeroApi.Linq
{
    /// <summary>
    /// Translates a linq query into a <c ref="LinqQueryDescription" />.
    /// </summary>
    internal class ApiQueryTranslator : ExpressionVisitor
    {
        private LinqQueryDescription _query;
        private ApiQuerystringName _currentQueryStringName = ApiQuerystringName.Unknown;


        /// <summary>
        /// Translates the specified linq expression into a <c ref="LinqQueryDescription" />.
        /// </summary>
        /// <param name="expression">The linq expression.</param>
        /// <returns></returns>
        internal LinqQueryDescription Translate(Expression expression)
        {
            _query = new LinqQueryDescription();

            if (expression.Type.IsGenericType)
            {
                _query.ElementType = expression.Type.GetGenericArguments()[0];
            }
            
            Visit(expression);

            return _query;
        }
        
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            switch (m.Method.Name)
            {
                case "Where":

                    using (new QuerystringScope(this, ApiQuerystringName.Where))
                    {
                        return base.VisitMethodCall(m);
                    }

                case "FirstOrDefault":
                case "First":
                case "SingleOrDefault":
                case "Single":

                    if (!string.IsNullOrEmpty(_query.ClientSideExpression) && !_query.ClientSideExpression.Equals(m.Method.Name))
                    {
                        throw new NotImplementedException("Only 1 aggregator expression can currently be performed.");
                    }

                    _query.ClientSideExpression = m.Method.Name;

                    using (new QuerystringScope(this, ApiQuerystringName.Where))
                    {
                        return base.VisitMethodCall(m);
                    }

                case "Count":

                    if (!string.IsNullOrEmpty(_query.ClientSideExpression) && !_query.ClientSideExpression.Equals(m.Method.Name))
                    {
                        throw new NotImplementedException("Only 1 aggregator expression can currently be performed.");
                    }

                    _query.ClientSideExpression = m.Method.Name;
                    return base.VisitMethodCall(m);
                    
                case "OrderBy":
                case "ThenBy":

                    using (new QuerystringScope(this, ApiQuerystringName.OrderBy))
                    {
                        return base.VisitMethodCall(m);
                    }
                    
                case "OrderByDescending":
                case "ThenByDescending":

                    using (new QuerystringScope(this, ApiQuerystringName.OrderBy))
                    {
                        var expression = base.VisitMethodCall(m);
                        Append(" DESC");
                        return expression;
                    }
                
                case "Skip":
                    using (new QuerystringScope(this, ApiQuerystringName.Skip))
                    {
                        return base.VisitMethodCall(m);
                    }
            }

            var rootExpressionType = FindRootExpressionType(m);

            switch (rootExpressionType)
            {
                case ExpressionType.Constant:
                    Append(EvaluateToLiteral(m));
                    return m;

                case ExpressionType.Parameter:
                    Append(ParseExpression(m));
                    return m;
            }

            // If this is a method from a clr object, as opposed to an extension method,  the API server just might be able to support it.
            if (m.Method.DeclaringType == typeof(string) || 
                m.Method.DeclaringType == typeof(DateTime) || 
                m.Method.DeclaringType == typeof(Guid))
            {
                return VisitObjectMethodCall(m);
            }

            throw new NotImplementedException(string.Format("The method '{0}' can't currently be used in a XeroApi WHERE querystring.", m.Method.Name));
        }


        protected Expression VisitObjectMethodCall(MethodCallExpression m)
        {
            // The .Contains and .StartsWith methods on string objects are supported by the API server
            // e.g.
            //      c => c.Name.StartsWith("Jason")
            //      c => c.Name.Contains("ase")

            if (m.Method.IsStatic && m.Method.DeclaringType != null)
            {
                Append(m.Method.DeclaringType.Name);
            }

            Expression obj = Visit(m.Object);
            
            Append(".");
            Append(m.Method.Name);
            Append("(");
            var args = VisitExpressionList(m.Arguments);
            Append(")");

            return UpdateMethodCall(m, obj, m.Method, args);
        }


        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    Append("(");
                    Visit(u.Operand);
                    Append(" == false)");
                    break;
                default:
                    Visit(u.Operand);
                    break;
            }

            return u;
        }


        protected override Expression VisitLambda(LambdaExpression lambda)
        {
            // If there WHERE clause is (user => user.IsSubscriber), this should be translated to (user => user.IsSubscriber == true)
            // TODO: Need to guard against this being run when parsing an ORDERBY clause
            if (lambda.Body is MemberExpression && (lambda.Body.Type == typeof(bool)))
            {
                return base.Visit(Expression.Equal(lambda.Body, Expression.Constant(true)));
            }

            if (lambda.Body is MemberExpression && (lambda.Body.Type == typeof(Enum)))
            {
                System.Diagnostics.Debug.WriteLine("We have an place to do something!");
            }

            return base.VisitLambda(lambda);
        }


        /// <summary>
        /// Determines whether the expression is to be rendered into the WHERE or ORDER clause
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// 	<c>true</c> if [is not rendered into expression] [the specified expression]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNotRenderedIntoExpression(Expression expression)
        {
            var binaryExpression = expression as BinaryExpression;

            if (binaryExpression == null) 
                return false;

            var mExp = binaryExpression.Left as MemberExpression;

            // Check if the LHS is an ItemId, ItemNumber or UpdatedDate. If so, record away from the main where clause.
            if (mExp != null && mExp.Member.DeclaringType != null && mExp.Member.DeclaringType.Name == _query.ElementName)
            {
                if (mExp.Member.Name == _query.ElementIdProperty.SafeName()
                || mExp.Member.Name == _query.ElementNumberProperty.SafeName()
                || mExp.Member.Name == _query.ElementUpdatedDateProperty.SafeName())
                {
                    return true;
                }
            }

            return false;
        }


        protected override Expression VisitBinary(BinaryExpression b)
        {
            var mExp = b.Left as MemberExpression;

            // Check if the LHS is an ItemId, ItemNumber or UpdatedDate. If so, record away from the main where clause.
            if (mExp != null && mExp.Member.DeclaringType != null && mExp.Member.DeclaringType.Name == _query.ElementName)
            {
                if (mExp.Member.Name == _query.ElementIdProperty.SafeName())
                {
                    if (b.Right.Type == typeof (Guid?))
                        _query.ElementId = EvaluateExpression<Guid?>(b.Right).ToString();
                    else if (b.Right.Type == typeof (Guid))
                        _query.ElementId = EvaluateExpression<Guid>(b.Right).ToString();
                    return b;
                }
                if (mExp.Member.Name == _query.ElementNumberProperty.SafeName())
                {
                    var rightValue = EvaluateExpression<object>(b.Right);
                    
                    if (rightValue != null)
                    {
                        _query.ElementId = rightValue.ToString();
                        return b;
                    }                    
                }
                if (mExp.Member.Name == _query.ElementUpdatedDateProperty.SafeName())
                {
                    if (b.Right.Type == typeof (DateTime?))
                        _query.UpdatedSinceDate = EvaluateExpression<DateTime?>(b.Right);
                    else if (b.Right.Type == typeof (DateTime))
                        _query.UpdatedSinceDate = EvaluateExpression<DateTime>(b.Right);
                    return b;
                }
            }

            if (b.Left.NodeType == ExpressionType.Convert)
            {
                var expression = b.Left as UnaryExpression;

                if (expression != null)
                {
                    var member = expression.Operand as MemberExpression;

                    if (member != null)
                    {
                        if (member.Type.IsEnum)
                        {
                            // The enum needs to be treated as a string
                            var enumInt = EvaluateExpression<int>(b.Right);
                            object enumValue = Enum.ToObject(member.Type, enumInt);
                            string enumString = Enum.GetName(member.Type, enumInt);

                            Append("(");
                            VisitMemberAccess(member);
                            AppendOperator(b);
                            VisitConstant(Expression.Constant(enumString));
                            Append(")");

                            return Expression.Equal(member, Expression.Constant(enumValue));
                        }
                    }
                }
            }
            
            // http://answers.xero.com/developer/question/39411/
            // Check for rogue VB methods that have been slipped into the linq expression..
            var leftMethod = b.Left as MethodCallExpression;

            if (leftMethod != null && leftMethod.Method.Name == "CompareString")
            {
                var memberExpression = leftMethod.Arguments[0];
                var valueExpression = leftMethod.Arguments[1];
                
                if (b.NodeType == ExpressionType.NotEqual)
                    return Visit(Expression.NotEqual(memberExpression, valueExpression));
                
                return Visit(Expression.Equal(memberExpression, valueExpression));
            }
            
            // Check if either the left or right hand side of the expression is a ItemId, ItemNumber or ElementUpdatedDate
            // binary expression. If so, visit the left and right hand sides separately.
            if (IsNotRenderedIntoExpression(b.Left))
            {
                VisitBinary((BinaryExpression)b.Left);
                Visit(b.Right);
                return b.Right;
            }  
            if (IsNotRenderedIntoExpression(b.Right)) 
            {
                Visit(b.Left);
                VisitBinary((BinaryExpression)b.Right);
                return b.Left;
            } 

            // Parse as a normal binary expression (operand1 operator operand2)
            Append("(");
            Visit(b.Left);

            AppendOperator(b);

            Visit(b.Right);
            Append(")");

            return b;
        }

        private void AppendOperator(BinaryExpression b)
        {
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    Append(" AND ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    Append(" == ");
                    break;
                case ExpressionType.NotEqual:
                    Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            var q = c.Value as IQueryable;

            if (q != null)
            {
                _query.ElementType = q.ElementType;
            }
            else if (c.Value == null)
            {
                Append("NULL");
            }
            else if (c.Value.GetType().IsEnum)
            {
                Append(c.Value.GetType().Name + "." + c.Value);
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        Append(((bool) c.Value) ? "true" : "false");
                        break;

                    case TypeCode.String:
                        Append("\"");
                        Append(c.Value.ToString());
                        Append("\"");
                        break;

                    case TypeCode.DateTime:
                        return c;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

                    default:
                    {
                        Append(c.Value.ToString());
                        break;
                    }
                }
            }

            return c;
        }

        protected override NewExpression VisitNew(NewExpression nex)
        {
            Append(EvaluateToLiteral(nex));
            return nex;
        }
        
        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression == null)
            {
                throw new NotSupportedException("The MemberExpression.Expression property is null");
            }

            ExpressionType rootExpressionType = FindRootExpressionType(m);

            if (rootExpressionType == ExpressionType.Constant)
            {
                Append(EvaluateToLiteral(m));
                return m;
            }

            if (rootExpressionType == ExpressionType.Parameter)
            {
                Append(ParseExpression(m));
                return m;
            }
            
            if (m.Expression.NodeType == ExpressionType.MemberAccess)
            {
                Append(ParseExpression(m));
                return m;
            }
            
            throw new NotSupportedException(string.Format("The member '{0}' of type {1} is not supported", m.Expression, m.Expression.NodeType));
        }

        private static ExpressionType FindRootExpressionType(Expression m)
        {
            if (m == null)
                throw new NullReferenceException("Parameter 'm' cannot be null");

            switch (m.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return FindRootExpressionType(((MemberExpression) m).Expression);

                case ExpressionType.Call:
                    
                    var methodExpression = (MethodCallExpression) m;

                    // check if this is a static method - i.e. no subject
                    if (methodExpression.Object == null)
                        return ExpressionType.Call;
                    
                    return FindRootExpressionType(methodExpression.Object);

                case ExpressionType.Parameter:
                case ExpressionType.Constant:
                    return m.NodeType;

                default:
                    throw new NotSupportedException(string.Format("Expression type {0} was not expected in this scenario", m.NodeType));
            }
        }

        private string ParseExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:

                    var memberExpression = (MemberExpression) expression;

                    string parentExpression = ParseExpression(memberExpression.Expression);
                    return ApplyDotNotation(parentExpression, memberExpression.Member.Name);

                case ExpressionType.Call:

                    var methodCallExpression = (MethodCallExpression) expression;

                    // When selecting the n-th item in a list, use the [n] notation.
                    if (methodCallExpression.Method.Name == "get_Item" && typeof(IList).IsAssignableFrom(methodCallExpression.Object.Type))
                    {
                        string parentExpressionEx = ParseExpression(methodCallExpression.Object);
                        var listIndex = EvaluateExpression<int>(methodCallExpression.Arguments[0]);

                        return string.Concat(parentExpressionEx, "[", listIndex, "]");
                    }

                    var argumentList = methodCallExpression.Arguments
                        .Select(EvaluateToLiteral)
                        .ToArray();

                    string methodCall = string.Concat(methodCallExpression.Method.Name, "(", string.Join(",", argumentList), ")");

                    string parent = (methodCallExpression.Object == null) 
                        ? methodCallExpression.Type.Name 
                        : ParseExpression(methodCallExpression.Object);

                    return ApplyDotNotation(parent, methodCall);

                case ExpressionType.Parameter:

                    // If this is a parameter of the base linq query, it doesn't need to be explicitly added to the output
                    return string.Empty;
                    
                default:
                    throw new NotSupportedException(string.Format("Expression type {0} was not expected in this scenario", expression.NodeType));
            }
        }
      
        private string EvaluateToLiteral(Expression exp)
        {
            switch (exp.Type.Name)
            {
                case "DateTime":
                    var date = EvaluateExpression<DateTime>(exp);

                    if (date.Date == date)
                        return (string.Format("DateTime({0},{1},{2})", date.Year, date.Month, date.Day));
                        
                    return (string.Format("DateTime({0},{1},{2},{3},{4},{5})", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second));

                case "Guid":
                    var guid = EvaluateExpression<Guid>(exp);

                    if (guid == Guid.Empty)
                        return ("Guid.Empty");
                    
                    return (string.Format("Guid(\"{0}\")", guid));

                case "String":
                    var stringValue = EvaluateExpression<string>(exp);

                    if (stringValue == null)
                        return ("null");
                    
                    return string.Format("\"{0}\"", stringValue);

                case "Int32":
                    var shortValue = EvaluateExpression<int>(exp);
                    return string.Format("\"{0}\"", shortValue);

                case "Int64":
                    var longValue = EvaluateExpression<long>(exp);
                    return string.Format("\"{0}\"", longValue);
                case "Boolean":
                    var boolValue = EvaluateExpression<bool>(exp);
                    return string.Format("{0}", boolValue);           
            }

            throw new NotSupportedException(string.Format("The Expression return type '{0}' is not supported", exp.Type.Name));
        }

        private static T EvaluateExpression<T>(Expression expression)
        {
            Expression<Func<T>> lambda = Expression.Lambda<Func<T>>(expression);
            Func<T> func = lambda.Compile();
            return (func).Invoke();
        }
        
        private void Append(string term)
        {
            _query.AppendTerm(term, _currentQueryStringName);
        }

        private static string ApplyDotNotation(params string[] input)
        {
            return input.Where(it => !string.IsNullOrEmpty(it)).Aggregate((s1, s2) => string.Concat(s1, ".", s2));
        }


        private class QuerystringScope : IDisposable
        {
            private readonly ApiQuerystringName _originalQuerystringName;
            private readonly ApiQueryTranslator _queryTranslator;

            public QuerystringScope(ApiQueryTranslator queryTranslator, ApiQuerystringName querystringName)
            {
                _originalQuerystringName = queryTranslator._currentQueryStringName;
                _queryTranslator = queryTranslator;

                _queryTranslator._currentQueryStringName = querystringName;
            }

            public void Dispose()
            {
                _queryTranslator._currentQueryStringName = _originalQuerystringName;
            }
        }

    }
}
