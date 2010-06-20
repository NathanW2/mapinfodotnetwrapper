﻿namespace Mapinfo.Wrapper.DataAccess.Row
{
    public interface IDataReader
    {
        bool Read();
        int CurrentRecord { get; }
        void Fetch(int recordIndex);
        void FetchLast();
        void FetchNext();
        void FetchFirst();
        bool EndOfTable();
        object Get(string p);
    }
}
