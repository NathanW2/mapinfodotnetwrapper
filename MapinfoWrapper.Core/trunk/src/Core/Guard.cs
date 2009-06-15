﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.TableOperations.RowOperations;
using System.Diagnostics;
using System.Threading;
using MapinfoWrapper.TableOperations.RowOperations.Entities;

namespace MapinfoWrapper.Core
{
    public static class Guard
    {
    	[DebuggerStepThrough]
    	public static void AgainstNullOrEmpty(string argument, string name)
    	{
            if (String.IsNullOrEmpty(argument)) 
            {
                throw new ArgumentNullException(name, "{0} can not be null".FormatWith(name));
            }
    	}
    	
        [DebuggerStepThrough]
        public static void AgainstNull(object @object,string name)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(name, "{0} can not be null".FormatWith(name));
            }
        }

        [DebuggerStepThrough]
        public static void AgainstNotInserted<TTableDef>(TTableDef entity, string name)
            where TTableDef : BaseEntity
        {
            if (entity.RowId == 0)
            {
                throw new ArgumentOutOfRangeException(name, entity.RowId, "Row has not been inserted into a table");
            }
        }

        [DebuggerStepThrough]
        public static void AgainstIsZero(int value,string name)
        {
            if (value == 0)
            {
                throw new ArgumentException("Value can not be zero", name);
            }
        }
    }
}
