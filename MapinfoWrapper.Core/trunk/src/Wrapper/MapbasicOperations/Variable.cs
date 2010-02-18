namespace MapinfoWrapper.MapbasicOperations
{
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Mapinfo;
    using System;
    using MapinfoWrapper.Exceptions;

    /// <summary>
    /// Represents a Mapbasic variable.
    /// To create a new variable use VariableFactory.
    /// </summary>
    public class Variable
    {
        private readonly MapinfoSession misession;
        private readonly VariableType type;

        internal Variable(string name, VariableType declareAs,bool isAssigned,MapinfoSession MISession)
        {
            this.Name = name;
            this.type = declareAs;
            this.IsAssigned = isAssigned;
            this.misession = MISession;
        }

        public string Name { get; private set; }

        public virtual string GetExpression()
        {
            return this.Name;
        }

        public bool IsAssigned {get; private set;}

        public void Assign(string expression)
        {
            try
            {
                this.misession.RunCommand("{0} = {1}".FormatWith(this.GetExpression(), expression));
                this.IsAssigned = true;
            }
            catch (MapinfoException mapinofex)
            {
                
                throw;
            }
        }
    }

    public enum VariableType
    {
        Object
    }
}
