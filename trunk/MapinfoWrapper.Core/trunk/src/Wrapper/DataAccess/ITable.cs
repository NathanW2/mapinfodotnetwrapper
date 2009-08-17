using MapinfoWrapper.DataAccess.RowOperations.Entities;
namespace MapinfoWrapper.DataAccess
{
    internal interface ITable
    {
        string Name { get; }
        int Number { get; }
        bool IsMappable { get; }
        void Close();
        void Insert(BaseEntity entity);
        void Delete(BaseEntity entity);
        void Update(BaseEntity entity);
        void InsertOnCommit(BaseEntity entity);
        void DeleteOnCommit(BaseEntity entity);
        void UpdateOnCommit(BaseEntity entity);
    }
}
