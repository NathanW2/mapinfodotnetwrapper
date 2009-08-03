﻿namespace MapinfoWrapper.MapbasicOperations
{
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Mapinfo;

    /// <summary>
    /// Represents a Mapbasic variable. 
    /// <para><b>You will not be able to create one these as the constructor is internal.
    /// Any methods that need a Mapbasic variable will create one using an internal Variable Factory.
    /// This is so that you will not have to worry about managment of variable in Mapinfo yourself.</b></para>
    /// </summary>
    public class Variable : IVariable
    {
        private readonly MapinfoSession misession;
        private readonly string name;
        private readonly VariableType type;
        private readonly bool isAssigned;

        internal Variable(string name, VariableType declareAs,bool isAssigned,MapinfoSession MISession)
        {
            this.name = name;
            this.type = declareAs;
            this.isAssigned = isAssigned;
            this.misession = MISession;
        }

        public virtual string GetExpression()
        {
            return this.name;
        }

        public bool IsAssigned
        {
            get { return this.isAssigned; }
        }

        public IVariable Assign(string expression)
        {
            this.misession.RunCommand("{0} = {1}".FormatWith(this.GetExpression(), expression));
            return this;
        }

        public enum VariableType
        {
            Object
        }
    }
}
