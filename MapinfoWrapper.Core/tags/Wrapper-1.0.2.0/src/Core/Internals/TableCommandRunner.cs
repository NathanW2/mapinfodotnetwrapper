using System;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess;

namespace MapinfoWrapper.Core.Internals
{
    internal class TableCommandRunner : ITableCommandRunner
    {
        private readonly IMapinfoWrapper wrapper;

        public TableCommandRunner() : this(null)
        { }

        public TableCommandRunner(IMapinfoWrapper mapinfo)
        {
            this.wrapper = mapinfo ?? ServiceLocator.GetInstance<IMapinfoWrapper>();
        }

        public string GetName(int tableNumber)
        {
            return this.GetName(tableNumber.ToString());
        }

        public string GetName(string tableName)
        {
            return (String) this.RunTableInfo(tableName, TableInfo.Name);
        }

        public void OpenTable(string tablePath)
        {
            this.wrapper.RunCommand("Open Table {0}".FormatWith(tablePath.InQuotes()));
        }

        public string GetPath(string tableName)
        {
            return (string) this.RunTableInfo(tableName, TableInfo.Tabfile);
        }

        public virtual object RunTableInfo(string tableName, TableInfo attribute)
        {
        	int enumvalue = (int)attribute;
        	string command = "TableInfo({0},{1})".FormatWith(tableName,enumvalue);
        	string value = this.wrapper.Evaluate(command);
        	switch (attribute) {
                case TableInfo.Tabfile:
        		case TableInfo.Name:
        			return value;
        		default:
        			return value;
        	}
        }

        public void RunCommand(string command)
        {
            this.wrapper.RunCommand(command);
        }

        public string Evaluate(string command)
        {
            return this.wrapper.Evaluate(command);
        }
    }
}
