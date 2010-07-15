using System;
using MapInfo.Wrapper.Core.Extensions;
using MapInfo.Wrapper.Mapinfo;

namespace MapInfo.Wrapper.MapOperations
{
/// <summary>
	/// Repersents a Mapinfo Window object. 
    /// Can be used to query and set values for the window in Mapinfo.
	/// </summary>
    public class Window
    {
        protected readonly MapInfoSession map_info;

        public Window(int windowID, MapInfoSession map_info)
        {
            this.ID = windowID;
            this.map_info = map_info;
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
                    string strwindowHWND = this.map_info.Eval(string.Format("WindowInfo({0},{1})", this.ID, (int)WindowInfo.Wnd));
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
                    string frontwindowtype = this.map_info.Eval(String.Format("WindowInfo({0},{1})", this.ID, (int)WindowInfo.Type));
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
                return this.map_info.Eval("WindowInfo({0},{1})".FormatWith(this.ID, (int)WindowInfo.name));
            }
            set
            {
                this.map_info.Do("Set Window {0} Title {1}".FormatWith(this.ID, value.InQuotes()));
            }
        }

        /// <summary>
        /// Closes the current Map window in Mapinfo.
        /// </summary>
        public void Close()
        {
            this.map_info.Do("Close window {0}".FormatWith(this.ID));
        }

        /// <summary>
        /// Brings the window to the front in MapInfo.
        /// </summary>
        public void BringToFront()
        {
            this.map_info.Do("Set Window {0} Front".FormatWith(this.ID));
        }

        /// <summary>
        /// Clones the current window and returns the newly created window.
        /// </summary>
        /// <returns>A <see cref="Window"/> of the newly created clone.</returns>
        public Window CloneWindow()
        {
            this.map_info.Do(this.CloneWindowCommand);
            string sid = this.map_info.Eval("WindowID(0)");
            return new Window(Convert.ToInt32(sid), this.map_info);
        }
        
        /// <summary>
        /// Gets the clone window command used to clone the window.
        /// </summary>
        public string CloneWindowCommand
        {
            get
            {
                return this.map_info.Eval("WindowInfo({0},{1})".FormatWith(this.ID,(int)WindowInfo.Clonewindow));
            }
        }
    }
}
