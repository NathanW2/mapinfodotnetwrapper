using System.Collections.Generic;

namespace MapInfo.Wrapper.Geometries
{
    public abstract class Geometry
    {
        private List<Coordinate> nodes = new List<Coordinate>();
        public List<Coordinate> Nodes
        {
            get
            {
                return this.nodes;
            }
            set
            {
                this.nodes = value;
            }
        }

        public abstract Coordinate Centroid();
        public abstract string ToBasicCreateCommand();
        public abstract string ToExtendedCreateString(string variableName);
        public abstract string ToExtendedCreateString(int windowID);
    }
}