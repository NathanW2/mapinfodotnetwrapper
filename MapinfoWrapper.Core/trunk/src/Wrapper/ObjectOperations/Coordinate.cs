using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wrapper.ObjectOperations
{
    public struct Coordinate
    {
        public Coordinate(decimal x, decimal y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public decimal X { get; private set; }
        public decimal Y { get; private set; }

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
