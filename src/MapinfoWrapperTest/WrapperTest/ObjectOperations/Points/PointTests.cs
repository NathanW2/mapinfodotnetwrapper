//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Xunit;
//using Wrapper.MapbasicOperations;
//using Wrapper;
//using Moq;
//using Wrapper.ObjectOperations.Points;
//using Specifications;
//using Wrapper.ObjectOperations;
//
//namespace MapinfoWrapperTest.WrapperTest.ObjectOperations.Points
//{
//    public class PointTests
//    {
//        [Fact]
//        public void CreatePointShouldReturnNewPointObject()
//        {
//            IMapinfoWrapper wrapper = new Mock<IMapinfoWrapper>().Object;
//            IMapbasicVariable variable = new Mock<IMapbasicVariable>().Object;
//            Coordinate point = new Coordinate(0.0000, 0.000);
//            object pointobject = Point.CreatePoint(wrapper,point,variable);
//            Assert.IsType(typeof(Point), pointobject);
//        }
//    }
//}
