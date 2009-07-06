//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Moq;
//using Wrapper;
//using Wrapper.TableOperations;
//using Wrapper.Extensions;
//using Wrapper.TableOperations.RowOperations;
//
//namespace MapinfoWrapperTest.WrapperTest.Table_Operations
//{
//    [TestFixture]
//    public class TableTests
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
//        public void RowsShouldReturnIEumerableOfGenericMapinfoRow()
//        {
//            Table<DummyTableDef> table = new Table<DummyTableDef>(mockmapinfo.Object, "Water_Mains");
//            Assert.AreEqual(typeof(RowList<DummyTableDef>), table.Rows.GetType());
//        }
//    }
//
//    public class DummyTableDef : Row<DummyTableDef>
//    {
//        public int AssetID { get; set; }
//    }
//}
