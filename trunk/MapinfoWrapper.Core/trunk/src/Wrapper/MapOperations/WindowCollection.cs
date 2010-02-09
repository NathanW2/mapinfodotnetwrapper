using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.MapOperations
{
    public class WindowCollection : IEnumerable<Window>
    {
        private readonly MapinfoSession mapinfo;
        public WindowCollection(MapinfoSession session)
        {
            this.mapinfo = session;
        }

        public IEnumerator<Window> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
