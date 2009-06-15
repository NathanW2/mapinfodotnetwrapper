using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.TableOperations.LINQ;
using Wrapper.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Wrapper.Extensions
{
    public static class LINQExtensions
    {
        public static string ToQueryString<T>(this IQueryable<T> source)
        {
            if (source.Provider is MapinfoQueryProvider)
            {
                MapinfoQueryProvider provider = source.Provider as MapinfoQueryProvider;
                return provider.GetQueryString(source.Expression);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Source provider is not a Mapinfo Query Provider");
            }
        }

        public static IQueryable<T> Into<T>(this IQueryable<T> source, string tableName)
        {
            Guard.AgainstNull(source, "source");
            Guard.AgainstNull(tableName, "Table name");

            ConstantExpression table = Expression.Constant(tableName);
            MethodInfo method = ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T));
            MethodCallExpression call = Expression.Call(null, method, new Expression[] { source.Expression, table });
            return source.Provider.CreateQuery<T>(call);
        }

    }
}
