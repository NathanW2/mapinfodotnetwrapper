using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Geometries;
using System.Reflection;
using MapinfoWrapper.TableOperations.RowOperations;

namespace MapinfoWrapper.TableOperations.LINQ.SQLBuilders
{
    public class TranslateResult
    {
        internal string CommandText;
        internal string TableName;
        internal LambdaExpression Projector;
    }

    internal class QueryTranslator : ExpressionVisitor
    {
        StringBuilder sb;
        StringBuilder selectbuilder;
        string tableName = "";
        bool dontadd = false;
        ColumnProjection projection;
        ParameterExpression datareader;

        internal QueryTranslator()
        {
        }

        internal TranslateResult Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.selectbuilder = new StringBuilder();
            this.datareader = Expression.Parameter(typeof(IDataReader), "datareader");
            this.Visit(expression);
            this.tableName = this.tableName.Length == 0 ? "WrapperTempTable" : this.tableName;
            string query = this.selectbuilder.Append(this.sb).ToString() + " INTO {0}".FormatWith(this.tableName);
            return new TranslateResult
            {
                CommandText = query,
                TableName = this.tableName,
                Projector = this.projection != null ?
                Expression.Lambda(this.projection.Selector, this.datareader) : null
            };
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable))
            {
                if (m.Method.Name == "Where")
                {
                    if (this.selectbuilder.Length == 0)
                    {
                        selectbuilder.Append("SELECT * FROM ");
                    }
                    this.Visit(m.Arguments[0]);
                    sb.Append(" WHERE ");
                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    this.Visit(lambda.Body);
                    return m;
                }
                else if (m.Method.Name == "Select")
                {
                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    ColumnProjection projection = new ColumnProjector().ProjectColumns(lambda.Body, this.datareader);
                    this.selectbuilder.Append("SELECT ");
                    this.selectbuilder.Append(projection.Columns);
                    this.selectbuilder.Append(" FROM ");
                    this.Visit(m.Arguments[0]);
                    this.projection = projection;
                    return m;
                }
            }
            if (m.Method.DeclaringType == typeof(LINQExtensions))
            {
                if (m.Method.Name == "Into")
                {
                    ConstantExpression table = (ConstantExpression)m.Arguments[1];
                    this.tableName = (string)table.Value;
                    this.Visit(m.Arguments[0]);
                    return m;
                }
             }
            
            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }
       
        protected override Expression VisitBinary(BinaryExpression b)
        {
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    sb.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    if (this.dontadd == false)
                    {
                        sb.Append(" = ");
                    }
                    this.dontadd = false;
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            this.Visit(b.Right);
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            ITable table = c.Value as ITable;
            if (table != null)
            {
                this.selectbuilder.Append(table.Name);
            }
            else if (c.Value == null)
            {
                sb.Append("".InQuotes());
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? "T" : "F");
                        break;
                    case TypeCode.String:
                        sb.Append(((string)c.Value).InQuotes());
                        break;
                    case TypeCode.Int32:
                        int value = Convert.ToInt32(c.Value);
                        sb.Append(value);
                        break;
                    case TypeCode.DateTime:
                        DateTime date = (DateTime)c.Value;
                        sb.Append(date.ToString().InQuotes());
                        break;
                    case TypeCode.Object:
                        DateTime? datetime = c.Value as DateTime?;
                        if (datetime != null && datetime.HasValue)
                        {
                            switch (datetime.HasValue)
                            {
                                case true:
                                    sb.Append(datetime.Value.ToString().InQuotes());
                                    break;
                                case false:
                                    sb.Append("".InQuotes());
                                    break;
                                default:
                                    break;
                            }
                        }

                        Coordinate? point = c.Value as Coordinate?;
                        if (point != null && point.HasValue)
                        {
                            sb.Replace("{X}", point.Value.X.ToString());
                            sb.Replace("{Y}", point.Value.Y.ToString());
                        }

                        break;
                    default:
                        sb.Append(c.Value);
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression != null)
            {
                switch (m.Expression.NodeType)
                {
                    case ExpressionType.Parameter:
                        sb.Append(m.Member.Name);
                        return m;
                    case ExpressionType.MemberAccess:
                        if (m.Member.Name == "Centroid")
                        {
                             string centroidxstring = "CENTROIDX(obj) = CENTROIDX(CREATEPOINT({X},{Y}))";
                             string centroidystring = "CENTROIDY(obj) = CENTROIDY(CREATEPOINT({X},{Y}))";
                             sb.Append(centroidxstring + " AND " + centroidystring);
                             this.dontadd = true;
                        }
                        return m;
                    default:
                        break;
                }
                
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }
    }
}
