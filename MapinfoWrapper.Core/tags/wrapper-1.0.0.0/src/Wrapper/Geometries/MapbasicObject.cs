using System;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Exceptions;
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
        protected IMapinfoWrapper mapinfoinstance = ServiceLocator.GetInstance<IMapinfoWrapper>();
        private readonly IVariable innervariable;

        public MapbasicObject(IVariable variable)
            : this(variable,null)
        { }

        internal MapbasicObject(IVariable variable,IMapinfoWrapper wrapper)
        {
            this.innervariable = variable;
            this.mapinfoinstance = wrapper ?? ServiceLocator.GetInstance<IMapinfoWrapper>();
        }

        /// <summary>
        /// Run the ObjectInfo mapbasic command in Mapinfo, and returns a string containing the result.
        /// </summary>
        /// <param name="variable">The variable used by the ObjectInfo call.</param>
        /// <param name="attribute">The attribute specifying which information to return.</param>
        /// <returns>A string containing the retured result from calling the ObjectInfo command in Mapinfo.</returns>
        public static string ObjectInfo(IVariable variable, ObjectInfoEnum attribute)
        {
            IMapinfoWrapper wrapper = ServiceLocator.GetInstance<IMapinfoWrapper>();
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
                try
                {
                    int retured = Convert.ToInt32(this.ObjectInfo(ObjectInfoEnum.OBJ_INFO_TYPE));
                    return (ObjectTypeEnum)retured;
                }
                catch (MapinfoException mapex)
                {
                    if (mapex.MapinfoErrorCode == 1650)
                    {
                        return ObjectTypeEnum.NoObject;
                    }
                    throw;
                }
            }
        }

        public IVariable Variable
        {
            get { return this.innervariable; }
        } 
    }
}
