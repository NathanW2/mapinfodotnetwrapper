using System.Runtime.InteropServices;

namespace MapInfo.Wrapper.Core.COMInterfaces
{
    [InterfaceType(1)]
    [Guid("D38D86C9-1A6C-4670-B8A9-868E9D74B77E")]
    public interface IMapInfo2 : IMapInfo
    {
        [DispId(1610678272)]
        object Application { get; }
        [DispId(1610678275)]
        string FullName { get; }
        [DispId(1610678279)]
        int LastErrorCode { get; set; }
        [DispId(1610678281)]
        string LastErrorMessage { get; }
        [DispId(1610678282)]
        object MBApplications { get; }
        [DispId(1610678290)]
        object MIMapGen { get; }
        [DispId(0)]
        string Name { get; }
        [DispId(1610678273)]
        object Parent { get; }
        [DispId(1610678289)]
        int ProductLevel { get; }
        [DispId(1610678276)]
        string Version { get; }
        [DispId(1610678277)]
        bool Visible { get; set; }

        object DataObject(int windowID);
        void Do(string command);
        string Eval(string expression);
        void RegisterCallback(object callbackobject);
        void RunCommand(string command);
        void RunMenuCommand(int id);
        void SetCallback(object callbackobject);
        void SetCallbackEvents(object callbackobject, int eventFlags);
        void UnregisterCallback(object callbackobject);
    }
}
