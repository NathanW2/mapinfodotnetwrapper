namespace MapinfoWrapper.DataAccess.LINQ
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using MapinfoWrapper.DataAccess.LINQ.SQLBuilders;
    using MapinfoWrapper.DataAccess.RowOperations;
    using MapinfoWrapper.DataAccess.RowOperations.Enumerators;
    using MapinfoWrapper.Mapinfo;

    internal class MapinfoQueryProvider : IQueryProvider
    {
        private readonly MapinfoSession misession;

        public MapinfoQueryProvider(MapinfoSession MISession)
        {
            this.misession = MISession;
        }

        public object Execute(Expression expression)
        {
            TranslateResult result = this.Translate(expression);
            this.misession.RunCommand(result.CommandText);

            Type elementType = TypeSystem.GetElementType(expression.Type);

            IDataReader reader = new DataReader(misession, result.TableName);
            if (result.Projector != null)
            {
                Delegate projector = result.Projector.Compile();
                return Activator.CreateInstance(typeof(ProjectionReader<>).MakeGenericType(elementType),
                                                new object[] { reader, projector });
            }
            else
            {
                Table table = this.misession.Tables.GetTable(result.TableName);

                return Activator.CreateInstance(typeof(RowList<>).MakeGenericType(elementType),
                                                new object[] { table, reader, this.misession });
            }
        }

        public string GetQueryString(System.Linq.Expressions.Expression expression)
        {
            return this.Translate(expression).CommandText;
        }

        public TranslateResult Translate(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            return new QueryTranslator( ).Translate(expression);
        }

        #region IQueryProvider Members

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            return (IQueryable<TElement>)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType),
                                                                  new object[] { this, expression });
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return this.CreateQuery(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)this.Execute(expression);
        }

        #endregion
    }

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    internal static class TypeSystem
    {

        internal static Type GetElementType(Type seqType)
        {
            Type ienum = FindIEnumerable(seqType);
            if (ienum == null)
                return seqType;
            return ienum.GetGenericArguments( )[0];
        }

        private static Type FindIEnumerable(Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType( ));
            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments( ))
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }

            Type[] ifaces = seqType.GetInterfaces( );

            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null)
                        return ienum;
                }
            }
            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }
            return null;
        }
    }
}
