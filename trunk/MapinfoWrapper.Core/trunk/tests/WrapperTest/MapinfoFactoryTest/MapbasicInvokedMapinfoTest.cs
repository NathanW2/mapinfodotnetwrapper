using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Moq;
//using NUnit.Framework;
//using Wrapper;
//using Wrapper.MapinfoSessionManager;
//using Wrapper.Exceptions;
//
//namespace MapinfoWrapperTest.WrapperTest.MapinfoFactoryTest
//{
//     [TestFixture]
//    public class MapbasicInvokedMapinfoTest
//    {
//        private Mock<IFakeMapinfo> mockmapinfo;
//
//     [SetUp]
//       public void Setup()
//        {
//            this.mockmapinfo = new Mock<IFakeMapinfo>();
//        }
//
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void NullStringDoThrowsException()
//        {
//            MapbasicInvokedMapinfo mapinfo = new MapbasicInvokedMapinfo(null);
//            mapinfo.RunCommand(null);
//        }
//
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void NullStringEvalThrowsException()
//        {
//            MapbasicInvokedMapinfo mapinfo = new MapbasicInvokedMapinfo(null);
//            mapinfo.Evaluate(null);
//        }
//    }
//}
