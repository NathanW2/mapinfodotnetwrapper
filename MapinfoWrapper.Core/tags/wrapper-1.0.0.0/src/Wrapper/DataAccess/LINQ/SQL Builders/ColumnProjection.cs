﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace MapinfoWrapper.DataAccess.LINQ.SQLBuilders
{
    internal class ColumnProjection
    {
        internal string Columns;
        internal Expression Selector;
    }
}
