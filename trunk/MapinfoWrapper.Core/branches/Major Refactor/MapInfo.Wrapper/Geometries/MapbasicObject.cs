using MapInfo.Wrapper.Core.Exceptions;
using Mapinfo.Wrapper.Exceptions;
using Mapinfo.Wrapper.MapbasicOperations;
using System;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.Geometries
{
    /// <summary>
    /// Provides a wrapper around a mapbasic object type,
    /// allows access to basic information about the object.
    /// </summary>
    public class ObjectVariable : IMapbasicObject
    {
        private readonly IVariable innervariable;
        protected readonly MapinfoSession misession;

        internal ObjectVariable(MapinfoSession MISession,IVariable variable)
        {
            this.innervariable = variable;
            this.misession = MISession;
        }

        /// <summary>
        /// Runs the ObjectInfo mapbasic command in Mapinfo, and returns a string containing the result.
        /// </summary>
        /// <param name="attribute">The attribute specifying which information to return.</param>
        /// <returns>A string containing the returned result from calling the ObjectInfo command in Mapinfo.</returns>
        public object ObjectInfo(ObjectInfoEnum attribute)
        {
            throw new NotImplementedException();
            //return this.misession.ObjectInfo(this.Variable, attribute);
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
