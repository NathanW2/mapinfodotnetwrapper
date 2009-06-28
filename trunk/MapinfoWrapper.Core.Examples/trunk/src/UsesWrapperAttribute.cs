using System;

namespace Wrapper.Example
{
    /// <summary>
    /// A attribute to mark that the method uses the Mapinfo OLE Wrapper.
    /// 
    /// This attribute is just for information purposes and doesn't do anything
    /// apart from to show which methods use the wrapper and which ones don't.
    /// </summary>
    public class UsesWrapperAttribute : Attribute
    {
        public UsesWrapperAttribute() { }
    }
}
