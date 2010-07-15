﻿using MapInfo.Wrapper.Core.Extensions;
using MapInfo.Wrapper.Mapinfo;
using System.Windows.Forms;

namespace MapInfo.Wrapper.UI
{
    /// <summary>
    /// A collection of Mapinfo control based extensions.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Sets the control as the parent for any dialog boxs that are created in the specifed Mapinfo instance.
        /// If you need to re-parent a document window use <see cref="SetAsNextDocumentParent"/> instead.
        /// </summary>
        /// <param name="value">The control to which will be the parent.</param>
        public static void SetAsMapinfoApplicationWindow(this Control value,MapInfoSession MISession)
        {
            MISession.Do("Set Application Window {0}".FormatWith(value.Handle.ToString()));
        }

        /// <summary>
        /// Sets the control as the parent for the next document window that is opened in Mapinfo.
        /// </summary>
        /// <param name="value">The control to which the next document will be the parent.</param>
        /// <param name="windowStyle">The style of window when opened.</param>
        public static void SetAsNextDocumentParent(this Control value,MapInfoSession MISession, NextDocumentEnum windowStyle)
        {
            MISession.Do("Set Next Document Parent {0} Style {1}".FormatWith(value.Handle.ToString(),
                                                                                   (int)windowStyle));
        }
    }
}
