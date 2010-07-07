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
                    string strwindowHWND = this.mapinfo.Eval(string.Format("WindowInfo({0},{1})", this.ID, (int)WindowInfo.Wnd));
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
                    string frontwindowtype = this.mapinfo.Eval(String.Format("WindowInfo({0},{1})", this.ID, (int)WindowInfo.Type));
                    this.type = (WindowTypes)(Convert.ToInt32(frontwindowtype));
                }
                return this.type;
            }
        }

        /// <summary>
        /// Gets or sets the name/title of the window.
        /// </summary>
        public string Name
        {
            get
            {
                return this.mapinfo.Eval("WindowInfo({0},{1})".FormatWith(this.ID, (int)WindowInfo.name));
            }
            set
            {
                this.mapinfo.Do("Set Window {0} Title {1}".FormatWith(this.ID, value.InQuotes()));
            }
        }

        /// <summary>
        /// Closes the current Map window in Mapinfo.
        /// </summary>
        public void Close()
        {
            this.mapinfo.Do("Close window {0}".FormatWith(this.ID));
        }

        /// <summary>
        /// Brings the window to the front in MapInfo.
        /// </summary>
        public void BringToFront()
        {
            this.mapinfo.Do("Set Window {0} Front".FormatWith(this.ID));
        }

        /// <summary>
        /// Clones the current window and returns the newly created window.
        /// </summary>
        /// <returns>A <see cref="Window"/> of the newly created clone.</returns>
        public Window CloneWindow()
        {
            this.mapinfo.Do(this.CloneWindowCommand);
            string sid = this.mapinfo.Eval("WindowID(0)");
            return new Window(Convert.ToInt32(sid), this.mapinfo);
        }
        
        /// <summary>
        /// Gets the clone window command used to clone the window.
        /// </summary>
        public string CloneWindowCommand
        {
            get
            {
                return this.mapinfo.Eval("WindowInfo({0},{1})".FormatWith(this.ID,(int)WindowInfo.Clonewindow));
            }
        }
    }
}
