using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.TableOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.TableOperations.RowOperations;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.MapOperations
{
    public class MapWindow
    {
        private IMapinfoWrapper wrapper;
        private int mapnumber;
        public MapWindow(IMapinfoWrapper wrapper, int mapNumber)
        {
            this.wrapper = wrapper;
            this.mapnumber = mapNumber;
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the table supplied as the first layer. 
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <param name="table">The table which will be opened in a new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window. </returns>
        public static MapWindow MapTable<TTable>(IMapinfoWrapper wrapper, TTable table)
            where TTable : ITable
        {
            if (!table.IsMappable)
            {
                throw new ArgumentException("Table must be mappable");
            }

            List<TTable> tablelist = new List<TTable>();
            tablelist.Add(table);
            return MapWindow.MapTables(wrapper, tablelist);
        }

        /// <summary>
        /// Returns a window ID of the formost window in Mapinfo.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <returns></returns>
        public static int GetFrontWindowID(IMapinfoWrapper wrapper)
        {
            int frontwindow = Convert.ToInt32(wrapper.Evaluate("FrontWindow()"));
            return frontwindow;
        }

        /// <summary>
        /// Gets the front window from Mapinfo.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <returns>An instance of <see cref="T:MapWindow"/> containing the front window.</returns>
        public static MapWindow GetFrontWindow(IMapinfoWrapper wrapper)
        {
            int windowid = MapWindow.GetFrontWindowID(wrapper);
            return new MapWindow(wrapper, windowid);
        }

        /// <summary>
        /// Returns the window id for the current window.
        /// </summary>
        public int WindowId
        {
            get
            {
                return Convert.ToInt32(this.wrapper.Evaluate("WindowID({0})".FormatWith(this.mapnumber)));
            }
        }

        /// <summary>
        /// Returns the handle for the current Map window.
        /// </summary>
        public IntPtr Hwnd
        {
            get
            {
               return (IntPtr)long.Parse(this.wrapper.Evaluate("WindowInfo({0},12)".FormatWith(this.mapnumber)));
            }
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the tables supplied as the layers for that map.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <param name="tablelist">A collection of tables which will be used in the new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window.</returns>
        public static MapWindow MapTables<TTable>(IMapinfoWrapper wrapper, IEnumerable<TTable> tablelist)
            where TTable : ITable
        {
            StringBuilder commandbuilder = new StringBuilder("Map From ");
            foreach (var table in tablelist)
            {
                commandbuilder.AppendFormat("{0},".FormatWith(table.Name));
            }
            string command = commandbuilder.ToString().TrimEnd(',');
            wrapper.RunCommand(command);
            int frontwindowid = MapWindow.GetFrontWindowID(wrapper);
            return new MapWindow(wrapper, frontwindowid);
        }
    }
}
