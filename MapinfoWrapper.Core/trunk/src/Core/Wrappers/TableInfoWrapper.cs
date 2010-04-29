using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Core.InfoWrappers
{
    public class TableInfoWrapper
    {
        private readonly MapinfoSession miSession;

        public TableInfoWrapper(MapinfoSession MISession)
        {
            Guard.AgainstNull(MISession,"MISession");

            miSession = MISession;
        }

        public string GetTableInfo(string tableName,TableInfo attribute)
        {
            int enumvalue = (int)attribute;
            string command = "TableInfo({0},{1})".FormatWith(tableName, enumvalue);
            string value = miSession.Eval(command);
            return value;
        }

        public string GetName(string tableName)
        {
            return this.GetTableInfo(tableName,TableInfo.Name);
        }

        public string GetName(int tableNumber)
        {
            return this.GetName(tableNumber.ToString());
        }
        
    }
}
