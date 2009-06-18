using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MapinfoWrapper.TableOperations;
using MapinfoWrapper.Core.IoC;
using Moq;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.TableOperations.RowOperations.Entities;
using MapinfoWrapper.Core.Internals;

namespace MapinfoWrapperTest.WrapperTest.Table_Operations
{
    [TestFixture]
    public class TableFactoryTests
    {
        private Mock<IMapinfoWrapper> mockmapinfo;
        private Mock<ITableCommandRunner> mockcommandrunner;

        [SetUp]
        public void SetUp()
        {
            mockmapinfo = new Mock<IMapinfoWrapper>();
            mockcommandrunner = new Mock<ITableCommandRunner>();

            DependencyResolver resolver = new DependencyResolver();
            resolver.Register(typeof(IMapinfoWrapper), mockmapinfo.Object);
            resolver.Register(typeof(ITableCommandRunner), mockcommandrunner.Object);
            IoC.Initialize(resolver);            
        }

        [Test]
        public void OpenTableReturnsNewTable()
        {
            TableFactory factory = new TableFactory();
            object obj = factory.OpenTable("");
            Assert.IsInstanceOf<ITable>(obj);
        }

        [Test]
        public void OpenTableGenericReturnsGenericTable()
        {
            TableFactory factory = new TableFactory();
            object obj = factory.OpenTable<DummyDef>("");
            Assert.IsInstanceOf<ITable<DummyDef>>(obj);
        }

        [Test]
        public void OpenTableCallsOpenTableOnCommandRunner()
        {
            TableFactory factory = new TableFactory();
            object table1 = factory.OpenTable("DummyPath");
            object table2 = factory.OpenTable<DummyDef>("DummyPath2");

            mockcommandrunner.Verify(cmd => cmd.OpenTable("DummyPath"));
            mockcommandrunner.Verify(cmd => cmd.OpenTable("DummyPath2"));
        }

        [Test]
        public void OpenTableResolvesNameUsingTableCommandRunnerWithZeroForLastTable()
        {
            TableFactory factory = new TableFactory();
            object table = factory.OpenTable("DummyPath");
            mockcommandrunner.Verify(cmd => cmd.GetName(0));
        }

        [Test]
        public void GenericOpenTableResolvesNameUsingTableCommandRunnerWithZeroForLastTable()
        {
            TableFactory factory = new TableFactory();
            object table = factory.OpenTable<DummyDef>("DummyPath");
            mockcommandrunner.Verify(cmd => cmd.GetName(0));
        }
    }

    public class DummyDef : BaseEntity
    { }
}
