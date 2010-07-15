﻿using MapInfo.Wrapper.DataAccess.Row;


namespace MapInfo.Wrapper.DataAccess
{
    /// <summary>
    /// Represents a column in a Mapinfo table.
    /// </summary>
    public class Column
    {
        public virtual string Name { get; set; }
        public virtual int Number { get; set; }
        public virtual ColumnType Type { get; set; }
        public virtual ITable Table { get; set; }
    }
}
