//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Moq;
//using Wrapper;
//using Wrapper.TableOperations;
//using Wrapper.MapOperations;
//using Wrapper.Extensions;
//using MapinfoWrapperTest.WrapperTest.Table_Operations;
//
//namespace MapinfoWrapperTest.WrapperTest.MapOperations
//{
//    [TestFixture]
//    [Ignore]
//    public class MapTests
//    {
//        private Moq.Mock<IMapinfoWrapper> mockmapinfo;
//        private Moq.Mock<ITable<DummyTableDef>> mocktable;
//
//        [SetUp]
//        public void CreateMockMapinfo()
//        {
//            mockmapinfo = new Moq.Mock<IMapinfoWrapper>();
//            mocktable = new Mock<ITable<DummyTableDef>>();
//        }
//
//        [Test]
//        public void MapTableShouldReturnMap()
//        {
//            mocktable.Setup(t => t.IsMappable).Returns(true);
//
//            ITable<DummyTableDef> table = mocktable.Object;
//
//            Assert.AreEqual(typeof(Map), Map.MapFromTable(mockmapinfo.Object,table).GetType());
//        }
//
//        [Test]
//        public void MapTableShouldCallMapFromTestCommand()
//        {
//            mockmapinfo.Setup(m => m.RunCommand("Map From Test"))
//                       .AtMostOnce();
//
//            mocktable.Setup(t => t.Name).Returns("Test");
//            mocktable.Setup(t => t.IsMappable).Returns(true);
//
//            ITable<DummyTableDef> table = mocktable.Object;
//            Map map = Map.MapFromTable(mockmapinfo.Object,table);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void MapTableShouldCallFrontWindowToGetTheLatestWindow()
//        {
//            mockmapinfo.Setup(m => m.Evaluate("FrontWindow()"))
//                       .Returns("12345")
//                       .AtMostOnce();
//
//            mocktable.Setup(t => t.IsMappable).Returns(true);
//
//            ITable<DummyTableDef> table = mocktable.Object;
//            Map map = Map.MapFromTable(mockmapinfo.Object, table);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void MapTableShouldCallCallJoinedMapForEachTablePassedIn()
//        {
//            mockmapinfo.Setup(m => m.RunCommand("Map From Test1,Test2,Test3,Test4,Test5,Test6"))
//                       .AtMostOnce()
//                       .Verifiable();
//
//            int tablenumber = 1;
//            mocktable.Setup(n => n.Name)
//                     .Returns(() => "Test{0}".FormatWith(tablenumber))
//                     .Callback(() => tablenumber++);
//
//            List<Table> tablelist = new List<Table>();
//           
//            for (int i = 0; i < 6; i++)
//            {
//                ITable<DummyTableDef> table = mocktable.Object;
//                tablelist.Add(table);
//            }
//
//
//            Map map = Map.MapFromTables(mockmapinfo.Object, tablelist);
//
//            mockmapinfo.Verify();
//        }
//
//        [Test]
//        public void GetFrontWindowIDReturnsVaildInt()
//        {
//            mockmapinfo.Setup(m => m.Evaluate("FrontWindow()"))
//                       .Returns("12345");
//
//            Assert.AreEqual(12345, Map.GetFrontWindowID(mockmapinfo.Object));  
//        }
//
//        [Test]
//        public void GetFrontWindowIDCallsFrontWindowCommand()
//        {
//            mockmapinfo.Setup(m => m.Evaluate("FrontWindow()"))
//                       .Returns("12345")
//                       .AtMostOnce();
//
//            int value = Map.GetFrontWindowID(mockmapinfo.Object);
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        [ExpectedException(typeof(ArgumentException),ExpectedMessage = "Table must be mappable")]
//        public void TableIsNotMappableMapTable()
//        {
//            mocktable.Setup(t => t.IsMappable).Returns(false);
//
//            ITable<DummyTableDef> table = mocktable.Object;
//            Map map = Map.MapFromTable(mockmapinfo.Object, table);
//        }
//    }
//}
