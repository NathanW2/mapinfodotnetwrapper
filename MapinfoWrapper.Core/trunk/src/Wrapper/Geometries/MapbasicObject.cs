using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.IoC;
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
        protected IMapinfoWrapper mapinfoinstance = IoC.Resolve<IMapinfoWrapper>();
        private readonly IVariable innervariable;

        public MapbasicObject(IVariable variable)
        {
            this.innervariable = variable;
        }

        /// <summary>
        /// Run the ObjectInfo mapbasic command in Mapinfo, and returns a string containing the result.
        /// </summary>
        /// <param name="wrapper">The instance of Mapinfo to run the command in.</param>
        /// <param name="variable">The variable used by the ObjectInfo call.</param>
        /// <param name="attribute">The attribute specifying which information to return.</param>
        /// <returns>A string containing the retured result from calling the ObjectInfo command in Mapinfo.</returns>
        public static string ObjectInfo(IVariable variable, ObjectInfoEnum attribute)
        {
            IMapinfoWrapper wrapper = IoC.Resolve<IMapinfoWrapper>();
            string expression = variable.GetExpression();
            string returnedstring = wrapper.Evaluate("ObjectInfo({0},{1})".FormatWith(expression, (int)attribute));
            return returnedstring;
        }

        /// <summary>
        /// Runs the ObjectInfo mapbasic command in Mapinfo, and returns a string containing the result.
        /// </summary>
        /// <param name="attribute">The attribute specifying which information to return.</param>
        /// <returns>A string containing the returned result from calling the ObjectInfo command in Mapinfo.</returns>
        public string ObjectInfo(ObjectInfoEnum attribute)
        {
            return MapbasicObject.ObjectInfo(this.Variable, attribute);
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

        public IVariable Variable
        {
            get { return this.innervariable; }
        } 
    }
}
