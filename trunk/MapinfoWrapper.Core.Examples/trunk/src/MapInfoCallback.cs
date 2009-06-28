using System;
using System.Runtime.InteropServices;
using MapinfoWrapper;

namespace Wrapper.Example.Callback
{
    /// <summary>
    /// This class inherits from the <see cref="T:MapinfoWrapper.MapinfoCallback"/> class in the wrapper,
    /// which provides the base events that Mapinfo uses with callbacks.
    /// 
    /// <para>This class will implement a custom event that we will call from a
    /// button pad calling the OLE callback method.</para>
    /// </summary>
    [UsesWrapper]
    [ComVisible(true)]
    public class CustomCallback : MapinfoCallback
    {
        public event Action<string> OnMenuItemClick;

        public void MenuItemHandler(string command)
        {
            // Store the event locally to save against a race condition.
            Action<string> menuEvent = OnMenuItemClick;
            if (menuEvent != null)
            {
                // Raise the event.
                menuEvent(command);
            }
        }
    }
}