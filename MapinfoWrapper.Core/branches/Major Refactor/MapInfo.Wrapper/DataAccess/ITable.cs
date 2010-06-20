using System.Collections.Generic;
using Mapinfo.Wrapper.DataAccess.Row.Entities;

namespace Mapinfo.Wrapper.DataAccess
{
    public interface ITable
    {
        string Name { get; }
        int Number { get; }
        IEnumerable<Column> Columns { get; }
        bool IsMappable { get; }
        void Close();
        void Insert(BaseEntity entity);
        void Delete(BaseEntity entity);
        void Update(BaseEntity entity);
        void InsertOnCommit(BaseEntity entity);
        void DeleteOnCommit(BaseEntity entity);
        void UpdateOnCommit(BaseEntity entity);
        Table<TEntity> ToGenericTable<TEntity>()
            where TEntity : BaseEntity, new();
    }
}
