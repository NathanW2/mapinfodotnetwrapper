using System;

namespace MapinfoWrapper.Geometries
{
    public interface IMapbasicObject
    {
        ObjectTypeEnum ObjectType { get; }
        string expression
        {
            get;
        }
    }
}
