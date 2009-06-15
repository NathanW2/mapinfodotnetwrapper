using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;


namespace MapinfoWrapper.Embedding
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
        /// <param name="wrapper">A instance of Mapinfo.</param>
        public static void SetAsMapinfoApplicationWindow(this Control value,IMapinfoWrapper wrapper)
        {
            wrapper.RunCommand("Set Application Window {0}".FormatWith(value.Handle.ToString()));
        }

        /// <summary>
        /// Sets the control as the parent for the next document window that is opened in Mapinfo.
        /// </summary>
        /// <param name="value">The control to which the next document will be the parent.</param>
        /// <param name="wrapper">A instance of Mapinfo.</param>
        /// <param name="windowStyle">The style of window when opened.</param>
        public static void SetAsNextDocumentParent(this Control value, IMapinfoWrapper wrapper,NextDocumentEnum windowStyle)
        {
            wrapper.RunCommand("Set Next Document Parent {0} Style {1}".FormatWith(value.Handle.ToString(),
                                                                                   (int)windowStyle));
        }
    }
}
