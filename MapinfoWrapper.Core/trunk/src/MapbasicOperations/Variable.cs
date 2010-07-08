using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;
using System;
using MapinfoWrapper.Exceptions;

namespace MapinfoWrapper.MapbasicOperations
{
    /// <summary>
    /// Represents a Mapbasic variable.
    /// To declare a new variable use VariableFactory.
    /// </summary>
    public class Variable : IDisposable
    {
        private readonly MapinfoSession mapinfo;
        private readonly VariableType type;

        internal Variable(string name, VariableType declareAs,MapinfoSession miSession)
        {
            this.Name = name;
            this.type = declareAs;
            this.mapinfo = miSession;
        }

        /// <summary>
        /// Gets the name of the Mapbasic variable.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Checks to see if a variable has been declared in the current Mapbasic session.
        /// </summary>
        public bool IsDeclared
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Assigns the variable with the result from the mapbasic expression.  If the variable has become undelcared a <see cref="MapbasicVariableException"/> will be thrown.
        /// </summary>
        /// <param name="expression">A mapbasic expression from which the result will be assigned to variable.</param>
        /// <exception cref="MapbasicVariableException"/>
        /// <exception cref="MapinfoException" />
        public void Assign(string expression)
        {
            if (!this.IsDeclared)
            {
                throw new MapbasicVariableException(@"Variable {0} is not declared, or is in a invaild state and can not be assigned. 
                                                      Please dispose and recreate using VariableFactory".FormatWith(this.Name));
            }
        
            this.mapinfo.Do("{0} = {1}".FormatWith(this.Name, expression));
        }

        /// <summary>
        /// Undims the Mapbasic variable in the Mapbasic session.
        /// </summary>
        public void Dispose()
        {
            // If it is not delcared then we can just return.
            if (!this.IsDeclared)
            {
                return;
            }

            this.mapinfo.Do("UnDim {0}".FormatWith(this.Name));
        }
    }

    public enum VariableType
    {
        Object
    }
}
