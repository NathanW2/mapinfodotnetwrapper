using Mapinfo.Wrapper.Core.Extensions;
using Mapinfo.Wrapper.MapbasicOperations;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.Geometries.Points
{
    /// <summary>
    /// Represents a point object in Mapinfo.
    /// </summary>
    public sealed class Point : Geometry
    {
        public Point(Variable variable,IMapinfoWrapper session)
            : base(variable,session)
        { }

        public override string ToBasicCreateCommand()
        {
            return "CreatePoint(CentroidX({0}),CentroidY({0}))".FormatWith(base.Variable.Name);
        }

        public override string ToExtendedCreateString(string variableName)
        {
            return "Create Point Into Variable {0} (CentroidX({1}),CentroidY({1})) {2}".FormatWith(variableName,
                                                               this.Variable.Name,
                                                               this.Style);
        }

        public override string ToExtendedCreateString(int windowID)
        {
            return "Create Point Into Window {0} (CentroidX({1}),CentroidY({1})) {2}".FormatWith(windowID,
                                                   this.Variable.Name,
                                                   this.Style);
        }

        public string Style { get; set; }
    }
}
