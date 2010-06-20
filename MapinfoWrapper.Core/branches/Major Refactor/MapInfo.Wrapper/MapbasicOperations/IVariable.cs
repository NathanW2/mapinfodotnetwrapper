namespace Mapinfo.Wrapper.MapbasicOperations
{
    public interface IVariable
    {
        string GetExpression();
        bool IsAssigned { get; }
        void Assign(string expression);
    }
}