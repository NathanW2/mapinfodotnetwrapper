using System;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.Embedding;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.MapOperations;

namespace MapinfoWrapper.Mapinfo
{
    using System.Collections.Generic;
    using System.Linq;
    using Internals;
    using Microsoft.Win32;

    public class MapinfoSession : IMapinfoWrapper
    {
        private readonly IMapinfoWrapper mapinfo;

        private MapinfoSession(IMapinfoWrapper mapinfoAPI)
        {
            this.mapinfo = mapinfoAPI;
        }

        private static DMapInfo CreateMapinfoInstance()
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
        public static MapinfoSession CreateCOMInstance()
        {
            DMapInfo instance = CreateMapinfoInstance();
            COMMapinfo mapinfo = new COMMapinfo(instance);
            return new MapinfoSession(mapinfo);
        }

        internal static MapinfoSession GetCurrentRunningInstance()
        {
            // TODO: Implement getting running instance of Mapinfo.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Run the ObjectInfo mapbasic command in Mapinfo, and returns a string containing the result.
        /// </summary>
        /// <param name="variable">The variable used by the ObjectInfo call.</param>
        /// <param name="attribute">The attribute specifying which information to return.</param>
        /// <returns>A string containing the retured result from calling the ObjectInfo command in Mapinfo.</returns>
        // This doesn't belong here.
        internal object ObjectInfo(IVariable variable, ObjectInfoEnum attribute)
        {
            string expression = variable.GetExpression();
            string returnedstring = this.Evaluate("ObjectInfo({0},{1})".FormatWith(expression, (int)attribute));
            return returnedstring;
        }

        /// <summary>
        /// Opens a workspace in a instance of Mapinfo.
        /// </summary>
        /// <param name="workspacePath">The path to the workspace which needs to be opened.</param>
        /// <returns>A instance of a <see cref="Workspace"/> which can be used to get infomation about the opened workspace.</returns>
        public Workspace OpenWorkspace(string workspacePath)
        {
            this.RunCommand("Run Application {0}".FormatWith(workspacePath.InQuotes()));
            this.tables.RefreshList();
            return new Workspace();
        }

        /// <summary>
        /// Gets the front window from Mapinfo.
        /// </summary>
        /// <returns>An instance of <see cref="MapWindow"/> containing the front window.</returns>
        public MapWindow GetFrontWindow()
        {
            int windowid = Convert.ToInt32(this.Evaluate("FrontWindow()"));
            return new MapWindow(this,windowid);
        }

        private SystemInfo systeminfo;
        /// <summary>
        /// Returns a <see cref="SystemInfo"/> object allowing access to system 
        /// info about the current running instance of Mapinfo.
        /// </summary>
        public SystemInfo SystemInfo
        {
            get
            {
                if (systeminfo == null)
                {
                    this.systeminfo = new SystemInfo(this);
                }
                return systeminfo;
            }
        }

        private TableCollection tables;
        /// <summary>
        /// Returns a new <see cref="TableCollection"/> 
        /// giving access to the current open tables in Mapinfo.
        /// </summary>
        public TableCollection Tables
        {
            get
            {
                if (this.tables == null)
                {
                    this.tables = new TableCollection(this);
                    this.tables.RefreshList();
                }
                return this.tables;
            }
        }
        
        /// <summary>
        /// Runs a command against the underlying Mapinfo instance.
        /// </summary>
        /// <param name="commandString">The command string to run in Mapinfo.</param>
        public void RunCommand(string commandString)
        {
            this.mapinfo.RunCommand(commandString);
        }

        /// <summary>
        /// Runs and evaluates a command string in mapinfo.
        /// </summary>
        /// <param name="commandString">The command string to run in Mapinfo.</param>
        /// <returns>The result of the eval command in Mapinfo.</returns>
        public string Evaluate(string commandString)
        {
            return this.mapinfo.Evaluate(commandString);
        }

        /// <summary>
        /// Returns the underlying Mapinfo instance for this session.
        /// </summary>
        /// <returns></returns>
        public object GetUnderlyingMapinfoInstance()
        {
            return this.mapinfo.GetUnderlyingMapinfoInstance();
        }

        /// <summary>
        /// Gets or sets the underlying Mapinfo callback object
        /// </summary>
        public MapinfoCallback Callback
        {
            get
            {
                return this.mapinfo.Callback;
            }
            set
            {
                this.mapinfo.Callback = value;
            }
        }

        // TODO Callback properties need to be set and foward on to the correct Mapinfo here.

        /// <summary>
        /// Returns a <see cref="IEnumerable{T}"/> containing a list of all installed versions
        /// of Mapinfo.
        /// </summary>
        /// <returns>A collection of int matching the versions of Mapinfo installed.</returns>
        public static IEnumerable<int> GetInstalledMapinfoVersions()
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
