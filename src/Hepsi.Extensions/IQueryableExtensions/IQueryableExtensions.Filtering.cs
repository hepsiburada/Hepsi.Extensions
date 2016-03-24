using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Hepsi.Extensions.Exceptions;
using Hepsi.Extensions.IQueryableExtensions.Filtering;

namespace Hepsi.Extensions.IQueryableExtensions
{
    public static partial class IQueryableExtensions
    {
        public static IQueryable<T> DynamicFiltering<T>(this IQueryable<T> source, string propertyName, string value)
        {
            var documentType = typeof(T);
            var propertyInfo = documentType
                .GetProperty(propertyName,
                    BindingFlags.Instance
                    | BindingFlags.IgnoreCase
                    | BindingFlags.Public);

            if (propertyInfo == null)
            {
                throw new PropertyNotFoundException(propertyName);
            }

            var parameterExpression = Expression.Parameter(documentType, "document");
            var memberAccessExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
            var convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
            var constantExpression = Expression.Constant(convertedValue, propertyInfo.PropertyType);
            var equalsExpression = Expression.Equal(memberAccessExpression, constantExpression);
            var lambdaExpression = Expression.Lambda<Func<T, bool>>(equalsExpression, parameterExpression);

            return source.Where(lambdaExpression);
        }

        public static IQueryable<T> DynamicFiltering<T>(this IQueryable<T> source, IEnumerable<FilterByRequest> filterByRequests)
        {
            if (filterByRequests != null)
            {
                filterByRequests.ToList().ForEach(filter => source = source.DynamicFiltering(filter.Name, filter.Value));
            }
            return source;
        }
    }
}
