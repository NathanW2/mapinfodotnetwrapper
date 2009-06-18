using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.TableOperations;
using MapinfoWrapper.TableOperations.RowOperations.Entities;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.Internals;

namespace MapinfoWrapper.TableOperations
{
    public class TableFactory
    {
        ITableCommandRunner tablerunner;

        public TableFactory()
        {
            tablerunner = IoC.Resolve<ITableCommandRunner>();
        }

        public ITable OpenTable(string tablePath)
        {
            tablerunner.OpenTable(tablePath);
            string name = tablerunner.GetName(0);
            return new Table(name);
        }

        public ITable<T> OpenTable<T>(string tablePath)
            where T : BaseEntity, new()
        {
            tablerunner.OpenTable(tablePath);
            string name = tablerunner.GetName(0);
            return new Table<T>(tablePath);
        }
    }
}
