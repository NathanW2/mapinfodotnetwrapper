using MapinfoWrapper.DataAccess.Entities;

namespace MapinfoWrapper.DataAccess.RowOperations.Enumerators
{
    using System.Collections.Generic;

    class RowEnumerator<T> : IEnumerator<T>
        where T : BaseEntity, new()
    {
        private IDataReader datareader;
        private T current;
        private readonly EntityMaterializer entityfactory;

        public RowEnumerator(IDataReader recordSelector, EntityMaterializer entityFactory)
        {
            this.datareader = recordSelector;
            this.entityfactory = entityFactory;
        }

        #region IEnumerator<MapinfoRow> Members

        public T Current
        {
            get
            {
                return this.current;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Reset();
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public bool MoveNext()
        {
            this.datareader.FetchNext();

            if (this.datareader.EndOfTable())
                return false;

            this.current = this.entityfactory.GenerateEntityForIndex<T>(this.datareader.CurrentRecord);
            return true;
        }

        public void Reset()
        {
            this.datareader.Fetch(0);
        }

        #endregion
    }
}
