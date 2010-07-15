using MapInfo.Wrapper.Core.Extensions;
using System;

namespace MapInfo.Wrapper.Geometries.Points
{
    /// <summary>
    /// Represents a point object.
    /// </summary>
    [Serializable]
    public sealed class Point : Geometry
    {
        public Point()
        {
            this.Position = new Coordinate();
        }

        public Point(double x, double y)
        {
            this.Position = new Coordinate(x, y);
        }

        public Point(Coordinate location)
        {
            this.Position = location;
        }

        public double X
        {
            get { return this.Position.X; }
        }

        public double Y
        {
            get { return this.Position.Y; }
        }

        public Coordinate Position
        {
            get { return this.Nodes[0]; }
            set { this.Nodes[0] = value; }
        }

        private string style;
        public string Style
        {
            get 
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        public override Coordinate Centroid()
        {
            return this.Position;
        }

        public override string ToBasicCreateCommand()
        {
            return "CreatePoint({0},{1})".FormatWith(X.ToString(), Y.ToString());
        }

        public override string ToExtendedCreateString(string variableName)
        {
            return "Create Point Into Variable {0} ({1},{2}) {3}".FormatWith(variableName,
                                                               this.Position.X,
                                                               this.Position.Y,
                                                               this.Style);
        }

        public override string ToExtendedCreateString(int windowID)
        {
            return "Create Point Into Window {0} ({1},{2}) {3}".FormatWith(windowID,
                                                   this.Position.X,
                                                   this.Position.Y,
                                                   this.Style);
        }
    }
}
