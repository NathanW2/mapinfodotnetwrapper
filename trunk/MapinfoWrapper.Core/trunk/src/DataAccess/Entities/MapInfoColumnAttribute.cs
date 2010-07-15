using System;
using MapInfo.Wrapper.DataAccess.Row;


namespace MapInfo.Wrapper.DataAccess.Entities
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
