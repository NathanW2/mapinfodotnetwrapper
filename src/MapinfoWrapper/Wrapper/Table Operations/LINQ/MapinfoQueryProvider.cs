﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MapinfoWrapper.TableOperations.RowOperations;
using System.Linq.Expressions;
using System.Diagnostics;
using MapinfoWrapper.TableOperations.LINQ;
using MapinfoWrapper.TableOperations.LINQ.SQLBuilders;
using MapinfoWrapper.TableOperations.RowOperations.Enumerators;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.TableOperations.LINQ
{
    public class MapinfoQueryProvider : IQueryProvider
    {
        private IMapinfoWrapper wrapper;
        private String tablename;

        public MapinfoQueryProvider(IMapinfoWrapper wrapper, string tableName)
        {
            this.wrapper = wrapper;
            this.tablename = tableName;
        }

        public object Execute(Expression expression)
        {
            TranslateResult result = this.Translate(expression);
            this.wrapper.RunCommand(result.CommandText);
            Type elementType = TypeSystem.GetElementType(expression.Type);
            if (result.Projector != null)
            {
                IDataReader selector = new DataReader(this.wrapper, result.TableName);
                Delegate projector = result.Projector.Compile( );
                return Activator.CreateInstance(typeof(ProjectionReader<>).MakeGenericType(elementType),
                                                new object[] { selector, projector });
            }
            else
            {
                return Activator.CreateInstance(typeof(RowList<>).MakeGenericType(elementType),
                                                new object[] { this.wrapper, result.TableName });
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
