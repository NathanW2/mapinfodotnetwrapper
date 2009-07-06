using System;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.MapOperations;

namespace MapinfoWrapper.Mapinfo
{
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
        /// <returns>An instance of <see cref="T:MapWindow"/> containing the front window.</returns>
        public MapWindow GetFrontWindow()
        {
            int windowid = Convert.ToInt32(this.Evaluate("FrontWindow()"));
            return new MapWindow(this,windowid);
        }
    }
}
