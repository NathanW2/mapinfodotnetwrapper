using System;
using System.Runtime.InteropServices;

namespace Wrapper.Example.Callback
{
    /// <summary>
    /// This class inherits from the <see cref="T:MapinfoCallback"/> class in the wrapper,
    /// which provides the base events that Mapinfo uses with callbacks.
    /// 
    /// <para>This class will implement a custom event that we will call from a
    /// button pad calling the OLE callback method.</para>
    /// </summary>
    [UsesWrapper]
    [ComVisible(true)]
    public class CustomCallback : Wrapper.MapinfoCallback
    {
        public event Action<string> OnMenuItemClick;

        public void MenuItemHandler(string command)
        {
            // Store the event locally to save a race condition when using threading.
            Action<string> menu = OnMenuItemClick;
            if (menu != null)
            {
                // Raise the event.
                menu(command);
            }
        }
    }
}