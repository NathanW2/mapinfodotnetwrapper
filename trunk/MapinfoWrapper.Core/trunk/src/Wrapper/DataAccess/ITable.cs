namespace MapinfoWrapper.DataAccess
{
    public interface ITable
    {
        string Name { get; }
        int Number { get; }
        bool IsMappable { get; }
        void Close();
    }
}
