namespace MapinfoWrapper.DataAccess.RowOperations.Enumerators
{
    using System.Collections.Generic;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;

    public class RowEnumerator<T> : IEnumerator<T>
        where T : BaseEntity, new()
    {
        private IDataReader datareader;
        private T current;

        public RowEnumerator(IDataReader recordSelector)
        {
            this.datareader = recordSelector;
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

            T instance = new T();

            this.current = this.datareader.PopulateEntity(instance);
            return true;
        }

        public void Reset()
        {
            this.datareader.Fetch(0);
        }

        #endregion
    }
}
