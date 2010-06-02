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

        /// <summary>
        /// Gets the current front window in MapInfo.
        /// </summary>
        public Window FrontWindow
        {
            get
            {
                int windowid = Convert.ToInt32(this.mapinfo.Eval("FrontWindow()"));
                return new Window(windowid, this.mapinfo);
            }
        }

        /// <summary>
        /// Returns the <see cref="IEnumerator"/> for the window collection.
        /// </summary>
        /// <returns>s</returns>
        public IEnumerator<Window> GetEnumerator()
        {
            int docwindows = Convert.ToInt32(this.mapinfo.Eval("NumWindows()"));
            
            for (int windownumber = 1; windownumber < docwindows + 1; windownumber++)
            {
                int ID = Convert.ToInt32(this.mapinfo.Eval("WindowInfo({0},{1})".FormatWith(windownumber, (int)WindowInfo.Windowid)));
                Window window = new Window(ID, this.mapinfo);
                switch (window.Type)
                {
                    case WindowTypes.mapper:
                        yield return new MapWindow(ID, this.mapinfo);
                        break;
                    case WindowTypes.Layout:
                        yield return new LayoutWindow(ID, this.mapinfo);
                        break;
                    default:
                        yield return window;
                        break;
                }
            }

            // TODO: Special windows eg toolbars, info etc are not handle correctly yet. Not sure if they need to be implemented here.

            //int otherwindows = Convert.ToInt32(this.mapinfo.Eval("NumAllWindows()"));
            //for (int windownumber = -1; windownumber < otherwindows - 1; windownumber--)
            //{
            //    int ID = Convert.ToInt32(this.mapinfo.Evaluate("WindowInfo({0},{1})".FormatWith(windownumber, (int)WindowInfo.Windowid)));
            //    this.windows.Add(new Window(ID, this.mapinfo));
            //}
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Gets the window from Mapinfo using the window ID.
        /// </summary>
        /// <param name="name">The ID of the window to return from MapInfo.</param>
        /// <returns>A new <see cref="Window"/> where the ID equals the ID supplied.</returns>
        public Window this[int windowID]
        {
            get
            {
                return new Window(windowID, this.mapinfo);
            }
        }

        /// <summary>
        /// Gets the window from Mapinfo using the name.
        /// </summary>
        /// <param name="name">The name of the window to return from MapInfo.</param>
        /// <returns>A new <see cref="Window"/> where the name equals the name supplied.</returns>
        public Window this[string name]
        {
            get
            {
                return this.Where(win => win.Name == name).FirstOrDefault();
            }
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the tables supplied as the layers for that map.
        /// </summary>
        /// <param name="tablelist">A collection of tables which will be used in the new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window.</returns>
        public MapWindow CreateMapFromTables(IEnumerable<Table> tablelist)
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
            this.mapinfo.Do(command);
            return new MapWindow(this.FrontWindow.ID, this.mapinfo);
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the table supplied as the first layer. 
        /// </summary>
        /// <param name="table">The table which will be opened in a new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window. </returns>
        public MapWindow CreateMapFromTable(Table table)
        {
            List<Table> tablelist = new List<Table>();
            tablelist.Add(table);
            return this.CreateMapFromTables(tablelist);
        }
    }
}
