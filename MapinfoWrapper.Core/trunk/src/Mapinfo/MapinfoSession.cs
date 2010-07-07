// --------------------------------
// <copyright file="MapinfoSession.cs" company="Nathan Woodrow">
//     Copyright (c) 2009 Nathan Woodrow All rights reserved.
// </copyright>
// <author>Nathan Woodrow</author>
// ---------------------------------
namespace MapinfoWrapper.Mapinfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Internals;
    using Core.Extensions;
    using MapinfoWrapper.DataAccess;
    using MapinfoWrapper.Embedding;
    using MapinfoWrapper.MapOperations;
    using MapinfoWrapper.UI;
    using Microsoft.Win32;
    using MapinfoWrapper.DataAccess.LINQ;
    using MapinfoWrapper.Core;
    using MapinfoWrapper.Exceptions;
    using System.Runtime.InteropServices;

    public class MapinfoSession : IMapinfoWrapper
    {
        private ButtonPadCollection buttonpads;
        private readonly IMapinfoWrapper mapinfo;
        private SystemInfo systeminfo;
        private TableCollection tables;

        public MapinfoSession(IMapinfoWrapper mapinfoAPI)
        {
            this.mapinfo = mapinfoAPI;
            this.LoadOptions = null;
        }

        /// <summary>
        /// Returns a collection of custom buttons in Mapinfo. 
        /// <para>This collection will only return the custom button pads add using the Wrapper. If you need to get a standard button pad or
        /// a custom one  </para>
        /// </summary>
        public ButtonPadCollection ButtonPads
        {
            get
            {
                if (this.buttonpads == null)
                {
                    this.buttonpads = new ButtonPadCollection(this);
                }
                return this.buttonpads;
            }
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

        private WindowCollection windows;
        public WindowCollection Windows
        {
            get
            {
                if (this.windows == null)
                {
                    this.windows = new WindowCollection(this);
                }
                return this.windows;
            }
        }

        /// <summary>
        /// Gets or sets the visiblity of the current MapinfoSession
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.mapinfo.Visible;
            }
            set
            {
                this.mapinfo.Visible = value;
            }
        }

        private IQueryProvider provider;
        internal IQueryProvider MapinfoProvider
        {
            get
            {
                if (this.provider == null)
                {
                    this.provider = new MapinfoProvider(this, new MaterializerFactory(this));
                }
                return this.provider;
            }
        }

        private EntityLoadOptions loadoptions;
        /// <summary>
        /// Gets or sets a <see cref="EntityLoadOptions"/> that contain the options that will be 
        /// used when retrieving entities from tables. 
        /// </summary>
        public EntityLoadOptions LoadOptions
        { 
            get
            {
                return this.loadoptions;
            }
            set
            {
                this.loadoptions = value ?? new EntityLoadOptions();
            }
        }

        public delegate void MapinfoSessionHandler(MapinfoSession session);

        /// <summary>
        /// A static event that gets fired when a Mapinfo session is created using the wrapper.
        /// </summary>
        public static event MapinfoSessionHandler SessionCreated;
        public event Action SessionEnded;

        /// <summary>
        /// Runs and evaluates a command string in mapinfo.
        /// </summary>
        /// <param name="commandString">The command string to run in Mapinfo.</param>
        /// <returns>The result of the eval command in Mapinfo.</returns>
        public string Eval(string commandString)
        {
            Guard.AgainstNullOrEmpty(commandString, "commandString");

            try
            {
                string value = this.mapinfo.Eval(commandString);

                if (this.mapinfo.LastErrorCode > 0)
                {
                    throw new MapinfoException(this.mapinfo.LastErrorMessage, null, this.mapinfo.LastErrorCode);
                }
                return value;
            }
            catch (COMException comex)
            {
                throw new MapinfoException(comex.Message, comex, this.mapinfo.LastErrorCode);
            }
        }

        /// <summary>
        /// Trys to evaluate a command string in MapInfo, this method will not throw exceptions but return true if the
        /// command was successful.
        /// </summary>
        /// <param name="commandString">The command string to run in Mapinfo.</param>
        /// <param name="value"></param>
        /// <returns> When this method returns, <paramref name="value"/> contains the string value returned from MapInfo.
        /// If the method fails <paramref name="value"/> will be null and the return value is false.</returns>
        public bool TryEval(string commandString, out string value)
        {
            try
            {
                value = this.Eval(commandString);
                return true;
            }
            catch (COMException)
            {
                value = string.Empty;
                return false;
            }
            catch (MapinfoException)
            {
                value = string.Empty;
                return false;
            }
                
        }

        /// <summary>
        /// Runs a command against the underlying Mapinfo instance.
        /// <para>This method will never throw a <see cref="COMException"/> if MapInfo hits a error.  This desgin decision was made
        /// because the MapInfo instance that is returned from MapInfo.InteropServices.MapInfoApplication does not thorw exception, you have to check
        /// the LastErrorMessage if you want to find the last error.  In order to reduce overhead of calling Do, this method does not check for LastErrorMessage or 
        /// throw exceptions.  This is so that both a COM invoked MapInfo and one that is used from the MapInfo.InteropServices.MapInfoApplication have the same result through
        /// this method.</para>
        /// <para>If you need to catch exceptions, you can use the TryDo method which will throw on exceptions and errors.</para>
        /// </summary>
        /// <param name="commandString">The command string to run in Mapinfo.</param>
        /// <exception cref="ArgumentNullException" />
        public void Do(string commandString)
        {
            Guard.AgainstNullOrEmpty(commandString, "commandString");

            try
            {
                this.mapinfo.Do(commandString);
            }
            catch (COMException comex)
            {
                return;
            }
        }

        /// <summary>
        /// Runs a command against the underlying Mapinfo instance.
        /// <para>This method will throw exceptions on any errors or COMExceptions, this method will have a slight overhead because it checks
        /// LastErrorCode on every pass.</para>
        /// </summary>
        /// <param name="commandString"></param>
        public void TryDo(string commandString)
        {
            Guard.AgainstNullOrEmpty(commandString, "commandString");

            try
            {
                this.mapinfo.Do(commandString);

                if (this.mapinfo.LastErrorCode > 0)
                {
                    throw new MapinfoException(this.mapinfo.LastErrorMessage, null, this.mapinfo.LastErrorCode);
                }
            }
            catch (COMException comex)
            {
                throw new MapinfoException(comex.Message, comex, this.mapinfo.LastErrorCode);
            }
            
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
        /// Opens a workspace in a instance of Mapinfo.
        /// <para>After the workspace is open the list of tables is refeshed.</para>
        /// </summary>
        /// <param name="workspacePath">The path to the workspace which needs to be opened.</param>
        /// <returns>A instance of a <see cref="Workspace"/> which can be used to get infomation about the opened workspace.</returns>
        public Workspace OpenWorkspace(string workspacePath)
        {
            this.Do("Run Application {0}".FormatWith(workspacePath.InQuotes()));
            this.Tables.RefreshList();
            return new Workspace();
        }

        /// <summary>
        /// Ends the current session of Mapinfo.
        /// </summary>
        public void CloseMapinfo()
        {
            this.Do("End Mapinfo");
            Marshal.ReleaseComObject(this.mapinfo.GetUnderlyingMapinfoInstance());
            GC.Collect();

            if (this.SessionEnded != null)
                this.SessionEnded();
        }

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

        /// <summary>
        /// Creates a new instance of Mapinfo and returns a <see cref="MapinfoSession"/>
        /// which contains the instance. 
        /// <para>The returned objet can be passed into objects and
        /// methods that need it in the MapinfoWrapper API.</para>
        /// </summary>
        /// <returns>A new <see cref="COMMapinfo"/> containing the running instance of Mapinfo.</returns>
        public static MapinfoSession CreateMapInfoInstance()
        {

            DMapInfo instance = CreateMapinfoInstance();
            COMMapinfo mapinfo = new COMMapinfo(instance);
            MapinfoSession session = new MapinfoSession(mapinfo);
            if (SessionCreated != null)
                SessionCreated(session);
            return session;
        }

        private static DMapInfo CreateMapinfoInstance()
        {
            Type mapinfotype = Type.GetTypeFromProgID("Mapinfo.Application");
            DMapInfo instance = (DMapInfo)Activator.CreateInstance(mapinfotype);
            return instance;
        }

        public static MapinfoSession GetLoadedInstance()
        {
            return new MapinfoSession(MapBasicInvokedMapInfo.CreateMapInfoFromInstance());
        }

        #region IMapinfoWrapper Members


        public int LastErrorCode
        {
            get { return this.mapinfo.LastErrorCode; }
        }

        public string LastErrorMessage
        {
            get { return this.mapinfo.LastErrorMessage; }
        }

        #endregion

        public void RegisterCallback(object obj)
        {
            this.mapinfo.RegisterCallback(obj);
        }
    }
}
