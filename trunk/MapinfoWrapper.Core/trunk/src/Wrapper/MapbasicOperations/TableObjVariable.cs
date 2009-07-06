using MapinfoWrapper.DataAccess.RowOperations;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.MapbasicOperations
{
    public class TableObjVariable : Variable
    {
        private readonly int rowid;
        private readonly IDataReader reader;

        internal TableObjVariable(string name, 
                                VariableType declareAs, 
                                bool isAssigned, 
                                MapinfoSession wrapper, 
                                int rowid, 
                                IDataReader reader) 
            : base(name, declareAs, isAssigned, wrapper)
        {
            this.rowid = rowid;
            this.reader = reader;
        }

        public override string GetExpression()
        {
            this.reader.Fetch(this.rowid);
            return this.reader.GetTableAndRowString("obj"); 
        }
    }
}