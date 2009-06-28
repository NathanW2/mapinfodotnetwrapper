using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.DataAccess.RowOperations
{
    public interface IDataReader
    {
        string GetName(int index);
        void Fetch(int recordIndex);
        void FetchLast();
        void FetchNext();
        void FetchFirst();
        bool EndOfTable();
        int GetColumnCount();

        object Get(string p);

        string GetTableAndRowString(string columnName);
    }
}
