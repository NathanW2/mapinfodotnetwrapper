using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using MapinfoWrapper;
using MapinfoWrapper.TableOperations;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.TableOperations.RowOperations;
using MapinfoWrapper.CommandBuilders;
using MapinfoWrapper.Core.IoC;
using Moq.Stub;
using MapinfoWrapper.TableOperations.RowOperations.Entities;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapperTest.WrapperTest.Table_Operations
{
    [TestFixture]
    public class GenericTableTests
    {
        private Mock<IMapinfoWrapper> mockmapinfo;

        [SetUp]
        public void SetUp()
        {
            mockmapinfo = new Mock<IMapinfoWrapper>();
            DependencyResolver resolver = new DependencyResolver();
            resolver.Register(typeof(IMapinfoWrapper), mockmapinfo.Object);
            IoC.Initialize(resolver);
        }

        [Test]
        public void CloseShouldCallCloseTable()
        {
            mockmapinfo.Setup(m => m.RunCommand("Close Table DummyTable"))
                       .Verifiable("Close table was not called");

            ITable table = new TestTable();

            table.Close();

            mockmapinfo.Verify();
        }

        [Test]
        public void StaticCloseShouldCallCloseOnTable()
        {
            Mock<ITable> mocktable = new Mock<ITable>();

            Table.CloseTable(mocktable.Object);

            mocktable.Verify(tab => tab.Close());
        }

        [Test]
        public void StaticCloseTablesShouldCallCloseForEachTable()
        {
            Mock<ITable> mocktable = new Mock<ITable>();

            List<ITable> tablelist = new List<ITable>();

            tablelist.Add(mocktable.Object);
            tablelist.Add(mocktable.Object);
            tablelist.Add(mocktable.Object);

            Table.CloseTables(tablelist);

            mocktable.Verify(tab => tab.Close(), Times.Exactly(3), "Close was not called 3 times");
        }

        public class TestTable : Table<BaseEntity>
        {
            public TestTable()
                : base(IoC.Resolve<IMapinfoWrapper>(), "DummyTable")
            { }

            public override string Name
            {
                get
                {
                    return "DummyTable";
                }
            }
        }

    }
}
//
//        [Test]
//        public void GetTableNameShouldReturnWater_Mains()
//        {
//            string ExpectedCommand = "TableInfo(1,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_NAME);
//            mockmapinfo.Setup(f => f.Evaluate(ExpectedCommand))
//                       .Returns("Water_Mains");
//
//            Table table = new Table (mockmapinfo.Object, 1);
//            Assert.AreEqual("Water_Mains", table.Name);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void GetTableNumberShouldReturnOne()
//        {
//            string ExpectedCommand = "TableInfo(1,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_NUM);
//            mockmapinfo.Setup(f => f.Evaluate(ExpectedCommand))
//                       .Returns("1");
//
//            Table tabinfo = new Table(mockmapinfo.Object, 1);
//
//            Assert.AreEqual(1, tabinfo.Number);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void RunTableInfoCommandPassingTableNumberShouldReturnExpectedName()
//        {
//            string ExpectedCommand = "TableInfo(1,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_NAME);
//
//            mockmapinfo.Setup(m => m.Evaluate(ExpectedCommand))
//                       .Returns("Water_Mains");
//
//            string name = Table.TableInfo(mockmapinfo.Object,
//                                                    1,
//                                                    TableInfoEnum.TAB_INFO_NAME);
//
//            Assert.AreEqual("Water_Mains", name);
//        }
//
//        [Test]
//        public void RunTableInfoCommandPassingTableNameShouldReturnExpectedName()
//        {
//            string ExpectedCommand = "TableInfo(Water_Mains,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_NAME);
//            mockmapinfo.Setup(m => m.Evaluate(ExpectedCommand))
//                       .Returns("Water_Mains");
//
//            string name = Table.TableInfo(mockmapinfo.Object,
//                                                    "Water_Mains",
//                                                    TableInfoEnum.TAB_INFO_NAME);
//
//            Assert.AreEqual("Water_Mains", name);
//        }
//
//        [Test]
//        public void InstanceRunTableInfoCommandReturnsTableNameUsingName()
//        {
//            string ExpectedCommand = "TableInfo(Water_Mains,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_NAME);
//            
//            mockmapinfo.Setup(m => m.Evaluate(ExpectedCommand))
//                       .Returns("Water_Mains");
//
//            Table<DummyTableDef> table = new Table<DummyTableDef>(mockmapinfo.Object, "Water_Mains");
//            string name = table.RunTableInfoCommand(TableInfoEnum.TAB_INFO_NAME);
//
//            Assert.AreEqual("Water_Mains",name);
//        
//        }
//
//        [Test]
//        public void InstanceRunTableInfoCommandReturnsTableNameUsingNumber()
//        {
//            string expectedCommand = "TableInfo(1,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_NAME);
//
//            mockmapinfo.Setup(m => m.Evaluate(expectedCommand))
//                       .Returns("Water_Mains");
//
//            Table<DummyTableDef> table = new Table<DummyTableDef>(mockmapinfo.Object,
//                                                            1);
//            string name = table.RunTableInfoCommand(TableInfoEnum.TAB_INFO_NAME);
//
//            Assert.AreEqual("Water_Mains", name);
//        
//        }
//
//        [Test]
//        public void GetTablePathFromNumber()
//        {
//            string ExpectedCommand = "TableInfo(1,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_TABFILE);
//            mockmapinfo.Setup(m => m.Evaluate(ExpectedCommand))
//                       .Returns(@"C:\Temp\Water_Mains");
//
//
//            Table tabinfo = new Table(mockmapinfo.Object, 1);
//            Assert.AreEqual(@"C:\Temp\Water_Mains",tabinfo.TablePath.FullName);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void GetTablePathFromNumberNullReturn()
//        {
//            string ExpectedCommand = "TableInfo(1,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_TABFILE);
//            mockmapinfo.Setup(m => m.Evaluate(ExpectedCommand))
//                       .Returns("");
//
//            Table tabinfo = new Table(mockmapinfo.Object, 1);
//            Assert.AreEqual(null, tabinfo.TablePath);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void GetNumberOfColumnsShouldReturnFive()
//        {
//            string expectedcommand = "TableInfo(1,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_NCOLS);
//            mockmapinfo.Setup(m => m.Evaluate(expectedcommand))
//                       .Returns("5");
//
//            Table tabinfo = new Table(mockmapinfo.Object, 1);
//            Assert.AreEqual(5, tabinfo.GetNumberOfColumns());
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void RowsPropertyReturnsRowCollection()
//        {
//            Table tabinfo = new Table(mockmapinfo.Object, "Water_Mains");
//            Assert.AreEqual(typeof(RowList<DummyTableDef>), tabinfo.Rows.GetType());
//        }
//
//        [Test]
//        public void OpenTableUsingCommandBuilderShouldCallOpenTableCommand()
//        {
//            mockmapinfo.Setup(m => m.RunCommand("Open Table {0}".FormatWith(@"C:\Temp\Test.tab".InQuotes())))
//                       .AtMostOnce();
//            
//            OpenTableCommandBuilder commandbuidler = new OpenTableCommandBuilder(@"C:\Temp\Test.tab");
//
//            Table.OpenTable(mockmapinfo.Object, commandbuidler);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void OpenTableUsingCommandBuilderShouldReturnTableInfoTest()
//        {
//            OpenTableCommandBuilder commandbuidler = new OpenTableCommandBuilder(@"C:\Temp\Test.tab");
//            Assert.AreEqual(typeof(Table), Table.OpenTable(mockmapinfo.Object, commandbuidler).GetType());
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void OpenTablePassingOnlyFileNameShouldCallOpenTableMapinfoCommand()
//        {
//            mockmapinfo.Setup(m => m.RunCommand("Open Table {0}".FormatWith(@"C:\Temp\Test.tab".InQuotes())))
//                        .AtMostOnce();
//
//            Table.OpenTable(mockmapinfo.Object, @"C:\Temp\Test.tab");
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void OpenTablePassingOnlyFileNameShouldReturnTable()
//        {
//            Assert.AreEqual(typeof(Table), Table.OpenTable(mockmapinfo.Object, @"C:\Temp\Test.tab").GetType());
//        }
//
//        [Test]
//        public void GenericOpenTableShouldReturnGenericTable()
//        {
//            Table<DummyTableDef> table = Table.OpenTable<DummyTableDef>(mockmapinfo.Object, @"C:\Temp\Water_Mains.Tab");
//            Assert.AreEqual(typeof(Table<DummyTableDef>), table.GetType());
//        }
//
//        [Test]
//        public void GenericOpenTableShouldCallMapinfoOpenTable()
//        {
//            mockmapinfo.Setup(m => m.RunCommand("Open Table {0}".FormatWith(@"C:\Temp\Test.tab".InQuotes())))
//                        .AtMostOnce();
//
//            Table<DummyTableDef> table = Table.OpenTable<DummyTableDef>(mockmapinfo.Object, @"C:\Temp\Test.tab");
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void MappableReturnsTrue()
//        {
//            mockmapinfo.Setup(m => m.Evaluate("TableInfo(0,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_MAPPABLE)))
//                       .Returns("T");
//
//            ITable<DummyTableDef> table = new Table<DummyTableDef>(mockmapinfo.Object, 0);
//            Assert.IsTrue(table.IsMappable);
//        }
//
//        [Test]
//        public void MappableReturnsFalse()
//        {
//            mockmapinfo.Setup(m => m.Evaluate("TableInfo(0,{0})".FormatWith((int)TableInfoEnum.TAB_INFO_MAPPABLE)))
//                           .Returns("F");
//
//            ITable<DummyTableDef> table = new Table<DummyTableDef>(mockmapinfo.Object, 0);
//            Assert.IsFalse(table.IsMappable);        
//        }
//    }
//}
