using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Core.Wrappers
{
    public class TableInfoWrapper
    {
        private readonly IMapinfoWrapper mapinfo;

        public TableInfoWrapper(IMapinfoWrapper miSession)
        {
            Guard.AgainstNull(miSession, "MISession");

            this.mapinfo = miSession;
        }

        public string GetTableInfo(string tableName,TableInfo attribute)
        {
            int enumvalue = (int)attribute;
            string command = "TableInfo({0},{1})".FormatWith(tableName, enumvalue);
            string value = mapinfo.Eval(command);
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
