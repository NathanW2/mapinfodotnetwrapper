﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Core.IoC;

namespace Wrapper.Mapinfo.Factory
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
            DependencyResolver resolver = new DependencyResolver();
            resolver.Register(typeof(IMapinfoWrapper), olemapinfo);
            IoC.Initialize(resolver);
            return olemapinfo; 
        }
    }
}
