using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using MapinfoWrapper.Mapinfo.Internals;

namespace MapinfoWrapper.Mapinfo
{
    /// <summary>
    /// Holds an instance of Mapinfo which has been created in the miadm.dll 
    /// when calling a .Net assembly from Mapbasic(MBX).
    /// <para>NOTE! This object uses reflection to invoke the Do and Eval commands of the Mapinfo instance, to allow this assembly to
    /// be Mapinfo version independent.</para>
    /// <para>If you are using a version of Mapinfo less then 9.5 you will need to use the <see cref="T:COMMapinfo"/> class/.</para>
    /// </summary>
    public class MapbasicInvokedMapinfo : IMapinfoWrapper
    {
        private IMapInfo2 mapinfoinstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapbasicInvokedMapinfo"/> class.
        /// </summary>
        /// <param name="mapinfoInstance">The current running instance</param>
        public MapbasicInvokedMapinfo(IMapInfo2 mapinfoInstance)
        {
           this.mapinfoinstance = mapinfoInstance;
        }

        public static MapbasicInvokedMapinfo GetMapinfoFromInstance(Object mapinfoInstance)
        {
            Type mapinfotype = mapinfoInstance.GetType();
            FieldInfo imapinfofield = mapinfotype.GetField("_mapinfo", BindingFlags.Instance | BindingFlags.NonPublic);
            Object imapinfoinstance = imapinfofield.GetValue(mapinfoInstance);
            return new MapbasicInvokedMapinfo((IMapInfo2)imapinfoinstance);
        }

        #region IMapinfoWrapper Members

        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        public void RunCommand(string commandString)
        {
            if (String.IsNullOrEmpty(commandString))
                throw new ArgumentNullException("commandString", "Command string can not be null");

            Object[] commandstrings = { commandString };
            MethodInfo domethod = this.mapinfoinstance.GetType().GetMethod("Do");
            domethod.Invoke(this.mapinfoinstance, commandstrings);
        }

        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo and retruns the result as a string.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        /// <returns>A string containing the value of the return from the command string just excuted.</returns>
        public string Evaluate(string commandString)
        {
            if (String.IsNullOrEmpty(commandString))
                throw new ArgumentNullException("commandString", "Command string can not be null");

            Object[] commandstrings = { commandString };
            MethodInfo domethod = this.mapinfoinstance.GetType().GetMethod("Eval");
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

        #endregion
    }
}
