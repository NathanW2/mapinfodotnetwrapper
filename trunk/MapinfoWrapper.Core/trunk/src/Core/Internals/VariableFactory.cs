namespace MapinfoWrapper.Core.Internals
{
    using System;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;

    internal class VariableFactory : IVariableFactory
    {
        private readonly MapinfoSession misession;

        public VariableFactory(MapinfoSession MISession) 
        {
            this.misession = MISession;
        }

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
            this.misession.RunCommand("Dim {0} as {1}".FormatWith(variablename, typename));
            
            Variable variable = new Variable(variablename,type,false,this.misession);

            return variable;
        }

    }
}
