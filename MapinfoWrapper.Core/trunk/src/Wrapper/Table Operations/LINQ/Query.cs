using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.TableOperations.RowOperations;
using System.Linq.Expressions;

namespace MapinfoWrapper.TableOperations.LINQ
{
    public sealed class Query<T> : IQueryable<T>
    {
        public Query(IQueryProvider provider,Expression expression)
        {
            this.Provider = provider;
            this.Expression = expression;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IQueryable Members

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public System.Linq.Expressions.Expression Expression {get; private set;}

        public IQueryProvider Provider { get; private set; }

        #endregion

        public string ToQueryString()
        {
            return ((MapinfoQueryProvider)this.Provider).GetQueryString(this.Expression);
        }
    }
}
