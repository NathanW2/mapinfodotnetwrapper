using System;
using System.Collections.Generic;
using System.Linq;
using MapinfoWrapper.Mapinfo.Internals;
using Microsoft.Win32;

namespace MapinfoWrapper.Mapinfo
{
    public class MapinfoSessionManager
    {
        protected virtual DMapInfo CreateMapinfoInstance()
        {
            Type mapinfotype = Type.GetTypeFromProgID("Mapinfo.Application");
            DMapInfo instance = (DMapInfo)Activator.CreateInstance(mapinfotype);
            return instance;
        }


        /// <summary>
        /// Creates a new instance of Mapinfo and returns a <see cref="COMMapinfo"/>
        /// which contains the instance. 
        /// <para>The returned objet can be passed into objects and
        /// methods that need it in the MapinfoWrapper API.</para>
        /// </summary>
        /// <returns>A new <see cref="COMMapinfo"/> containing the running instance of Mapinfo.</returns>
        public COMMapinfo CreateCOMInstance()
        {
            DMapInfo instance = CreateMapinfoInstance();
            return new COMMapinfo(instance); ; 
        }


        internal COMMapinfo GetCurrentRunningInstance()
        {
            // TODO: Implement getting running instance of Mapinfo.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a <see cref="IEnumerable{T}"/> containing a list of all installed versions
        /// of Mapinfo.
        /// 
        /// <para>This function uses the registry to find installed versions of Mapinfo.</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetInstalledMapinfoVersions()
        {
            string registryKey = @"SOFTWARE\MapInfo\MapInfo\Professional";

            Microsoft.Win32.RegistryKey prokey = Registry.LocalMachine.OpenSubKey(registryKey);

            if (prokey == null)
                return null;

            var versions = from a in prokey.GetSubKeyNames()
                           let r = prokey.OpenSubKey(a)
                           let name = r.Name
                           let slashindex = name.LastIndexOf(@"\")
                           select Convert.ToInt32(name.Substring(slashindex + 1, name.Length - slashindex - 1));
            
            return versions.ToList();
        }
    }
}
