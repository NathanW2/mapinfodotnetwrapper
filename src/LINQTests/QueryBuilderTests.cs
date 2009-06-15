using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.TableOperations.LINQ;
using NUnit.Framework;
using Moq;
using Moq.Stub;
using Wrapper.TableOperations;
using Wrapper.Extensions;
using System.Linq.Expressions;
using Wrapper;
using Wrapper.ObjectOperations;
using Wrapper.TableOperations.RowOperations;

namespace LINQTests
{
    [TestFixture]
    public class QueryBuilderTests
    {
        Mock<DummyRow> mockrow;
        Mock<ITable<DummyRow>> mocktable;
        Mock<IMapinfoWrapper> mockwrapper;

        [SetUp]
        public void SetUpQueryBuilderTests()
        {
            mockrow = new Mock<DummyRow>();
            mocktable = new Mock<ITable<DummyRow>>();
            mockwrapper = new Mock<IMapinfoWrapper>();

            mocktable.Setup(table => table.Name)
                     .Returns("MockTable");

            mocktable.Setup(table => table.Expression)
                    .Returns(Expression.Constant(mocktable.Object));

            mocktable.Setup(table => table.Provider)
                     .Returns(new MapinfoQueryProvider(mockwrapper.Object, "MockTable"));
        }

        [Test]
        public void WhereWithSingleName()
        {
            ITable<DummyRow> table = mocktable.Object;

            IQueryable<DummyRow> linq = table.Where(row => row.Name == "Hello World");

            string result = linq.ToQueryString();

            string expected = @"SELECT * FROM MockTable WHERE Name = {0} INTO WrapperTempTable".FormatWith("Hello World".InQuotes());

            Assert.AreEqual(expected,result);
        }

        [Test]
        public void WhereWithBinaryAndWithName()
        {
            ITable<DummyRow> table = mocktable.Object;

            IQueryable<DummyRow> linq = table.Where(row => row.Name == "Hello World"
                                                        & row.Name == "Test World");

            string result = linq.ToQueryString();

            string expected = @"SELECT * FROM MockTable WHERE Name = {0} AND Name = {1} INTO WrapperTempTable".FormatWith("Hello World".InQuotes(),
                                                                                                                          "Test World".InQuotes());
            Assert.AreEqual(expected,result);
        }

        [Test]
        public void DatesinWhereClause()
        {
            ITable<DummyRow> table = mocktable.Object;

            IQueryable<DummyRow> linq = table.Where(row => row.AssetDate == new DateTime(2008,01,01));

            string result = linq.ToQueryString();

            string expecteddate = "1/01/2008 12:00:00 AM".InQuotes();
            string expected = @"SELECT * FROM MockTable WHERE AssetDate = {0} INTO WrapperTempTable".FormatWith(expecteddate);

            Assert.AreEqual(expected, result);
        
        }

        [Test]
        public void LocalVariableWithDatesinWhereClause()
        {
            ITable<DummyRow> table = mocktable.Object;

            DateTime date = new DateTime(2008, 01, 01);
            IQueryable<DummyRow> linq = table.Where(row => row.AssetDate == date);

            string result = linq.ToQueryString();

            string expecteddate = "1/01/2008 12:00:00 AM".InQuotes();
            string expected = @"SELECT * FROM MockTable WHERE AssetDate = {0} INTO WrapperTempTable".FormatWith(expecteddate);

            Assert.AreEqual(expected, result);

        }

        [Test]
        public void ComparingCentroidsLINQTest()
        {
            ITable<DummyRow> table = mocktable.Object;

            Coordinate point = new Coordinate(507288.24M, 10002958.9M);
            IQueryable<DummyRow> linq = table.Where(row => row.obj.Centroid == point);

            string result = linq.ToQueryString();

            string select = @"SELECT * FROM MockTable "; 
            string where = @"WHERE CENTROIDX(obj) = CENTROIDX(CREATEPOINT(507288.24,10002958.9)) AND ";
            string where2 = @"CENTROIDY(obj) = CENTROIDY(CREATEPOINT(507288.24,10002958.9)) ";
            string into = @"INTO WrapperTempTable";

            string expected = select + where + where2 + into;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SelectTest()
        {
            ITable<DummyRow> table = mocktable.Object;

            IQueryable<string> linq = table.Select(row => row.Name);

            string result = linq.ToQueryString();

            string expected = @"SELECT Name FROM MockTable INTO WrapperTempTable";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SelectShouldAlsoWorkWithWhere()
        {
            ITable<DummyRow> table = mocktable.Object;

            IQueryable<string> linq = table.Where(row => row.Name == "Hello World")
                                           .Select(row => row.Name);

            string result = linq.ToQueryString();

            string expected = @"SELECT Name FROM MockTable WHERE Name = {0} INTO WrapperTempTable".FormatWith("Hello World".InQuotes());

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SelectShouldAlsoWorkWithWhere2()
        {
            ITable<DummyRow> table = mocktable.Object;

            IQueryable<string> linq = table.Where(row => row.Name == "Hello World")
                                           .Select(row => row.Name);

            string result = linq.ToQueryString();

            string expected = @"SELECT Name FROM MockTable WHERE Name = {0} INTO WrapperTempTable".FormatWith("Hello World".InQuotes());
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void IntoTestWithSelectAndWhere()
        {
            ITable<DummyRow> table = mocktable.Object;

            IQueryable<string> linq = table.Where(row => row.Name == "Hello World")
                                           .Select(row => row.Name)
                                           .Into("TempTable");
                                           
            string result = linq.ToQueryString();

            string expected = @"SELECT Name FROM MockTable WHERE Name = {0} INTO TempTable".FormatWith("Hello World".InQuotes());
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void IntoTestWithWhere()
        {
            ITable<DummyRow> table = mocktable.Object;

            IQueryable<DummyRow> linq = table.Where(row => row.Name == "Hello World")
                                             .Into("TempTable");

            string result = linq.ToQueryString();

            string expected = @"SELECT * FROM MockTable WHERE Name = {0} INTO TempTable".FormatWith("Hello World".InQuotes());
            Assert.AreEqual(expected, result);
        }
    }

    public class DummyRow : IMappableRow
    {
        public DummyRow()
        {

        }
        public int AssetID { get; set; }
        public string Name { get; set; }
        public DateTime AssetDate { get; set; }

        #region IMappableRow Members

        public Geometry obj
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IRow Members

        public IMapinfoWrapper Wrapper
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string TableName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int? SelectedRowId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IRecordSelector RowRecordSelector
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInserted
        {
            get { throw new NotImplementedException(); }
        }

        public string ToInsertString()
        {
            throw new NotImplementedException();
        }

        public string ToUpdateString()
        {
            throw new NotImplementedException();
        }

        public object Get(string columnName)
        {
            throw new NotImplementedException();
        }

        public void Set(string columnName, object value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public static class LINQExtensionsHelper
    {
        public static string ToQueryString<T>(this IQueryable<T> source)
        {
            if (source.Provider is MapinfoQueryProvider)
            {
                MapinfoQueryProvider provider = source.Provider as MapinfoQueryProvider;
                return provider.GetQueryString(source.Expression);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Source provider is not a Mapinfo Query Provider");
            }
        }
    
    }

}
