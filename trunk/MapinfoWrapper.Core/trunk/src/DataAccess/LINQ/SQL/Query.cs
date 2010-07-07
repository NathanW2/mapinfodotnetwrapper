using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.DataAccess.LINQ.SQL
{
    public abstract class Query
    {
        public abstract void Excute();
        public abstract string GetQueryString();
    }
}
