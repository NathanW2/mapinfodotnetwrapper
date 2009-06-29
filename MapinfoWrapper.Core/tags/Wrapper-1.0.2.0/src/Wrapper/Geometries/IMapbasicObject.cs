using System;
using MapinfoWrapper.MapbasicOperations;

namespace MapinfoWrapper.Geometries
{
    public interface IMapbasicObject
    {
        ObjectTypeEnum ObjectType { get; }
        IVariable Variable { get; }
    }
}
