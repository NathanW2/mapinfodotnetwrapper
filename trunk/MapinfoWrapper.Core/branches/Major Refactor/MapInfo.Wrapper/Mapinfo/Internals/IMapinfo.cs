using System.Runtime.InteropServices;

namespace Mapinfo.Wrapper.Mapinfo.Internals
{
    [InterfaceType(1)]
    [Guid("1D42EC62-7B28-11CE-B83D-00AA002C4F58")]
    public interface IMapInfo
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
        void RunCommand(string command);
        void RunMenuCommand(int id);
        void SetCallback(object callbackobject);
    }
}
