using System.Runtime.InteropServices;

namespace MapinfoWrapper.Mapinfo
{
    /// <summary>
    /// Contains only the basic functions
    /// needed to communicate with Mapinfo.
    /// </summary>
    public interface IMapinfoWrapper
    {
        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        void Do(string commandString);
        
        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo and retruns the result as a string.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        /// <returns>A string containing the value of the return from the command string just excuted.</returns>
        string Eval(string commandString);

        /// <summary>
        /// Returns the underlying type of Mapinfo, this can be used to access to methods exposed by 
        /// Mapinfo's COM API but not contained in the wrapper or the <see cref="IMapinfoWrapper"/> interface.
        /// </summary>
        /// <returns>The underlying type of Mapinfo.</returns>
        object GetUnderlyingMapinfoInstance();

        /// <summary>
        /// Gets or sets the visiblity of the current MapInfo instance,
        /// if you are using a instance of MapInfo that is loaded when calling from MapBasic into .Net
        /// this property will throw a NotSupportedException.
        /// </summary>
        bool Visible { get; set; }
        
        /// <summary>
        /// Returns the last error code that was throw from MapInfo.
        /// </summary>
        int LastErrorCode { get; }

        /// <summary>
        /// Returns the last error message that was throw from MapInfo.
        /// </summary>
        string LastErrorMessage { get; }

        /// <summary>
        /// Registers a object as a callback object for MapInfo.
        /// </summary>
        /// <param name="obj">The object to set as the MapInfo callback object, the object must use the <see cref="ComVisibleAttribute"/> in order to recive updates from
        /// MapInfo.  The base class <see cref="MapinfoCallback"/>is included with the wrapper that provides basic events from Map</param>
        void RegisterCallback(object obj);
    }
}
