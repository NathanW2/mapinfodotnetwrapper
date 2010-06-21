using System;
using System.Linq;
using Mapinfo.Wrapper.DataAccess.LINQ;
using System.Linq.Expressions;
using System.Reflection;

namespace Mapinfo.Wrapper.Core.Extensions
{
    public static class LINQExtensions
    {
        public static IQueryable<T> Into<T>(this IQueryable<T> source, string tableName)
        {
            Guard.AgainstNull(source, "source");
            Guard.AgainstNull(tableName, "Table name");

            ConstantExpression table = Expression.Constant(tableName);
            MethodInfo method = ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T));
            MethodCallExpression call = Expression.Call(null, method, new[] { source.Expression, table });
            return source.Provider.CreateQuery<T>(call);
        }

        public static string ToQueryString<T>(this IQueryable<T> source)
        {
            if (source.Provider is MapinfoProvider)
            {
                MapinfoProvider provider = source.Provider as MapinfoProvider;
                return provider.GetQueryString(source.Expression);
            }
            else
            {
                throw new ArgumentOutOfRangeException("source", source.Provider.GetType().Name, 
                                                      "Expected MapinfoProvider but was {0}".FormatWith(source.Provider.GetType().Name));
            }
        }
    }
}
