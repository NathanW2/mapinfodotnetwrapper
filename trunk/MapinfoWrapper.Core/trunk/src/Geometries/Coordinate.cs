using MapInfo.Wrapper.Core.Extensions;
using System;

namespace MapInfo.Wrapper.Geometries
{
    /// <summary>
    /// A structure for holding X and Y pair.
    /// </summary>
    [Serializable]
    public class Coordinate : IEquatable<Coordinate>
    {
        public Coordinate() : this(0.0,0.0) { }
        
        public Coordinate(Coordinate c) : this(c.X, c.Y) { }
        
        public Coordinate(double x, double y) 
        {
            this.X = x;
            this.Y = y;
        }
        
        public double X { get; set; }

        public double Y { get; set; }

        /// <summary>
        /// Returns distance from <c>p</c> coordinate.
        /// </summary>
        /// <param name="p"><c>Coordinate</c> with which to do the distance comparison.</param>
        /// <returns></returns>
        public double Distance(Coordinate p)
        {
            var dx = this.X - p.X;
            var dy = this.Y - p.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Returns <c>true</c> if <c>other</c> has the same values for the x and y ordinates.
        /// </summary>
        /// <param name="other"><c>Coordinate</c> with which to do the comparison.</param>
        /// <returns><c>true</c> if <c>other</c> is a <c>Coordinate</c> with the same values for the x and y ordinates.</returns>
        public bool Equals(Coordinate other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            Coordinate point = (Coordinate)obj;
            return this.Equals(point);
        }

        /// <summary>
        /// Overloaded GetHashCode method.
        /// </summary>
        /// <returns>The hash code for the current object.</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 23) + this.X.GetHashCode();
            hash = (hash * 23) + this.Y.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Returns a formated string the form of {X},{Y}.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{0},{1}".FormatWith(this.X.ToString(), this.Y.ToString());
        }

        /// <summary>
        /// Overloaded == operator.
        /// </summary>
        public static bool operator ==(Coordinate point1, Coordinate point2)
        {
            if ((object)point1 == null || ((object)point2 == null))
                return false;
            else return
                point1.Equals(point2);
        }

        /// <summary>
        /// Overloaded != operator.
        /// </summary>
        public static bool operator !=(Coordinate point1, Coordinate point2)
        {
            return !(point1 == point2);
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(Coordinate coord1, Coordinate coord2)
        {
            return new Coordinate(coord1.X + coord2.X, coord1.Y + coord2.Y);
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X + d, coord1.Y + d);
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(double d, Coordinate coord1)
        {
            return coord1 + d;
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(Coordinate coord1, Coordinate coord2)
        {
            return new Coordinate(coord1.X * coord2.X, coord1.Y * coord2.Y);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X * d, coord1.Y * d);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(double d, Coordinate coord1)
        {
            return coord1 * d;
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(Coordinate coord1, Coordinate coord2)
        {
            return new Coordinate(coord1.X - coord2.X, coord1.Y - coord2.Y);
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X - d, coord1.Y - d);
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(double d, Coordinate coord1)
        {
            return coord1 - d;
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(Coordinate coord1, Coordinate coord2)
        {
            return new Coordinate(coord1.X / coord2.X, coord1.Y / coord2.Y);
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X / d, coord1.Y / d);
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(double d, Coordinate coord1)
        {
            return coord1 / d;
        }
    }
}
