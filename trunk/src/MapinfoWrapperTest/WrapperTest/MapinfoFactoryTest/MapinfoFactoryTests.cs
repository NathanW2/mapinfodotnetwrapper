using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper;
using MapinfoWrapper.Core.IoC;
using MapinfoWrapper.Mapinfo.Internals;


namespace MapinfoWrapperTest.WrapperTest.MapinfoFactoryTest
{
    [TestFixture]
    public class MapinfoFactoryTests
    {
        [Test]
        public void CreateNewCOMInstanceShouldReturnCOMMapinfo()
        {
            TestFactory factory = new TestFactory();
            object result = factory.CreateCOMInstance();
            Assert.IsInstanceOf<IMapinfoWrapper>(result);
            Assert.IsInstanceOf<COMMapinfo>(result);
        }

        [Test]
        public void CreateNewCOMInstanceWireUpCorrectDependencies()
        {
            TestFactory factory = new TestFactory();
            object result = factory.CreateCOMInstance();
            object iocmapinfo = IoC.Resolve<IMapinfoWrapper>();
            Assert.AreEqual(result, iocmapinfo);
        }
    }

    public class TestFactory : MapinfoFactory
    {
        protected override DMapInfo CreateMapinfoInstance()
        {
            return new Mock<DMapInfo>().Object;
        }
    }
}
