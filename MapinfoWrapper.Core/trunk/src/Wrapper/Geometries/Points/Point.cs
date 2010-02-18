namespace MapinfoWrapper.Geometries
{
    using System;
    using MapinfoWrapper.Geometries;
    using MapinfoWrapper.Core.Extensions;

    /// <summary>
    /// Represents a point object.
    /// </summary>
    [Serializable]
    public sealed class Point : Geometry
    {
        private Coordinate location;

        public Point()
        {
            this.Position = new Coordinate();
        }

        public Point(double x, double y)
        {
            this.location = new Coordinate(x, y);
        }

        public Point(Coordinate location)
        {
            this.location = location;
        }

        public double X
        {
            get { return this.location.X; }
        }

        public double Y
        {
            get { return this.location.Y; }
        }

        public Coordinate Position
        {
            get { return this.location; }
            set { this.location = value; }
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
