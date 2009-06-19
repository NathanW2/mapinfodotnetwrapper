﻿using System;
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
        private readonly IMapinfoWrapper wrapper;
        
        public TableCommandRunner() 
            : this(IoC.IoC.Resolve<IMapinfoWrapper>())
        { }

        public TableCommandRunner(IMapinfoWrapper mapinfo)
        {
            this.wrapper = mapinfo;
        }

        public string GetName(int tableNumber)
        {
        	return (String)this.RunTableInfo(tableNumber.ToString(), TableInfo.Name);
        }

        public void OpenTable(string tablePath)
        {
            this.wrapper.RunCommand("Open Table {0}".FormatWith(tablePath.InQuotes()));
        }
        
        public virtual object RunTableInfo(string tableName, TableInfo attribute)
        {
        	int enumvalue = (int)attribute;
        	string command = "TableInfo({0},{1})".FormatWith(tableName,enumvalue);
        	string value = this.wrapper.Evaluate(command);
        	switch (attribute) {
        		case TableInfo.Name:
        			return value;
        		default:
        			return value;
        	}
        }
    }
}