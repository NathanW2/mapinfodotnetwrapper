using System.Collections.ObjectModel;

namespace Mapinfo.Wrapper.DataAccess.Row.Entities
{
    public class EntityChangeSet
    {
        public ReadOnlyCollection<BaseEntity> ForUpdate { get; private set; }
        public ReadOnlyCollection<BaseEntity> ForInsert { get; private set; }
        public ReadOnlyCollection<BaseEntity> ForDelete { get; private set; }

        public EntityChangeSet(ReadOnlyCollection<BaseEntity> forUpdate,
                               ReadOnlyCollection<BaseEntity> forInsert,
                               ReadOnlyCollection<BaseEntity> forDelete)
        {
            ForUpdate = forUpdate;
            ForInsert = forInsert;
            ForDelete = forDelete;
        }
    }
}