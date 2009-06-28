using MapinfoWrapper.DataAccess.RowOperations;
namespace MapinfoWrapper.MapbasicOperations
{
    public class TableObjVariable : Variable
    {
        private readonly int rowid;
        private readonly IDataReader reader;

        public TableObjVariable(IDataReader reader,int rowId)
            : this(null,VariableType.Object,true)
        {
            this.reader = reader;
            this.rowid = rowId;
        }

        private TableObjVariable(string name, VariableType declareAs, bool isAssigned) 
            : base(name, declareAs, isAssigned)
        { }

        public override string GetExpression()
        {
            this.reader.Fetch(this.rowid);
            return this.reader.GetTableAndRowString("obj"); 
        }
    }
}