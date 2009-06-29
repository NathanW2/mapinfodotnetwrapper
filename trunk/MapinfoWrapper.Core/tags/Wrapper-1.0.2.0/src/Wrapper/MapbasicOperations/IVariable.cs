namespace MapinfoWrapper.MapbasicOperations
{
    public interface IVariable
    {
        string GetExpression();
        bool IsAssigned { get; }
        IVariable Assign(string expression);
    }
}