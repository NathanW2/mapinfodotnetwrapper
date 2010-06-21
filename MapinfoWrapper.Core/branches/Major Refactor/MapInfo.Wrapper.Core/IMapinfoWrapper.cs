using MapInfo.Wrapper.Core;

namespace Mapinfo.Wrapper.Mapinfo
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

        MapinfoCallback Callback { get; set; }

        bool Visible { get; set; }

        int LastErrorCode { get; }

        string LastErrorMessage { get; }

        void RegisterCallback(object obj);
    }
}
