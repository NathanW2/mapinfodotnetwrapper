using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MapinfoWrapper.TableOperations.RowOperations;
using MapinfoWrapper.TableOperations.RowOperations.Entities;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.TableOperations.RowOperations.Enumerators
{
    public class RowList<TTabeDef> : IEnumerable<TTabeDef>
        where TTabeDef : BaseEntity, new()
    {
        private IMapinfoWrapper wrapper;
        private string tablename;

        public RowList(IMapinfoWrapper wrapper,string tableName)
        {
            this.wrapper = wrapper;
            this.tablename = tableName;
        }

        public IEnumerator<TTabeDef> GetEnumerator()
        {
            //RecordSelector<TTabeDef> selector = new RecordSelector<TTabeDef>(this.wrapper, this.tablename);
            //RowEnumerator<TTabeDef> rowenumerator = new RowEnumerator<TTabeDef>(selector);
            DataReader reader = new DataReader(wrapper, tablename);
            RowEnumerator<TTabeDef> rowenumerator = new RowEnumerator<TTabeDef>(reader);
            rowenumerator.Reset();
            return rowenumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
