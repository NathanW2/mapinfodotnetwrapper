using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.Wrapper.DataAccess.LINQ.SQL_Builders
{
    class UpdateExpression
    {

        public UpdateExpression(string tableName)
        {
            this.TableName = tableName;
        }

        public string TableName { get; private set; }

    }
}
