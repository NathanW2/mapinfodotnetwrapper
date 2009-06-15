using System;

namespace Wrapper.ObjectOperations
{
    public interface IMapbasicObject
    {
        ObjectTypeEnum ObjectType { get; }
        string VariableName { get; }
    }
}
