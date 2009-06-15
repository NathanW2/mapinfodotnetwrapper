using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.MapbasicOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.Geometries
{
    /// <summary>
    /// Provides a wrapper around a mapbasic object type, allows access to basic information about the object.
    /// </summary>
    public class MapbasicObject : IMapbasicObject
    {
        protected IMapinfoWrapper mapinfoinstance;
        private IVariableExtender variable;

        public MapbasicObject(IMapinfoWrapper wrapper,IVariableExtender variable)
        {
            this.mapinfoinstance = wrapper;
            this.variable = variable;
        }

        /// <summary>
        /// Run the ObjectInfo mapbasic command in Mapinfo, and returns a string containing the result.
        /// </summary>
        /// <param name="wrapper">The instance of Mapinfo to run the command in.</param>
        /// <param name="variable">The variable used by the ObjectInfo call.</param>
        /// <param name="attribute">The attribute specifying which information to return.</param>
        /// <returns>A string containing the retured result from calling the ObjectInfo command in Mapinfo.</returns>
        public static string ObjectInfo(IMapinfoWrapper wrapper, IVariableExtender variable, ObjectInfoEnum attribute)
        {
            string returnedstring = wrapper.Evaluate("ObjectInfo({0},{1})".FormatWith(variable.ObjectExpression, (int)attribute));
            return returnedstring;
        }

        /// <summary>
        /// Runs the ObjectInfo mapbasic command in Mapinfo, and returns a string containing the result.
        /// </summary>
        /// <param name="attribute">The attribute specifying which information to return.</param>
        /// <returns>A string containing the returned result from calling the ObjectInfo command in Mapinfo.</returns>
        public string ObjectInfo(ObjectInfoEnum attribute)
        {
            return MapbasicObject.ObjectInfo(this.mapinfoinstance, this.variable, attribute);
        }

        /// <summary>
        /// Gets the type of the object in Mapinfo.
        /// </summary>
        /// <returns>The type of the object.</returns>
        public ObjectTypeEnum ObjectType
        {
            get
            {
                int retured = Convert.ToInt32(this.ObjectInfo(ObjectInfoEnum.OBJ_INFO_TYPE));
                return (ObjectTypeEnum)retured;
            }
        }

        public string expression 
        {
            get 
            {
                return this.variable.ObjectExpression;
            }
        }
    }
}
