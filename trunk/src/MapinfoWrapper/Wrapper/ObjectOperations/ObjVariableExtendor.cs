using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.TableOperations.RowOperations;

namespace Wrapper.Wrapper.ObjectOperations
{
    public class ObjVariableExtendor
    {
        private IDataReader reader;
        private int index;

        public ObjVariableExtendor(IDataReader dataReader,int index)
        {
            this.reader = dataReader;
            this.index = index;
        }

        public string ObjectExpression
        {
            get 
            {
                this.reader.Fetch(this.index);
                return this.reader.GetTableAndRowString("obj");  
            }
        }
    }
}
