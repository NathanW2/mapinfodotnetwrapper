//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Moq;
//using Wrapper;
//using Wrapper.Extensions;
//using Wrapper.LayerOperations;
//
//namespace MapinfoWrapperTest.WrapperTest.LayerOperations
//{
//    [TestFixture]
//    public class LayerTests
//    {
//        private Moq.Mock<IMapinfoWrapper> mockmapinfo;
//
//        [SetUp]
//        public void CreateMockMapinfo()
//        {
//            mockmapinfo = new Moq.Mock<IMapinfoWrapper>();
//        }
//
//        [Test]
//        public void GetLayerFromNumberReturnsNewLayer()
//        {
//            Assert.AreEqual(typeof(Layer), Layer.GetLayerFromNumber(mockmapinfo.Object,1).GetType());
//        }
//
//        [Test]
//        public void GetLayerFromNameReturnsNewLayer()
//        {
//            Assert.AreEqual(typeof(Layer), Layer.GetLayerFromName(mockmapinfo.Object,"Test").GetType());
//        }
//
//
//    }
//}
