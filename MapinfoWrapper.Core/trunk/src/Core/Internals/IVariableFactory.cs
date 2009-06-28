using MapinfoWrapper.MapbasicOperations;

namespace MapinfoWrapper.Core.Internals
{
    public interface IVariableFactory
    {
        IVariable CreateNewWithGUID(Variable.VariableType type);
    }
}