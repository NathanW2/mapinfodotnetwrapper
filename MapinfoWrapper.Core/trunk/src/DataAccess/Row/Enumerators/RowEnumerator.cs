using MapInfo.Wrapper.DataAccess;
using MapInfo.Wrapper.DataAccess.Entities;
using System.Collections.Generic;

namespace MapInfo.Wrapper.DataAccess.Row.Enumerators
{
    class RowEnumerator<T> : IEnumerator<T>
        where T : BaseEntity, new()
    {
        private IMapInfoDataReader datareader;
        private T current;
        private readonly EntityMaterializer entityfactory;

        public RowEnumerator(IMapInfoDataReader recordSelector, EntityMaterializer entityFactory)
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
