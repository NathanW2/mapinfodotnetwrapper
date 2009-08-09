﻿using System;
using MapinfoWrapper.DataAccess.LINQ.SQLBuilders;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using Moq;
using NUnit.Framework;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.Wrapper.Geometries;

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
        public void GenerateInsertStringCanHandleGeometryObjects()
        {
            SqlStringGenerator gen = new SqlStringGenerator();
            MappableEntity entity = new MappableEntity();
            entity.obj = new Line();

            string updatestring = gen.GenerateInsertString(entity, "DummyTable");
            string expected = "INSERT INTO DummyTable (obj) VALUES (CreateLine(0,0,0,0))";
            
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