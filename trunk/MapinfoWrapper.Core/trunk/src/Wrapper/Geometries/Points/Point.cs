namespace MapinfoWrapper.Geometries.Points
{
    using MapinfoWrapper.Geometries;

    /// <summary>
    /// Represents a point object.
    /// </summary>
    public sealed class Point
    {
        private Coordinate location;
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
    }
}
