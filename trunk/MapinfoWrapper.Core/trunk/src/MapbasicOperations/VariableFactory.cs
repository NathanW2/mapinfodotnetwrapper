namespace MapinfoWrapper.MapbasicOperations
{
    using System;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.MapbasicOperations;
    using MapinfoWrapper.Mapinfo;

    public class VariableFactory
    {
        private readonly MapinfoSession misession;

        public VariableFactory(MapinfoSession mapinfoSession) 
        {
            this.misession = mapinfoSession;
        }

        public Variable DefineVariableWithGUID(VariableType type, string expression)
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

            return this.DefineVariable(variablename, type, expression);
        }

        /// <summary>
        /// Declares a new variable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Variable DefineVariable(string name, VariableType type, string expression)
        {
            try
            {
                string typename = Enum.GetName(typeof(VariableType), type);
                this.misession.Do("Dim {0} as {1} /n".FormatWith(name, typename,expression));

                return new Variable(name, type, this.misession);
            }
            catch (Exceptions.MapinfoException mapinfoex)
            {
                if (mapinfoex.MapinfoErrorCode == 579)
                {
                    throw new Exceptions.MapbasicVariableException("Variable with name {0} has already been defined.".FormatWith(name), mapinfoex);
                }
                throw;
            }
        }
    }
}
