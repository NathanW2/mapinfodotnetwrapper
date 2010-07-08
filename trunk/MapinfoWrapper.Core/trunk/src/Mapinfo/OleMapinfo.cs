using System;
using MapinfoWrapper.Mapinfo.Internals;
namespace MapinfoWrapper.Mapinfo
{
    /// <summary>
    /// Contains a running instance of Mapinfo COM object.  This is the lowest object
    /// in the MapinfoWrapper API, all objects in the MapinfoWrapper API take and 
    /// make calls through this object. 
    /// </summary>
    public class COMMapinfo : IMapinfoWrapper
    {
        private DMapInfo mapinfoinstance;

        /// <summary>
        /// Creates a new instance of Mapinfo and returns a <see cref="MapinfoSession"/>
        /// which contains the instance. 
        /// <para>The returned objet can be passed into objects and
        /// methods that need it in the MapinfoWrapper API.</para>
        /// </summary>
        /// <returns>A new <see cref="COMMapinfo"/> containing the running instance of Mapinfo.</returns>
        public static IMapinfoWrapper CreateMapInfoInstance()
        {
            DMapInfo instance = CreateMapinfoInstance();
            COMMapinfo mapinfo = new COMMapinfo(instance);
            return mapinfo;
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
        /// <para>Initializes a new instance of the <see cref="COMMapinfo"/> class, which holds 
        /// an instance of a currently running instance of Mapinfo's COM object.</para>
        /// <param name="mapinfoInstance">A currently running instance of Mapinfo's COM object.</param>
        public COMMapinfo(DMapInfo mapinfoInstance)
        {
            this.mapinfoinstance = mapinfoInstance;
        }
      
        #region IMapinfoWrapper Members

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
        /// Mapinfo's COM API but not contained in the wrapper or the <see cref="IMapinfoWrapper"/> interface.
        /// </summary>
        /// <returns>The underlying type of Mapinfo.</returns>
        public object GetUnderlyingMapinfoInstance()
        {
            return this.mapinfoinstance;
        }

        #endregion

        private MapinfoCallback callback;
        public MapinfoCallback Callback {
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

        #region IMapinfoWrapper Members

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
