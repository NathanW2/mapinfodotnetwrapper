using System.Windows.Forms;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;


namespace MapinfoWrapper.Embedding
{
    /// <summary>
    /// A collection of Mapinfo control based extensions.
    /// </summary>
    public static class ControlExtensions
    {
        private static readonly IMapinfoWrapper wrapper = ServiceLocator.GetInstance<IMapinfoWrapper>();
       
        /// <summary>
        /// Sets the control as the parent for any dialog boxs that are created in the specifed Mapinfo instance.
        /// If you need to re-parent a document window use <see cref="SetAsNextDocumentParent"/> instead.
        /// </summary>
        /// <param name="value">The control to which will be the parent.</param>
        public static void SetAsMapinfoApplicationWindow(this Control value)
        {
            wrapper.RunCommand("Set Application Window {0}".FormatWith(value.Handle.ToString()));
        }

        /// <summary>
        /// Sets the control as the parent for the next document window that is opened in Mapinfo.
        /// </summary>
        /// <param name="value">The control to which the next document will be the parent.</param>
        /// <param name="windowStyle">The style of window when opened.</param>
        public static void SetAsNextDocumentParent(this Control value,NextDocumentEnum windowStyle)
        {
            wrapper.RunCommand("Set Next Document Parent {0} Style {1}".FormatWith(value.Handle.ToString(),
                                                                                   (int)windowStyle));
        }
    }
}
