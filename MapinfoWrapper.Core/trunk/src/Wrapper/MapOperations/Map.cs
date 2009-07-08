using System;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.MapOperations
{
    public class MapWindow
    {
        private MapinfoSession misession;
        private int mapnumber;

        public MapWindow(MapinfoSession wrapper, int mapNumber)
        {
            this.misession = wrapper;
            this.mapnumber = mapNumber;
        }

        /// <summary>
        /// Returns the window id for the current window.
        /// </summary>
        public int WindowId
        {
            get
            {
                return Convert.ToInt32(this.misession.Evaluate("WindowID({0})".FormatWith(this.mapnumber)));
            }
        }

        /// <summary>
        /// Returns the handle for the current Map window.
        /// </summary>
        public IntPtr Hwnd
        {
            get
            {
                return (IntPtr)long.Parse(this.misession.Evaluate("WindowInfo({0},12)".FormatWith(this.mapnumber)));
            }
        }

        /// <summary>
        /// Closes the current Map window in Mapinfo.
        /// </summary>
        public void CloseWindow()
        {
            this.misession.RunCommand("Close window {0}".FormatWith(this.WindowId));
            // TODO Dispose of Mapwindow here.
        }
    }
}
