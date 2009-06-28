using System;
using System.Collections.Generic;
using System.Linq;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo.Internals;

namespace MapinfoWrapper.Mapinfo
{
    public class MapinfoFactory
    {
        protected virtual DMapInfo CreateMapinfoInstance()
        {
            Type mapinfotype = Type.GetTypeFromProgID("Mapinfo.Application");
            DMapInfo instance = (DMapInfo)Activator.CreateInstance(mapinfotype);
            return instance;
        }

        public COMMapinfo CreateCOMInstance()
        {
            DMapInfo instance = CreateMapinfoInstance();
            COMMapinfo olemapinfo = new COMMapinfo(instance);

            Bootstrapper.WireUp(olemapinfo);

            return olemapinfo; 
        }
    }
}
