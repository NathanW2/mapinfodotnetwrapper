using System.Linq.Expressions;

namespace MapInfo.Wrapper.DataAccess.LINQ.SQL
{
    internal class ColumnProjection
    {
        internal string Columns;
        internal Expression Selector;
    }
}
