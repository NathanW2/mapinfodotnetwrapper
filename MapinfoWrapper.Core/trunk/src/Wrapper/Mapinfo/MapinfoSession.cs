using System;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.Embedding;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.MapOperations;

namespace MapinfoWrapper.Mapinfo
{
    /// <summary>
    /// An absract base class that is used to make calls to Mapinfo.
    /// 
    /// <para>This object can not be created directly, to create a new instance of Mapinfo
    /// you must call <see cref="COMMapinfo.CreateInstance()"/> which will create a new instance of Mapinfo
    /// and returns a <see cref="MapinfoSession"/> object that contains the instance of Mapinfo.</para>
    /// 
    /// <para>This is the lowest object in the MapinfoWrapper API,
    /// all objects in the MapinfoWrapper API take and make calls through this object.</para> 
    /// </summary>
    public abstract class MapinfoSession : IMapinfoWrapper
    {
        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        public abstract void RunCommand(string commandString);

        /// <summary>
        /// Runs a specified Mapinfo command string in Mapinfo and retruns the result as a string.
        /// </summary>
        /// <param name="commandString">The Mapbasic command string to send to Mapinfo.</param>
        /// <returns>A string containing the value of the return from the command string just excuted.</returns>
        public abstract string Evaluate(string commandString);

        /// <summary>
        /// Returns the underlying type of Mapinfo, this can be used to access to methods exposed by 
        /// Mapinfo's COM API but not contained in the wrapper or the <see cref="IMapinfoWrapper"/> interface.
        /// </summary>
        /// <returns>The underlying type of Mapinfo.</returns>
        public abstract object GetUnderlyingMapinfoInstance();

        internal virtual object RunTableInfo(string tableName, TableInfo attribute)
        {
            int enumvalue = (int)attribute;
            string command = "TableInfo({0},{1})".FormatWith(tableName, enumvalue);
            string value = this.Evaluate(command);
            switch (attribute)
            {
                case TableInfo.Tabfile:
                case TableInfo.Name:
                    return value;
                default:
                    return value;
            }
        }

        /// <summary>
        /// Run the ObjectInfo mapbasic command in Mapinfo, and returns a string containing the result.
        /// </summary>
        /// <param name="variable">The variable used by the ObjectInfo call.</param>
        /// <param name="attribute">The attribute specifying which information to return.</param>
        /// <returns>A string containing the retured result from calling the ObjectInfo command in Mapinfo.</returns>
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

        /// <summary>
        /// Returns a <see cref="SystemInfo"/> object allowing access to system 
        /// info about the current running instance of Mapinfo.
        /// </summary>
        public SystemInfo SystemInfo
        {
            get
            {
                return new SystemInfo(this);
            }
        }

        /// <summary>
        /// Returns a new <see cref="TableManager"/> which allows
        /// access to open tables in the current session, opening tables and closeing tables.
        /// </summary>
        public TableManager TableManager
        {
            get
            {
                return new TableManager(this);
            }
        }
    }
}
