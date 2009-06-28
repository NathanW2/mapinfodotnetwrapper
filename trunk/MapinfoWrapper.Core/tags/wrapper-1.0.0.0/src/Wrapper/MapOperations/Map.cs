using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.MapOperations
{
    public class MapWindow
    {
        private IMapinfoWrapper mapinfoinstance;
        private int mapnumber;

        internal MapWindow(int mapNumber) : this(null, mapNumber)
        { }

        internal MapWindow(IMapinfoWrapper wrapper, int mapNumber)
        {
            this.mapinfoinstance = wrapper ?? ServiceLocator.GetInstance<IMapinfoWrapper>();
            this.mapnumber = mapNumber;
        }

        /// <summary>
        /// Returns a window ID of the formost window in Mapinfo.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <returns></returns>
        internal static int GetFrontWindowID()
        {
            int frontwindow = Convert.ToInt32(wrapper.Evaluate("FrontWindow()"));
            return frontwindow;
        }

        /// <summary>
        /// Gets the front window from Mapinfo.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <returns>An instance of <see cref="T:MapWindow"/> containing the front window.</returns>
        public static MapWindow GetFrontWindow()
        {
            int windowid = MapWindow.GetFrontWindowID();
            return new MapWindow(windowid);
        }

        /// <summary>
        /// Returns the window id for the current window.
        /// </summary>
        public int WindowId
        {
            get
            {
                return Convert.ToInt32(this.mapinfoinstance.Evaluate("WindowID({0})".FormatWith(this.mapnumber)));
            }
        }

        /// <summary>
        /// Returns the handle for the current Map window.
        /// </summary>
        public IntPtr Hwnd
        {
            get
            {
                return (IntPtr)long.Parse(this.mapinfoinstance.Evaluate("WindowInfo({0},12)".FormatWith(this.mapnumber)));
            }
        }

        /// <summary>
        /// Closes the current Map window in Mapinfo.
        /// </summary>
        public void CloseWindow()
        {
            this.mapinfoinstance.RunCommand("Close window {0}".FormatWith(this.WindowId));
            // TODO Dispose of Mapwindow here.
        }

        private static readonly IMapinfoWrapper wrapper = ServiceLocator.GetInstance<IMapinfoWrapper>();

        /// <summary>
        /// Opens a new Map window in Mapinfo using the tables supplied as the layers for that map.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <param name="tablelist">A collection of tables which will be used in the new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window.</returns>
        public static MapWindow MapTables(IEnumerable<ITable> tablelist)
        {
            var mappable = tablelist.ToList().FindAll(table => table.IsMappable);

            if (mappable == null)
                throw new ArgumentNullException("mappable","No tables are mappable");

            StringBuilder commandbuilder = new StringBuilder("Map From ");
            foreach (var table in tablelist)
            {
                commandbuilder.AppendFormat("{0},".FormatWith(table.Name));
            }
            string command = commandbuilder.ToString().TrimEnd(',');
            wrapper.RunCommand(command);
            int frontwindowid = MapWindow.GetFrontWindowID();
            return new MapWindow(frontwindowid);
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the table supplied as the first layer. 
        /// </summary>
        /// <param name="table">The table which will be opened in a new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window. </returns>
        public static MapWindow MapTable(ITable table)
        {
            List<ITable> tablelist = new List<ITable>();
            tablelist.Add(table);
            return MapWindow.MapTables(tablelist);
        }
    }
}
