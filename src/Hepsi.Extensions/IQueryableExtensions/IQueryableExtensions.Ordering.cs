using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Hepsi.Extensions.IQueryableExtensions.Ordering;

namespace Hepsi.Extensions.IQueryableExtensions
{
    public static partial class IQueryableExtensions
    {
        public static IQueryable<T> DynamicOrderBy<T>(this IQueryable<T> source, string propertyName, string direction)
        {
            OrderByDirection orderByDirection;

            switch (direction.ToLower())
            {
                case "ascending":
                    orderByDirection = OrderByDirection.Ascending;
                    break;
                case "descending":
                    orderByDirection = OrderByDirection.Descending;
                    break;
                default:
                    return source;
            }

            return source.DynamicOrderBy(new OrderByRequest(propertyName, orderByDirection));
        }

        public static IQueryable<T> DynamicOrderBy<T>(this IQueryable<T> source, OrderByRequest orderByRequest)
        {
            var result = GetMemberAccessExpressionAndSubPropertyType<T>(orderByRequest.PropertyName);

            var orderByExpression = result.Item2;
            var subPropertyType = result.Item1;

            if (orderByExpression == null || subPropertyType == null)
            {
                return source;
            }

            var queryableType = typeof(Queryable);
            var methodName = RetrieveOrderByMethodName(orderByRequest.Direction);
            Type[] typeArguments = { typeof(T), GetPropertyInfo(subPropertyType, orderByRequest.PropertyName.Split('.').Last()).PropertyType };
            Expression[] arguments = { source.Expression, Expression.Quote(orderByExpression) };

            var orderByCallExpression = Expression.Call(queryableType, methodName, typeArguments, arguments);

            foreach (var thenByRequest in orderByRequest.ThenByRequests)
            {
                var thenByExpression = GetMemberAccessExpressionAndSubPropertyType<T>(thenByRequest.Key).Item2;

                if (thenByExpression == null)
                {
                    break;
                }

                var thenByMethodName = RetrieveThenByMethodName(thenByRequest.Value);
                var thenByTypeArguments = new[]
                {
                    typeof(T),
                    GetPropertyInfo(typeof(T),thenByRequest.Key).PropertyType
                };
                var thenByArguments = new Expression[]
                {
                    orderByCallExpression,
                    Expression.Quote(thenByExpression)
                };

                orderByCallExpression = Expression.Call(queryableType, thenByMethodName, thenByTypeArguments, thenByArguments);
            }

            return source.Provider.CreateQuery<T>(orderByCallExpression);
        }


        private static Tuple<Type, LambdaExpression> GetMemberAccessExpressionAndSubPropertyType<T>(
            string propertyName)
        {
            var type = typeof(T);

            if (string.IsNullOrEmpty(propertyName))
            {
                return new Tuple<Type, LambdaExpression>(null, null);
            }

            var parameterExpression = Expression.Parameter(type, "doc");

            var propertyList = propertyName.Split('.');
            var lastPropertyName = propertyList.Last();

            Expression lastParsedPropertyExpression = parameterExpression;
            Expression subPropertyExpression = Expression.Parameter(type, "doc");

            foreach (var property in propertyList)
            {
                try
                {
                    lastParsedPropertyExpression = Expression.PropertyOrField(lastParsedPropertyExpression, property);
                }
                catch (ArgumentException)
                {
                    return new Tuple<Type, LambdaExpression>(null, null);
                }

                if (property != lastPropertyName)
                {
                    subPropertyExpression = lastParsedPropertyExpression;
                }
            }

            var lambdaExpression = Expression.Lambda(lastParsedPropertyExpression, parameterExpression);

            return new Tuple<Type, LambdaExpression>(subPropertyExpression.Type, lambdaExpression);
        }

        private static PropertyInfo GetPropertyInfo(Type documentType, string propertyName)
        {
            return documentType
                .GetProperty(propertyName,
                    BindingFlags.Instance
                    | BindingFlags.IgnoreCase
                    | BindingFlags.Public);
        }

        private static string RetrieveThenByMethodName(OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                ? "ThenBy"
                : "ThenByDescending";
        }

        private static string RetrieveOrderByMethodName(OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                ? "OrderBy"
                : "OrderByDescending";
        }
    }
}
