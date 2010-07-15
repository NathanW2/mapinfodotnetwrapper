using System;
using System.Linq;
using MapInfo.Wrapper.DataAccess;
using MapInfo.Wrapper.Embedding;
using MapInfo.Wrapper.Exceptions;
using MapInfo.Wrapper.MapOperations;
using MapInfo.Wrapper.UI;

namespace MapInfo.Wrapper.Mapinfo
{
    public interface IMapInfoSession : IMapInfoWrapper
    {
        /// <summary>
        /// Event that is fired when the session has be closed.
        /// </summary>
        event EventHandler SessionEnded;

        /// <summary>
        /// Returns a collection of custom buttons in Mapinfo. 
        /// <para>This collection will only return the custom button pads add using the Wrapper. If you need to get a standard button pad or
        /// a custom one  </para>
        /// </summary>
        ButtonPadCollection ButtonPads { get; }

        /// <summary>
        /// Returns a <see cref="SystemInfo"/> object allowing access to system 
        /// info about the current running instance of Mapinfo.
        /// </summary>
        SystemInfo SystemInfo { get; }

        /// <summary>
        /// Returns a new <see cref="TableCollection"/> 
        /// giving access to the current open tables in Mapinfo.
        /// </summary>
        TableCollection Tables { get; }

        /// <summary>
        /// Returns a collection of windows for this session.
        /// </summary>
        WindowCollection Windows { get; }


        IQueryProvider MapinfoProvider { get; }

        /// <summary>
        /// Gets or sets a <see cref="EntityLoadOptions"/> that contain the options that will be 
        /// used when retrieving entities from tables. 
        /// </summary>
        EntityLoadOptions LoadOptions { get; set; }

        /// <summary>
        /// Evaluates a command string in MapInfo.
        /// <para>This method will throw a <see cref="MapinfoException"/> if any errors are found when calling LastErrorCode or COMExceptions,
        /// this method will have a slight overhead because it checks LastErrorCode on every pass.</para>
        /// </summary>
        /// <param name="commandString">The result of the command string.</param>
        string TryEval(string commandString);

        /// <summary>
        /// Runs a command against the underlying Mapinfo instance.
        /// <para>This method will throw exceptions on any errors or COMExceptions, this method will have a slight overhead because it checks
        /// LastErrorCode on every pass.</para>
        /// </summary>
        /// <param name="commandString"></param>
        void TryDo(string commandString);

        /// <summary>
        /// Opens a workspace in a instance of Mapinfo.
        /// <para>After the workspace is open the list of tables is refeshed.</para>
        /// </summary>
        /// <param name="workspacePath">The path to the workspace which needs to be opened.</param>
        /// <returns>A instance of a <see cref="Workspace"/> which can be used to get infomation about the opened workspace.</returns>
        Workspace OpenWorkspace(string workspacePath);

        /// <summary>
        /// Ends the current session of Mapinfo.
        /// </summary>
        void CloseMapinfo();

        Query CreateQuery(string querystring);
    }
}