namespace MapinfoWrapper.DataAccess.LINQ
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public sealed class LinqQuery<T> : IQueryResult<T>, IQueryable<T>
    {
        public LinqQuery(IQueryProvider provider, Expression expression)
        {
            this.provider = provider;
            this.expression = expression;
        }

        public IEnumerator<T> GetEnumerator()
        {
            IQueryable<T> query = (IQueryable<T>)this;
            return ((IEnumerable<T>)query.Provider.Execute(query.Expression))
                                                 .GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        private Expression expression;
        Expression IQueryable.Expression
        {
            get { return this.expression; }
        }

        private IQueryProvider provider;
        IQueryProvider IQueryable.Provider 
        {
            get { return this.provider; }
        }

        public string GetQueryString()
        {
            IQueryable<T> query = (IQueryable<T>)this;
            return ((MapinfoProvider)query.Provider).GetQueryString(query.Expression);
        }

        public override string ToString()
        {
            return this.GetQueryString();
        }

    }
}