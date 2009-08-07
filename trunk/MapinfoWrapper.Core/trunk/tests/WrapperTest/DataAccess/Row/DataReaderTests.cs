using MapinfoWrapper.DataAccess.RowOperations;
using NUnit.Framework;
using Moq;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapperTest.WrapperTest.Table_Operations.Row
{
    public class DataReaderTest
    {
        //private Mock<IMapinfoWrapper> mockwrapper;

        //[SetUp]
        //public void SetUp()
        //{
        //    mockwrapper = new Mock<IMapinfoWrapper>();
        //}

        //[Test]
        //public void FetchCallsMapinfoFetchAtIndex()
        //{
        //    DataReader reader = new DataReader("DummyTable");
        //    reader.Fetch(1);

        //    mockwrapper.Verify(w => w.RunCommand("Fetch Rec {0} From {1}".FormatWith(1, "DummyTable")),
        //                       Times.AtMostOnce(),
        //                       "Fetch Rec was not called");
        //}

        //[Test]
        //public void FetchNextCallsMapinfoFetchNext()
        //{
        //    DataReader reader = new DataReader("DummyTable");
        //    reader.FetchNext();

        //    mockwrapper.Verify(w => w.RunCommand("Fetch Next From {0}".FormatWith("DummyTable")),
        //                       Times.AtMostOnce(),
        //                       "Fetch Next was not called");
        //}

        //[Test]
        //public void FetchFirstCallsMapinfoFetchFirst()
        //{
        //    DataReader reader = new DataReader("DummyTable");
        //    reader.FetchFirst();

        //    mockwrapper.Verify(w => w.RunCommand("Fetch First From {0}".FormatWith("DummyTable")),
        //                       Times.AtMostOnce(),
        //                       "Fetch First was not called");
        //}

        //[Test]
        //public void FetchLastCallsMapinfoFetchLast()
        //{
        //    DataReader reader = new DataReader("DummyTable");
        //    reader.FetchLast();

        //    mockwrapper.Verify(w => w.RunCommand("Fetch Last From {0}".FormatWith("DummyTable")),
        //                       Times.AtMostOnce(),
        //                       "Fetch Last was not called");
        //}

        //[Test]
        //public void GetTableAndRowStringResolvesTableAndColumnName()
        //{
        //    DataReader reader = new DataReader("DummyTable");
        //    string result = reader.GetTableAndRowString("RowId");

        //    Assert.AreEqual("DummyTable.RowId", result);
        //}

        //[Test]
        //public void EndTableReturnsFalse()
        //{
        //    mockwrapper.Setup(w => w.Evaluate("EOT(DummyTable)"))
        //               .Returns("F")
        //               .Verifiable();

        //    DataReader reader = new DataReader("DummyTable");

        //    Assert.False(reader.EndOfTable());

        //    mockwrapper.Verify();        
        //}

        //[Test]
        //public void EndTableReturnsTrue()
        //{
        //    mockwrapper.Setup(w => w.Evaluate("EOT(DummyTable)"))
        //               .Returns("T")
        //               .Verifiable();

        //    DataReader reader = new DataReader("DummyTable");

        //    Assert.True(reader.EndOfTable());

        //    mockwrapper.Verify();
        //}

        //[Test]
        //public void GetColumnCountReturnsColumnCountAndCallsTableinfo()
        //{
        //    mockwrapper.Setup(w => w.Evaluate("TableInfo(DummyTable,4)"))
        //               .Returns("10")
        //               .Verifiable();

        //    DataReader reader = new DataReader("DummyTable");

        //    Assert.AreEqual(reader.GetColumnCount(), 10);

        //    mockwrapper.Verify();
        //}
    }
}
