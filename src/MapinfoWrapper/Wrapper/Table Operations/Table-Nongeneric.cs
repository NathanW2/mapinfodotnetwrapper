using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.TableOperations.RowOperations;
using MapinfoWrapper.TableOperations.RowOperations.Entities;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.TableOperations
{
    /// <summary>
    /// A Mapinfo Table container which can be used when the
    /// table definition is not known. This will still give you strong access
    /// to the RowId column.
    /// </summary>
    public class Table : Table<BaseEntity>
    {
        public Table(string tableName)
            : this(IoC.Resolve<IMapinfoWrapper>(), tableName)
        { }

        /// <summary>
        /// Creates a new Table object used to gather information
        /// about a specifed table.
        /// </summary>
        /// <param name="wrapper">A wrapper object containing the running instace of mapinfo.</param>
        /// <param name="tablename">The name of the table for which to gather information about.</param>
        public Table(IMapinfoWrapper wrapper, string tablename)
            : base(wrapper,tablename)
        {}
    }
}


