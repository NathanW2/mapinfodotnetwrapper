using System.Linq.Expressions;

namespace MapinfoWrapper.DataAccess.LINQ.SQL
{
    internal class ColumnProjection
    {
        internal string Columns;
        internal Expression Selector;
    }
}
