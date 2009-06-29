//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Xunit;
//using Wrapper.ObjectOperations;
//using Wrapper;
//using Moq;
//using Wrapper.MapbasicOperations;
//
//namespace MapinfoWrapperTest.WrapperTest.ObjectOperations
//{
//    public class GeometryTests
//    {
//        [Fact]
//        public void RunObjectInfoReturnsString()
//        {
//            IMapinfoWrapper wrapper = new Mock<IMapinfoWrapper>().Object;
//            IMapbasicVariable variable = new Mock<IMapbasicVariable>().Object;
//            ObjectInfoEnum attribute = ObjectInfoEnum.OBJ_INFO_SYMBOL;
//            Assert.IsType(typeof(string),MapbasicObject.ObjectInfo(wrapper,variable.Name,attribute));
//        }
//
//        [Fact]
//        public void ObjectTypeReturnsObjectTypeEnum()
//        {
//            IMapinfoWrapper wrapper = new Mock<IMapinfoWrapper>().Object;
//            IMapbasicVariable variable = new Mock<IMapbasicVariable>().Object;
//            MapbasicObject @object = new MapbasicObject(wrapper, variable.Name);
//            Assert.IsType(typeof(ObjectTypeEnum), @object.ObjectType);
//        }
//    }
//}
