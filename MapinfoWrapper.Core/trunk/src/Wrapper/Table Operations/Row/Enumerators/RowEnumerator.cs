using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.TableOperations.RowOperations;
using System.Reflection;
using MapinfoWrapper.TableOperations.RowOperations.Entities;

namespace MapinfoWrapper.TableOperations.RowOperations.Enumerators
{
    public class RowEnumerator<T> : IEnumerator<T>
        where T : BaseEntity, new()
    {
        private IDataReader datareader;
        private T current;
        private PropertyInfo[] properties;

        public RowEnumerator(IDataReader recordSelector)
        {
            this.datareader = recordSelector;
            this.properties = typeof(T).GetProperties();
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
            for (int i = 0, n = this.properties.Length; i < n; i++)
            {
                    PropertyInfo fi = this.properties[i];
                    fi.SetValue(instance, this.datareader.Get(fi.Name),null);
            }

            this.current = instance;
            return true;
        }

        public void Reset()
        {
            this.datareader.Fetch(0);
        }

        #endregion
    }
}
