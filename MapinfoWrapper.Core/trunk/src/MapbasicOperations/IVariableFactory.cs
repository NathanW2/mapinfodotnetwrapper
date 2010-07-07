using MapinfoWrapper.MapbasicOperations;

namespace MapinfoWrapper.MapbasicOperations
{
    internal interface IVariableFactory
    {
        Variable DefineVariableWithGUID(VariableType type);
    }
}