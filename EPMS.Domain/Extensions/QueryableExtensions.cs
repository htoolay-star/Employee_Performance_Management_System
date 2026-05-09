using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, string sortDirection)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression body = parameter;

            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            var lambda = Expression.Lambda(body, parameter);

            var methodName = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "OrderByDescending"
                : "OrderBy";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), body.Type },
                source.Expression,
                Expression.Quote(lambda)
            );

            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
