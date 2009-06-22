using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo.Internals;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.TableOperations;

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

            DependencyResolver resolver = new DependencyResolver();
            resolver.Register(typeof(IMapinfoWrapper), olemapinfo);
            IoC.Initialize(resolver);

            TableCommandRunner tablerunner = new TableCommandRunner();
            VariableFactory varfactory = new VariableFactory();

            resolver.Register(typeof(ITableCommandRunner),tablerunner);
            resolver.Register(typeof(IVariableFactory),varfactory);

            TableFactory tableFactory = new TableFactory();

            resolver.Register(typeof(ITableCommandRunner), tableFactory);
            
            return olemapinfo; 
        }
    }
}
