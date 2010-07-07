using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.DataAccess.RowOperations;

namespace MapinfoWrapper.DataAccess.Row.Entities
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
