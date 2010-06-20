using System.Linq.Expressions;

namespace Mapinfo.Wrapper.DataAccess.LINQ.SQL
{
    internal class ColumnProjection
    {
        internal string Columns;
        internal Expression Selector;
    }
}
