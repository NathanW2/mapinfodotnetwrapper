namespace MapinfoWrapper.Geometries
{
    using System;
    using MapinfoWrapper.Geometries;

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
            throw new System.NotImplementedException();
        }

        internal override string ToBasicCreateCommand()
        {
            throw new NotImplementedException();
        }
    }
}
