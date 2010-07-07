namespace MapinfoWrapper.DataAccess.LINQ.SQLBuilders
{
    using System.Linq.Expressions;

    internal class ColumnProjection
    {
        internal string Columns;
        internal Expression Selector;
    }
}
