using System.Collections.Generic;
using System.Windows.Forms;
using MapinfoWrapper;
using MapinfoWrapper.DataAccess;
using Wrapper.Example.Tables;

namespace Wrapper.Example.Workspaces
{
    /// <summary>
    /// This is a example workspace class, it allows the user to open the WorldWorkspace without needing to
    /// worry about the path.  It also contains properties which give you strong typed access to the tables in the workspace
    /// this example workspace class only contains one table which is the world table, but the actual workspace contains other tables also 
    /// which haven't been inculded in this example.
    /// </summary>
    /// <remarks>NOTE! In future releases of the OLE wrapper, I will be inculding a tool that will generate this kind workspace entity and any table
    /// entities from the tables that workspace uses automaticlly</remarks>
    [UsesWrapper]
    public class WorldWorkspace : Workspace
    {
        private WorldWorkspace()
        { }

        /// <summary>
        /// Open the world work space in Mapinfo.
        /// </summary>
        /// <returns>An instance of the world workspace which gives you strong typed access to the tables within the workspace.</returns>
        public static WorldWorkspace Open()
        {
            Workspace workspace = Workspace.OpenWorkspace(Application.StartupPath + @"\Maps\WORLD.WOR");
            return new WorldWorkspace();
        }

        /// <summary>
        /// Holds an instance of the world table in Mapinfo, that was open with the workspace.
        /// </summary>
        public Table<World> WorldTable 
        {
            get 
            {
                return (Table<World>)Table.GetTable<World>("World");
            }
        }

        /// <summary>
        /// Returns a collection of all the tables in the workspace.
        /// </summary>
        public IEnumerable<ITable> Tables
        {
            get 
            {
                List<ITable> tablelist = new List<ITable>();
                tablelist.Add(WorldTable);
                return tablelist;
            }
        }
    }
}
