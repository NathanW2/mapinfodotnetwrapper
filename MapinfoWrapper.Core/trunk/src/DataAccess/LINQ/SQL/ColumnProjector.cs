using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MapinfoWrapper.DataAccess.RowOperations;

namespace MapinfoWrapper.DataAccess.LINQ.SQL
{
    internal class ColumnProjector : ExpressionVisitor
    {
        StringBuilder sb;
        ParameterExpression datareader;
        static MethodInfo getvaluemethodinfo;

        internal ColumnProjector()
        {
            if (getvaluemethodinfo == null)
            {
                getvaluemethodinfo = typeof(IDataReader).GetMethod("Get",new Type[] {typeof(string)});
            }
        }

        internal ColumnProjection ProjectColumns(Expression expression, ParameterExpression row)
        {
            this.sb = new StringBuilder();
            this.datareader = row;
            Expression selector = this.Visit(expression);
            return new ColumnProjection { Columns = this.sb.ToString(), Selector = selector };
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                if (this.sb.Length > 0)
                {
                    this.sb.Append(", ");
                }
                this.sb.Append(m.Member.Name);
                return Expression.Convert(Expression.Call(this.datareader, getvaluemethodinfo, Expression.Constant(m.Member.Name)), m.Type);
            }
            else
            {
                return base.VisitMemberAccess(m);
            }
        }
    }
}
