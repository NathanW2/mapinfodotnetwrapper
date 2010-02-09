using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Exceptions;
using MapinfoWrapper.Core;
using MapinfoWrapper.Core.Extensions;

namespace MapinfoWrapper.MapOperations
{
/// <summary>
	/// Repersents a Mapinfo Window object. 
    /// Can be used to query and set values for the window in Mapinfo.
	/// </summary>
    public class Window
    {
        protected readonly MapinfoSession mapinfo;

        public Window(int windowID, MapinfoSession mapinfo)
        {
            this.ID = windowID;
            this.mapinfo = mapinfo;
        }

        /// <summary>
        /// The ID of the current window.
        /// </summary>
        public int ID { get; private set; }

        private IntPtr handle;
        /// <summary>
        /// Gets the handle to the window.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                if (this.handle == IntPtr.Zero)
                {
                    string strwindowHWND = this.mapinfo.Evaluate(string.Format("WindowInfo({0},{1})", this.ID, (int)WindowInfo.Wnd));
                    this.handle = new IntPtr(Convert.ToInt32(strwindowHWND));
                }
                return this.handle;
            }
        }

        private WindowTypes type;
        /// <summary>
        /// Gets the window type.
        /// </summary>
        public WindowTypes Type
        {
            get
            {
                if (this.type == 0)
                {
                    string frontwindowtype = this.mapinfo.Evaluate(String.Format("WindowInfo({0},{1})", this.ID, (int)WindowInfo.Type));
                    this.type = (WindowTypes)(Convert.ToInt32(frontwindowtype));
                }
                return this.type;
            }
        }

        /// <summary>
        /// Closes the current Map window in Mapinfo.
        /// </summary>
        public void CloseWindow()
        {
            this.mapinfo.RunCommand("Close window {0}".FormatWith(this.ID));
            // TODO Dispose of Mapwindow here.
        }
    }
}
