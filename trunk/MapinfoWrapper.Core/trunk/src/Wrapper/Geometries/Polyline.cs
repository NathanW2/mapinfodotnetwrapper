using MapinfoWrapper.Geometries;
using MapinfoWrapper.MapbasicOperations;
using System;
using MapinfoWrapper.Core.Extensions;

namespace MapinfoWrapper.Wrapper.Geometries
{
    public class Polyline : Geometry
    {
        private Coordinate[] nodes;
        public Coordinate[] Nodes
        {
            get
            {
                return this.nodes;
            }
            set
            {
                if (value != this.nodes)
                {
                    this.nodes = value;
                    this.Dirty = true;
                }
            }
        }

        public bool Dirty { get; internal set; }

        public override Coordinate Centroid()
        {
            throw new System.NotImplementedException();
        }

        public override string ToBasicCreateCommand()
        {
            throw new NotSupportedException("The polyline class does not have a basic create string");
        }

        public override string ToExtendedCreateString(string variableName)
        {
            string createstring = "Create Pline Into Varaible {0} {1}".FormatWith(variableName, this.Nodes.Length);
            string nodesstring = String.Empty;
            foreach (var node in this.Nodes)
            {
                nodesstring += "({0}) ".FormatWith(node.ToString());
            }
            return createstring + nodesstring;
        }

        public override string ToExtendedCreateString(int windowID)
        {
            string createstring = "Create Pline Into Window {0} {1}".FormatWith(windowID, this.Nodes.Length);
            string nodesstring = String.Empty;
            foreach (var node in this.Nodes)
            {
                nodesstring += "({0}) ".FormatWith(node.ToString());
            }
            return createstring + nodesstring;
        }
    }
}
