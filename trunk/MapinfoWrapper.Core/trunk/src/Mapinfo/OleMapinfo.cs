using System;
using MapInfo.Wrapper;
using MapInfo.Wrapper.Mapinfo.Internals;

namespace MapInfo.Wrapper.Mapinfo
{
    /// <summary>
    /// Contains a running instance of Mapinfo COM object.  This is the lowest object
    /// in the MapinfoWrapper API, all objects in the MapinfoWrapper API take and 
    /// make calls through this object. 
    /// </summary>
    public class ComMapInfo : IMapInfoWrapper
    {
        private DMapInfo mapinfoinstance;

        /// <summary>
        /// Creates a new instance of Mapinfo and returns a <see cref="MapInfoSession"/>
        /// which contains the instance. 
        /// <para>The returned objet can be passed into objects and
        /// methods that need it in the MapinfoWrapper API.</para>
        /// </summary>
        /// <returns>A new <see cref="ComMapInfo"/> containing the running instance of Mapinfo.</returns>
        public static IMapInfoWrapper CreateMapInfoInstance()
        {
            DMapInfo instance = CreateMapinfoInstance();
            ComMapInfo map_info = new ComMapInfo(instance);
            return map_info;
        }

        private static DMapInfo CreateMapinfoInstance()
        {
            Type mapinfotype = Type.GetTypeFromProgID("Mapinfo.Application");
            DMapInfo instance = (DMapInfo)Activator.CreateInstance(mapinfotype);
            return instance;
        }


        /// <summary>
        /// <b>This is only provided to allow for testing and should not be used outside of a test, if you need to
        /// create a new instance of Mapinfo please use <see cref="MapinfoSessionManager"/>
        /// 
        /// <para>Initializes a new instance of the <see cref="ComMapInfo"/> class, which holds 
        /// an instance of a currently running instance of Mapinfo's COM object.</para>
        /// <param name="mapinfoInstance">A currently running instance of Mapinfo's COM object.</param>
        public ComMapInfo(DMapInfo mapinfoInstance)
        {
            this.mapinfoinstance = mapinfoInstance;
        }
      
        #region IMapInfoWrapper Members

        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        public void Do(string commandString)
        {
            this.mapinfoinstance.Do(commandString);
        }

        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo and retruns the result as a string.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        /// <returns>A string containing the value of the return from the command string just excuted.</returns>
        public string Eval(string commandString)
        {
            return this.mapinfoinstance.Eval(commandString);
        }

        /// <summary>
        /// Returns the underlying type of Mapinfo, this can be used to access to methods exposed by 
        /// Mapinfo's COM API but not contained in the wrapper or the <see cref="IMapInfoWrapper"/> interface.
        /// </summary>
        /// <returns>The underlying type of Mapinfo.</returns>
        public object GetUnderlyingMapinfoInstance()
        {
            return this.mapinfoinstance;
        }

        #endregion

        private MapInfoCallback callback;
        public MapInfoCallback Callback {
            get 
            {
                return this.callback;
            }
            set 
            {
                this.mapinfoinstance.SetCallback(value);
                callback = value;
            }
        }

        public bool Visible 
        {
            get 
            {
                return this.mapinfoinstance.Visible;
            }
            set 
            {
                this.mapinfoinstance.Visible = value;
            }
        }

        #region IMapInfoWrapper Members

        public int LastErrorCode
        {
            get { return this.mapinfoinstance.LastErrorCode; }
        }

        public string LastErrorMessage
        {
            get { return this.mapinfoinstance.LastErrorMessage; }
        }

        #endregion

        public void RegisterCallback(object obj)
        {
            this.mapinfoinstance.SetCallback(obj);
        }
    }
}
