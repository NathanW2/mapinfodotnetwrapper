namespace MapinfoWrapper.Mapinfo
{
    using System;
    using System.Reflection;
    using MapinfoWrapper.Mapinfo.Internals;

    /// <summary>
    /// Holds an instance of Mapinfo which has been created in the miadm.dll 
    /// when calling a .Net assembly from Mapbasic(MBX).
    /// <para>NOTE! This object uses reflection to invoke the Do and Eval commands of the Mapinfo instance, to allow this assembly to
    /// be Mapinfo version independent.</para>
    /// <para>If you are using a version of Mapinfo less then 9.5 you will need to use the <see cref="COMMapinfo"/> class/.</para>
    /// </summary>
    internal class MapbasicInvokedMapinfo : IMapinfoWrapper
    {
        private object mapinfoinstance;
        private Type m_RuntimeMapInfoType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapbasicInvokedMapinfo"/> class.
        /// </summary>
        /// <param name="mapinfoInstance">The current running instance</param>
        internal MapbasicInvokedMapinfo(object mapinfoInstance, Type mapinfotype)
        {
            this.m_RuntimeMapInfoType = mapinfotype;
            this.mapinfoinstance = mapinfoInstance;
        }

        public static MapbasicInvokedMapinfo GetMapinfoFromInstance(Assembly miadmassembly)
        {
            Type intertype = miadmassembly.GetType("MapInfo.MiPro.Interop.InteropServices");
            //object interobject = Activator.CreateInstance(intertype);

            PropertyInfo propery = intertype.GetProperty("MapInfoApplication", BindingFlags.Public | BindingFlags.Static);

            object obj = null;
            object value = propery.GetValue(obj, null);
            return new MapbasicInvokedMapinfo(value, propery.PropertyType);
        }

        #region IMapinfoWrapper Members

        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        public void Do(string commandString)
        {
            if (String.IsNullOrEmpty(commandString))
                throw new ArgumentNullException("commandString", "Command string can not be null");

            Object[] commandstrings = { commandString };
            MethodInfo domethod = this.m_RuntimeMapInfoType.GetMethod("Do");
            domethod.Invoke(this.mapinfoinstance, commandstrings);
        }

        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo and retruns the result as a string.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        /// <returns>A string containing the value of the return from the command string just excuted.</returns>
        public string Eval(string commandString)
        {
            if (String.IsNullOrEmpty(commandString))
                throw new ArgumentNullException("commandString", "Command string can not be null");

            Object[] commandstrings = { commandString };
            MethodInfo domethod = this.m_RuntimeMapInfoType.GetMethod("Eval");
            return (String)domethod.Invoke(this.mapinfoinstance, commandstrings);
        }

        /// <summary>
        /// Returns the underlying type of Mapinfo, this can be used to access to methods exposed by 
        /// Mapinfo's COM API but not contained in the wrapper or the <see cref="IMapinfoWrapper"/> interface.
        /// </summary>
        /// <returns>The underlying type of Mapinfo.</returns>
        public object GetUnderlyingMapinfoInstance()
        {
            return mapinfoinstance;
        }

        public MapinfoCallback Callback
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Visible
        {
            get
            {
                return ((IMapInfo)this.mapinfoinstance).Visible;
            }
            set
            {
                ((IMapInfo)this.mapinfoinstance).Visible = true;
            }
        }

        #endregion

        #region IMapinfoWrapper Members


        public int LastErrorCode
        {
            get { return 0; }
        }

        public string LastErrorMessage
        {
            get { return ""; }
        }

        #endregion
    }
}
