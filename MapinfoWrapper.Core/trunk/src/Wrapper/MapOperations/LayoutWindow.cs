using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.MapOperations
{
    public class LayoutWindow: Window 
    {
        public LayoutWindow(int windowID, MapinfoSession mapinfo)
            : base(windowID, mapinfo)
        { }
    }
}
