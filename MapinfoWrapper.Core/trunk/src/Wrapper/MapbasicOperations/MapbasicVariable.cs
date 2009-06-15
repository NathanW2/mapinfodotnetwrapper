using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.MapbasicOperations
{
    /// <summary>
    /// A wrapper for the Mapbasic variable type object.
    /// </summary>
    public class MapbasicVariable : IMapbasicVariable
    {
        private IMapinfoWrapper mapinfoinstace;

        private MapbasicVariable(IMapinfoWrapper wrapper, string variableName,string declareAs)
        {
            this.mapinfoinstace = wrapper;
            this.Name = variableName;
            this.DeclaredAs = declareAs;
        }

        /// <summary>
        /// Returns the name of the variable.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Reutrns the type the variable has been declared as.
        /// </summary>
        public string DeclaredAs { get; private set; }

        public static IMapbasicVariable Declare(string variableName, string type)
        {
            return MapbasicVariable.Declare(IoC.Resolve<IMapinfoWrapper>(), variableName, type);
        }

        /// <summary>
        /// Declares a variable with the specifed name in Mapbasic.
        /// </summary>
        /// <param name="wrapper">An instance of Mapinfo.</param>
        /// <param name="variableName">The name a the variable to declare.</param>
        /// <param name="typeToBeDeclared">The type the variable will be delcared as.</param>
        /// <returns>A instance of <see cref="T:MBObjectVariable"/>.</returns>
        public static IMapbasicVariable Declare(IMapinfoWrapper wrapper, string variableName, string typeToBeDeclared)
        {
            if (String.IsNullOrEmpty(variableName))
                throw new ArgumentNullException("variableName", "Variable name can not be null");

            wrapper.RunCommand("Dim {0} as {1}".FormatWith(variableName,typeToBeDeclared));
            return new MapbasicVariable(wrapper, variableName,typeToBeDeclared);
        }

        /// <summary>
        /// Returns a string containing the name of the type of object.
        /// </summary>
        /// <example>If MBObjectVariable is assigned with a region object the return value will be "Region"</example>
        /// <returns>The type name of the object.</returns>
        public override string ToString()
        {
            string value = this.mapinfoinstace.Evaluate(this.Name);
            return value;
        }
    
        /// <summary>
        /// Calls the Mapbasic "Undim" command on the
        /// variable.
        /// </summary>
        public void Dispose()
        {
            this.mapinfoinstace.RunCommand("Undim {0}".FormatWith(this.Name));
        }
        
        /// <summary>
        /// Assigns the variable with the result from a Mapbasic expression.
        /// </summary>
        /// <example>AssignFromMapbasicCommand("CreatePoint(12345.122,454545.323)").
        /// Will assign the variable with the return result from the Mapbasic CreatePoint function.</example>
        /// <param name="mapbasicExpression">An expression that will run in Mapinfo and be assigned to the variable.</param>
        /// <returns>The current instance of <see cref="T:MBObjectVariable"/></returns>
        public IMapbasicVariable AssignFromMapbasicCommand(string mapbasicExpression)
        {
            this.mapinfoinstace.RunCommand("{0} = {1}".FormatWith(this.Name,mapbasicExpression));
            return this;
        }


        public string ObjectExpression
        {
            get { return this.Name; }
        }
    }
}
