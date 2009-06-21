using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.Core.IoC;

namespace MapinfoWrapper.Core.Internals
{
    internal class VariableFactory : IVariableFactory
    {
        private readonly IMapinfoWrapper wrapper = IoC.IoC.Resolve<IMapinfoWrapper>();
        
        public VariableFactory() {}

        public IVariable CreateNewWithGUID(Variable.VariableType type)
        {
            Guid id = Guid.NewGuid();
            string striped = id.ToString().Replace("-","");
            int index = 0;
            foreach (char c in striped) {
                if (Char.IsLetter(c)) {
                    index = striped.IndexOf(c);
                    break;      
                }
            }
            string variablename = striped.Substring(index);

            string typename = Enum.GetName(typeof(Variable.VariableType), type);
            this.wrapper.RunCommand("Dim {0} as {1}".FormatWith(variablename, typename));
            
            Variable variable = new Variable(variablename,type,false);

            return variable;
        }

    }
}
