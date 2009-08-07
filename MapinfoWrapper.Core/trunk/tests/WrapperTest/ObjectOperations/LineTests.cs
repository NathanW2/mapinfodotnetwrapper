using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.Wrapper.Geometries;

namespace MapinfoWrapperTest.WrapperTest.ObjectOperations
{
    [TestFixture]
    public class LineTests
    {
        [Test]
        public void LineTest()
        {
            Coordinate start = new Coordinate(1.2, 1.3);
            Coordinate end = new Coordinate(2.4, 3.2);
            Line line = new Line(start, end);
        }
    }
}
