namespace MapinfoWrapper.DataAccess
{
    internal interface ITable
    {
        string Name { get; }
        int Number { get; }
        bool IsMappable { get; }
        void Close();
    }
}
