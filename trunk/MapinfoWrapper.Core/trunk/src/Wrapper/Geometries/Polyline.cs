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

        internal override string ToBasicCreateCommand()
        {
            throw new NotSupportedException("The polyline class does not have a basic create string");
        }

        internal string ToFullCreateCommand(string variableName)
        {
            string createstring = "Create Pline Into Varaible {0} {1}".FormatWith(variableName,this.Nodes.Length);
            string nodesstring = String.Empty;
            foreach (var node in this.Nodes)
            {
                nodesstring += "({0}) ".FormatWith(node.ToString());               
            }
            return createstring + nodesstring;
        }
    }
}
