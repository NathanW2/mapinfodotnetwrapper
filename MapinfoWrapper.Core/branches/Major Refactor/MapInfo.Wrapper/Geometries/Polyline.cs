using System;

namespace Mapinfo.Wrapper.Geometries
{
    public class Polyline : Geometry
    {
        public override string ToBasicCreateCommand()
        {
            throw new NotSupportedException("The polyline class does not have a basic create string");
        }

        public override string ToExtendedCreateString(string variableName)
        {
            string createstring = "Create Pline Into Variable {0} {1} ".FormatWith(variableName, this.Nodes.Count);
            string nodesstring = String.Empty;
            foreach (var node in base.Nodes)
            {
                nodesstring += " ({0}) ".FormatWith(node.ToString());
            }
            return createstring + nodesstring;
        }

        public override string ToExtendedCreateString(int windowID)
        {
            string createstring = "Create Pline Into Window {0} {1} ".FormatWith(windowID, this.Nodes.Count);
            string nodesstring = String.Empty;
            foreach (var node in base.Nodes)
            {
                nodesstring += " ({0}) ".FormatWith(node.ToString());
            }
            return createstring + nodesstring;
        }
    }
}
