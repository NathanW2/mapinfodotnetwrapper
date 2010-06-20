using System;
using System.Windows.Forms;

namespace Mapinfo.Wrapper.Embedding
{
    /// <summary>
    /// A container to hold the handle for a mapinfo window.
    /// </summary>
    public class MapinfoWindowHandle : IWin32Window
    {
        IntPtr handle;

        /// <summary>
        /// Creates a object which holds a handle to a Mapinfo window. 
        /// </summary>
        /// <param name="mapinfoHandle"></param>
        public MapinfoWindowHandle(IntPtr mapinfoHandle)
        {
            this.handle = mapinfoHandle;     
        }

        /// <summary>
        /// Returns the handle of the mapinfo window in the form of a <see cref="System.IntPrt"/>
        /// </summary>
        IntPtr IWin32Window.Handle
        {
            get { return this.handle; }
        }
    }
}
