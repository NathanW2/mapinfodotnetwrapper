using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Core.Extensions;

namespace MapinfoWrapper.MapbasicOperations
{
    public class Variable : IVariable
    {
        private readonly IMapinfoWrapper wrapper;
        private readonly string name;
        private readonly VariableType type;
        private readonly bool isAssigned;

        public Variable(string name, VariableType declareAs,bool isAssigned)
            : this(name,declareAs,isAssigned,null)
        { }

        internal Variable(string name, VariableType declareAs,bool isAssigned,IMapinfoWrapper wrapper)
        {
            this.name = name;
            this.type = declareAs;
            this.isAssigned = isAssigned;
            this.wrapper = wrapper ?? ServiceLocator.GetInstance<IMapinfoWrapper>();
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
            this.wrapper.RunCommand("{0} = {1}".FormatWith(this.GetExpression(), expression));
            return this;
        }

        public enum VariableType
        {
            Object
        }
    }
}
