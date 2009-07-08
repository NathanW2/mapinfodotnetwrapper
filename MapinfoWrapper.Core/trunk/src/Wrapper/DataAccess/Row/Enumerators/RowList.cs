﻿using System.Collections.Generic;
using System.Collections;
using MapinfoWrapper.DataAccess.RowOperations.Entities;

namespace MapinfoWrapper.DataAccess.RowOperations.Enumerators
{
    public class RowList<TTabeDef> : IEnumerable<TTabeDef>
        where TTabeDef : BaseEntity, new()
    {
        private readonly string tablename;
        private readonly IDataReader reader;

        public RowList(string tableName, IDataReader reader)
        {
            this.tablename = tableName;
        }

        public IEnumerator<TTabeDef> GetEnumerator()
        {
            RowEnumerator<TTabeDef> rowenumerator = new RowEnumerator<TTabeDef>(this.reader);
            rowenumerator.Reset();
            return rowenumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
