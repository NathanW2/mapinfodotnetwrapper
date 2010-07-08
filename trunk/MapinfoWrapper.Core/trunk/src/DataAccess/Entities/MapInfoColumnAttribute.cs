using System;
using MapinfoWrapper.DataAccess.RowOperations;

namespace MapinfoWrapper.DataAccess.Entities
{
    public class MapInfoColumnAttribute : Attribute
    {
        public MapInfoColumnAttribute(ColumnType columnType)
        {
            this.Type = columnType;
        }

        public ColumnType Type { get; private set; }
    }
}
