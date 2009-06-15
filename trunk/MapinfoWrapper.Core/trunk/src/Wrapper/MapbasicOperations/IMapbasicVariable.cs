using System;

namespace MapinfoWrapper.MapbasicOperations
{
    public interface IMapbasicVariable : IDisposable, IVariableExtender
    {
        IMapbasicVariable AssignFromMapbasicCommand(string mapbasicExpression);
        string Name { get; set; }
        string ToString();
    }
}
