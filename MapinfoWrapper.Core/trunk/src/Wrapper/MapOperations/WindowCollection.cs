using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess;

namespace MapinfoWrapper.MapOperations
{
    public class WindowCollection : IEnumerable<Window>
    {
        private List<Window> windows;
        private readonly MapinfoSession mapinfo;

        public WindowCollection(MapinfoSession session)
        {
            this.mapinfo = session;
            this.windows = new List<Window>();
        }

        public IEnumerator<Window> GetEnumerator()
        {
            return this.windows.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.windows.GetEnumerator();
        }

        /// <summary>
        /// Gets the current front window in MapInfo.
        /// </summary>
        public Window FrontWindow
        {
            get
            {
                int windowid = Convert.ToInt32(this.mapinfo.Evaluate("FrontWindow()"));
                return new Window(windowid, this.mapinfo);
            }
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the tables supplied as the layers for that map.
        /// </summary>
        /// <param name="tablelist">A collection of tables which will be used in the new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window.</returns>
        public MapWindow MapTables(IEnumerable<Table> tablelist)
        {
            List<Table> mappable = tablelist.ToList().FindAll(table => table.IsMappable);

            if (mappable == null)
                throw new ArgumentNullException("mappable", "No tables are mappable");

            StringBuilder commandbuilder = new StringBuilder("Map From ");
            foreach (var table in tablelist)
            {
                commandbuilder.AppendFormat("{0},".FormatWith(table.Name));
            }
            string command = commandbuilder.ToString().TrimEnd(',');
            this.mapinfo.RunCommand(command);
            return new MapWindow(this.FrontWindow.ID, this.mapinfo);
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the table supplied as the first layer. 
        /// </summary>
        /// <param name="table">The table which will be opened in a new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window. </returns>
        public MapWindow MapTable(Table table)
        {
            List<Table> tablelist = new List<Table>();
            tablelist.Add(table);
            return this.MapTables(tablelist);
        }

        public void RefreshList()
        {
            this.windows = new List<Window>();
            int docwindows = Convert.ToInt32(this.mapinfo.Evaluate("NumWindows()"));
            int otherwindows = Convert.ToInt32(this.mapinfo.Evaluate("NumAllWindows()"));

            for (int windownumber = 0; windownumber < docwindows; windownumber++)
            {
                int ID = Convert.ToInt32(this.mapinfo.Evaluate("WindowInfo({0},{1})".FormatWith(windownumber, (int)WindowInfo.Windowid)));
                Window window = new Window(ID, this.mapinfo);
                switch (window.Type)
                {
                    case WindowTypes.mapper:
                        this.windows.Add(new MapWindow(ID, this.mapinfo));
                        break;
                    case WindowTypes.Layout:
                        this.windows.Add(new LayoutWindow(ID, this.mapinfo));
                        break;
                    default:
                        this.windows.Add(window);
                        break;
                }
            }

            //for (int windownumber = -1; windownumber < otherwindows - 1; windownumber--)
            //{
            //    int ID = Convert.ToInt32(this.mapinfo.Evaluate("WindowInfo({0},{1})".FormatWith(windownumber, (int)WindowInfo.Windowid)));
            //    this.windows.Add(new Window(ID, this.mapinfo));
            //}
        }

        public Window this[int index]
        {
            get { return this.windows[index]; }
        }
    }
}
