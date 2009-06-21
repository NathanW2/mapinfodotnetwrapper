using System;
using Moq;
using NUnit.Framework;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.TableOperations.LINQ.SQLBuilders;
using MapinfoWrapper.TableOperations.RowOperations.Entities;

namespace MapinfoWrapperTest.WrapperTest.Table_Operations
{
    [TestFixture]
    public class SqlStringGeneratorTests
    {
        [Test]
        public void GenerateInsertStringReturnsVaildString()
        {
            SqlStringGenerator gen = new SqlStringGenerator();
            DummyEntity entity = new DummyEntity();
            entity.ID = 100;
            entity.Name = "TestValue";
            entity.Time = new DateTime(2009,12,01);
            string updatestring = gen.GenerateInsertString(entity, "DummyTable");
            string expected = "INSERT INTO DummyTable (ID,Name,Time) VALUES (100,{0},{1})".FormatWith("TestValue".InQuotes(),
                                                                                                      "1/12/2009 12:00:00 AM".InQuotes());
            Assert.AreEqual(expected, updatestring);
        }

        [Test]
        public void GenerateInsertStringCanHandleNullDatesAndStrings()
        {
            SqlStringGenerator gen = new SqlStringGenerator();
            DummyEntity entity = new DummyEntity();
            entity.ID = 100;
            entity.Name = null;
            entity.Time = null;
            string updatestring = gen.GenerateInsertString(entity, "DummyTable");
            string expected = "INSERT INTO DummyTable (ID,Name,Time) VALUES (100,{0},{0})".FormatWith("".InQuotes());
            Assert.AreEqual(expected, updatestring);
        }

        [Test]
        public void GenerateInsertStringCanHandleGeometryObjects(
            [Values("DummyVar","DummyTable.Obj")] string variableName)
        {
            Mock<IGeometry> mockobj = new Mock<IGeometry>();
            mockobj.Setup(obj => obj.Variable.GetExpression())
                   .Returns(variableName);

            SqlStringGenerator gen = new SqlStringGenerator();
            MappableEntity entity = new MappableEntity();
            entity.obj = mockobj.Object;

            string updatestring = gen.GenerateInsertString(entity, "DummyTable");
            string expected = "INSERT INTO DummyTable (obj) VALUES ({0})".FormatWith(variableName);
            
            Assert.AreEqual(expected, updatestring);
        }

        public class DummyEntity : BaseEntity
        {
            public int ID
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public DateTime? Time
            {
                get;
                set;
            }
        }
    }
}