﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.DataAccess.RowOperations;

namespace MapinfoWrapper.DataAccess
{
    /// <summary>
    /// Represents a column in a Mapinfo table.
    /// </summary>
    public class Column
    {
        public virtual string Name { get; set; }
        public virtual int Number { get; set; }
        public virtual ColumnTypes Type { get; set; }
        public virtual Table Table { get; set; }
    }
}
