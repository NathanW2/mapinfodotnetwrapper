namespace MapinfoWrapper.Geometries
{

    /// <summary>
    /// A structure for holding X and Y pair.
    /// </summary>
    public struct Coordinate
    {
        public Coordinate(double x, double y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; private set; }
        public double Y { get; private set; }

        public override bool Equals(object obj)
        {
            Coordinate point = (Coordinate)obj;
            return this.X == point.X && this.Y == point.Y;
        }

        public static bool operator ==(Coordinate point1, Coordinate point2)
        {
            return point1.Equals((Coordinate)point2);
        }

        public static bool operator !=(Coordinate point1, Coordinate point2)
        {
            return point2.Equals((Coordinate)point2);
        }
    }
}
