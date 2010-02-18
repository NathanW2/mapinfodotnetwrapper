using MapinfoWrapper.MapbasicOperations;

namespace MapinfoWrapper.Core.Internals
{
    internal interface IVariableFactory
    {
        Variable DefineVariableWithGUID(VariableType type);
    }
}