using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.DataAccess.LINQ.SQL
{
    public class ColumnWithData : Column
    {
        public object Value { get; set; }
    }
}
