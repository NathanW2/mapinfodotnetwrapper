using Mapinfo.Wrapper.Core.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mapinfo.Wrapper.DataAccess.Row;

namespace Mapinfo.Wrapper.DataAccess.LINQ.SQL
{
    /// <summary>
    /// The result object that is returned from parsing an expression tree.
    /// </summary>
    internal class TranslateResult
    {
        /// <summary>
        /// The SQL command that needs to run in Mapinfo to get the results.
        /// <para>If this is null, check the <c>TableName</c> because we might just want all the records from that table.</para>
        /// </summary>
        internal string SQLCommand;
        /// <summary>
        /// The resulting table we the results are held.
        /// </summary>
        internal string TableName;

        internal LambdaExpression Projector;
    }
    
    // HACK This class works but is really ugly and hard to work on. Really needs to be refactored.
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
            
            // If the expression is just a constant and is a table. Then we select all from that table.
            // HACK This really should be done better.
            if (expression.NodeType == ExpressionType.Constant && ((ConstantExpression)expression).Value is ITable)
            {
                ITable table = ((ConstantExpression)expression).Value as ITable;

                if (table == null)
                {
                    throw new ArgumentNullException("Could not translate contanst to type ITable");
                }

                return new TranslateResult
                {
                    SQLCommand = null,
                    TableName = table.Name,
                    Projector = null
                };
            }

            this.Visit(expression);

            this.tableName = this.tableName.Length == 0 ? "WrapperTempTable" : this.tableName;

            string query = this.selectbuilder.Append(this.sb).ToString() + " INTO {0}".FormatWith(this.tableName);
            
            return new TranslateResult
            {
                SQLCommand = query,
                TableName = this.tableName,
                Projector = this.projection != null ? Expression.Lambda(this.projection.Selector, this.datareader) : null
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

                        Coordinate point = c.Value as Coordinate;
                        if (point != null)
                        {
                            sb.Replace("{X}", point.X.ToString());
                            sb.Replace("{Y}", point.Y.ToString());
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
