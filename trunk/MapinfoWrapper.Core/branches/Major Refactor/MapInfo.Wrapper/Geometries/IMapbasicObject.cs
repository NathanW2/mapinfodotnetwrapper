using Mapinfo.Wrapper.MapbasicOperations;

namespace Mapinfo.Wrapper.Geometries
{
    public interface IMapbasicObject
    {
        ObjectTypeEnum ObjectType { get; }
        IVariable Variable { get; }
    }
}
