using System.Collections.Generic;
using Mapinfo.Wrapper.DataAccess.Row.Entities;

namespace Mapinfo.Wrapper.DataAccess
{
    public class TableChangeManger
    {
        internal TableChangeManger() { }

        private List<BaseEntity> EntitiesForInsert = new List<BaseEntity>();
        private List<BaseEntity> EntitiesForUpdate = new List<BaseEntity>();
        private List<BaseEntity> EntitiesForDelete = new List<BaseEntity>();

        internal void AddForInsert(BaseEntity entity)
        {
            this.EntitiesForInsert.Add(entity);
        }

        internal void AddForDelete(BaseEntity entity)
        {
            this.EntitiesForDelete.Add(entity);
        }

        internal void AddForUpdate(BaseEntity entity)
        {
            this.EntitiesForUpdate.Add(entity);
        }

        public void ClearInsertes()
        {
            this.EntitiesForInsert.Clear();
        }

        public void ClearUpdates()
        {
            this.EntitiesForUpdate.Clear();
        }

        public void ClearDeletes()
        {
            this.EntitiesForDelete.Clear();
        }

        public EntityChangeSet GetCurrentChangeSet()
        {
            // Return null if there are no entities in the change set.
            if (this.EntitiesForInsert.Count == 0 &&
                this.EntitiesForUpdate.Count == 0 &&
                this.EntitiesForDelete.Count == 0)
            {
                return null;
            }

            EntityChangeSet changeset = new EntityChangeSet(
                this.EntitiesForUpdate.AsReadOnly(),
                this.EntitiesForInsert.AsReadOnly(),
                this.EntitiesForDelete.AsReadOnly()
                );
            return changeset;
        }
    }
}
