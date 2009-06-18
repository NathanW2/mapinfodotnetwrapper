using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.TableOperations;

namespace MapinfoWrapper.Core.Internals
{
    internal class TableCommandRunner : ITableCommandRunner
    {
        private IMapinfoWrapper wrapper;

        public TableCommandRunner() 
            : this(IoC.IoC.Resolve<IMapinfoWrapper>())
        { }

        public TableCommandRunner(IMapinfoWrapper mapinfo)
        {
            this.wrapper = mapinfo;
        }

        public string GetName(int tableNumber)
        {
            int enumValue = (int)TableInfo.Name;
            string command = "TableInfo(0,{0})".FormatWith(enumValue);
            return this.wrapper.Evaluate(command);
        }

        public void OpenTable(string tablePath)
        {
            this.wrapper.RunCommand("Open Table {0}".FormatWith(tablePath.InQuotes()));
        }
    }
}
