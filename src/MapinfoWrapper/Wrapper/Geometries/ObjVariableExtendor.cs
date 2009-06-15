using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.TableOperations.RowOperations;
using MapinfoWrapper.MapbasicOperations;

namespace MapinfoWrapper.Geometries
{
    public class ObjVariableExtender : IVariableExtender
    {
        private IDataReader reader;
        private int index;

        public ObjVariableExtender(IDataReader dataReader,int index)
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
