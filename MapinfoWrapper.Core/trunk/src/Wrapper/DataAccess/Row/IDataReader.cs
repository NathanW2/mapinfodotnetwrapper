namespace MapinfoWrapper.DataAccess.RowOperations
{
    public interface IDataReader
    {
        bool Read();
        string GetName(int index);
        void Fetch(int recordIndex);
        void FetchLast();
        void FetchNext();
        void FetchFirst();
        bool EndOfTable();
        int GetColumnCount();
        object Get(string p);
        TEntity PopulateEntity<TEntity>(TEntity obj);
    }
}
