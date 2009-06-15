using System;
using Moq;
using NUnit.Framework;
using MapinfoWrapper.Geometries;
using MapinfoWrapper.TableOperations.RowOperations;

namespace MapinfoWrapperTest.WrapperTest.ObjectOperations
{
    [TestFixture]
    public class ObjVariableExtendorTests
    {
        private Mock<IDataReader> mockreader = new Mock<IDataReader>();

        [Test]
        public void ExpressionCallsFetch()
        {
            mockreader.Setup(reader => reader.Fetch(It.IsAny<int>()))
                      .Verifiable();

            ObjVariableExtender variableextendor = new ObjVariableExtender(mockreader.Object, 0);

            object expression = variableextendor.ObjectExpression;

            mockreader.Verify();
        }

        [Test]
        public void ExpressionCallsFetchWithRightIndex()
        {
            mockreader.Setup(reader => reader.Fetch(10))
                      .Verifiable();

            ObjVariableExtender extendor = new ObjVariableExtender(mockreader.Object, 10);

            object expression = extendor.ObjectExpression;

            mockreader.Verify();
        }

        [Test]
        public void ExpressionReturnsObjectExpressionStringForTable()
        {
            mockreader.Setup(reader => reader.GetTableAndRowString("obj"))
                      .Returns("DummyTable.obj");

            ObjVariableExtender extendor = new ObjVariableExtender(mockreader.Object, 0);

            object expression = extendor.ObjectExpression;

            Assert.IsInstanceOf<String>(expression);
            Assert.AreEqual("DummyTable.obj", expression);
        }
    }
}
