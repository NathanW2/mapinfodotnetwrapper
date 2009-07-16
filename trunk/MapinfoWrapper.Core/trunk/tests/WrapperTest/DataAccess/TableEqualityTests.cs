using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapperTest.WrapperTest.DataAccess
{
    using MapinfoWrapper.DataAccess;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.Mapinfo;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TableEqualityTests
    {
        private const string TableName = "Test";
        private Mock<IMapinfoWrapper> mockmapinfo = new Mock<IMapinfoWrapper>();

        private MapinfoSession fakesession;
        private Table table;

        [SetUp]
        public void Given_a_new_table()
        {
            fakesession = new MapinfoSession(mockmapinfo.Object);
            table = new Table(fakesession, TableName);
        }

        [Test]
        public void It_Should_be_equal_to_a_table_with_the_same_name_and_session()
        {
            Table table2 = new Table(fakesession, TableName);
            Assert.AreEqual(table,table2);
        }

        [Test]
        public void It_Should_not_equal_a_table_with_a_different_name_or_session()
        {
            Table table2 = new Table(fakesession, "Test2");
            Assert.AreNotEqual(table, table2);
        }

        [Test]
        public void If_tables_are_equal_they_should_have_the_same_hash()
        {
            Table table2 = new Table(fakesession, TableName);
            Assert.AreEqual(table,table2,"Tables are not equal");
            Assert.AreEqual(table.GetHashCode(),table2.GetHashCode());
        }

        [Test]
        public void It_should_be_equal_to_generic_table_with_same_name()
        {
            Table<BaseEntity> table2 = new Table<BaseEntity>(fakesession, TableName);
            Assert.AreEqual(table,table2);
        }

    }
}
