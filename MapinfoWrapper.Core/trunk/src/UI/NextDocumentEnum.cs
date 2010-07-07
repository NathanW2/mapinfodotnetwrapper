using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.UI
{
    public enum NextDocumentEnum
    {
        /// <summary>
        /// Resets the style of the window back to its default.
        /// </summary>
        WIN_STYLE_STANDARD = 0,
        /// <summary>
        /// Next window is created as a child window.
        /// </summary>
        WIN_STYLE_CHILD = 1,
        /// <summary>
        /// Next window is created as a popup window
        /// with a full-height title bar caption.
        /// </summary>
        WIN_STYLE_POPUP_FULLCAPTION = 2,
        /// <summary>
        /// Next window is created as a popup window with a half-height
        /// title bar caption.
        /// </summary>
        WIN_STYLE_POPUP = 3
    }
}
