using System.Data;

namespace MapInfo.Wrapper.DataAccess.Row
{
    public interface IMapInfoDataReader
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
