using System;
using System.Collections;
using System.Collections.Generic;

namespace Mapinfo.Wrapper.DataAccess.Row.Enumerators
{
    internal class ProjectionReader<T> : IEnumerable<T>
    {
        Enumerator enumerator;

        public ProjectionReader(IDataReader reader, Func<IDataReader, T> projector)
        {
            this.enumerator = new Enumerator(reader, projector);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Enumerator e = this.enumerator;
            if (e == null)
            {
                throw new InvalidOperationException("Cannot enumerate more than once");
            }
            this.enumerator = null;
            e.Reset();
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            IDataReader reader;
            T current;
            Func<IDataReader, T> projector;

            internal Enumerator(IDataReader reader, Func<IDataReader, T> projector)
            {
                this.reader = reader;
                this.projector = projector;
            }

            public T Current
            {
                get { return this.current; }
            }

            object IEnumerator.Current
            {
                get { return this.current; }
            }

            public bool MoveNext()
            {
                this.reader.FetchNext();
                if (!this.reader.EndOfTable())
                {
                    this.current = this.projector(this.reader); 
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                this.reader.Fetch(0);
            }

            public void Dispose()
            {
                this.reader.Fetch(0);
            }
        }
    }
}
