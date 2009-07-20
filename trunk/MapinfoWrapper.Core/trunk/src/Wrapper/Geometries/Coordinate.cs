using System;

namespace MapinfoWrapper.Geometries
{

    /// <summary>
    /// A structure for holding X and Y pair.
    /// </summary>
    public struct Coordinate : IEquatable<Coordinate>
    {
        public Coordinate(double x, double y) 
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; private set; }
        public double Y { get; private set; }

        public bool Equals(Coordinate other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            Coordinate point = (Coordinate)obj;
            return this.Equals(point);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 23) + this.X.GetHashCode();
            hash = (hash * 23) + this.Y.GetHashCode();
            return hash;
        }

        public static bool operator ==(Coordinate point1, Coordinate point2)
        {
            if ((object)point1 == null || ((object)point2 == null))
                return false;
            else return
                point1.Equals(point2);
        }

        public static bool operator !=(Coordinate point1, Coordinate point2)
        {
            return !(point1 == point2);
        }
    }
}
