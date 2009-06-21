using MapinfoWrapper.MapbasicOperations;

namespace MapinfoWrapper.Core.Internals
{
    internal interface IVariableFactory
    {
        IVariable CreateNewWithGUID(Variable.VariableType type);
    }
}