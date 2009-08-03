namespace MapinfoWrapper.Core.Extensions
{
    using System;
    using System.Linq;
    using MapinfoWrapper.DataAccess.LINQ;
    using MapinfoWrapper.Core;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class LINQExtensions
    {
		#region Methods (2) 

		// Public Methods (2) 

        public static IQueryable<T> Into<T>(this IQueryable<T> source, string tableName)
        {
            Guard.AgainstNull(source, "source");
            Guard.AgainstNull(tableName, "Table name");

            ConstantExpression table = Expression.Constant(tableName);
            MethodInfo method = ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T));
            MethodCallExpression call = Expression.Call(null, method, new Expression[] { source.Expression, table });
            return source.Provider.CreateQuery<T>(call);
        }

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

		#endregion Methods 
    }
}
