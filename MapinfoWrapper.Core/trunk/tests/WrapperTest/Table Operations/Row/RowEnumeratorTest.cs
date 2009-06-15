//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Moq;
//using Wrapper;
//using Wrapper.TableOperations.Row;
//using System.Linq.Expressions;
//using System.Collections;
//using Wrapper.TableOperations;
//using Wrapper.TableOperations.TableDefs;
//
//namespace MapinfoWrapperTest.WrapperTest.Table_Operations.Row
//{
//    [TestFixture]
//    public class RowEnumeratorTests
//    {
//        private Mock<IMapinfoWrapper> mockwrapper;
//        [SetUp]
//        public void CreateMapinfoMockWrapper()
//        {
//            mockwrapper = new Mock<IMapinfoWrapper>();
//        }
//
//        [Test]
//        public void FetchNextMustBeCalledOnMoveNext()
//        {
//            mockwrapper.Setup(m => m.RunCommand("Fetch Next From Water_Mains"))
//                       .AtMostOnce();
//
//            IMapinfoWrapper wrapper = mockwrapper.Object;
//            var rowenerator = new RowEnumerator<DummyTableDef>(wrapper, "Water_Mains");
//
//            rowenerator.MoveNext();
//
//            mockwrapper.VerifyAll();
//        }
//
//        [Test]
//        public void MoveNextMustCallEOTAndReturnTrueOnFalseEOTReturn()
//        {
//            mockwrapper.Setup(m => m.Evaluate("EOT(Water_Mains)"))
//                      .Returns("F")
//                      .AtMostOnce();
//
//
//            IMapinfoWrapper wrapper = mockwrapper.Object;
//            var rowenerator = new RowEnumerator<DummyTableDef>(wrapper, "Water_Mains");
//
//            Assert.IsTrue(rowenerator.MoveNext());
//            mockwrapper.VerifyAll();
//        }
//
//        [Test]
//        public void MoveNextMustCallEOTAndReturnFalseOnTrueEOTReturn()
//        {
//            mockwrapper.Setup(m => m.Evaluate("EOT(Water_Mains)"))
//                      .Returns("T")
//                      .AtMostOnce()
//                      .Verifiable();
//
//            IMapinfoWrapper wrapper = mockwrapper.Object;
//            IEnumerator<DummyTableDef> rowenerator = new RowEnumerator<DummyTableDef>(wrapper, "Water_Mains");
//
//            Assert.IsFalse(rowenerator.MoveNext());
//        }
//
//        [Test]
//        public void ResetMustCallFetchRec0()
//        {
//            mockwrapper.Setup(m => m.RunCommand("Fetch Rec 0 From Water_Mains"))
//                       .AtMostOnce()
//                       .Verifiable();
//
//            IMapinfoWrapper wrapper = mockwrapper.Object;
//            IEnumerator<DummyTableDef> rowenerator = new RowEnumerator<DummyTableDef>(wrapper, "Water_Mains");
//
//            rowenerator.Reset();
//        }
//
//        [Test]
//        public void DisposeMustCallFetchRec0()
//        {
//            mockwrapper.Setup(m => m.RunCommand("Fetch Rec 0 From Water_Mains"))
//                       .AtMostOnce()
//                       .Verifiable();
//
//            IMapinfoWrapper wrapper = mockwrapper.Object;
//            IEnumerator<DummyTableDef> rowenerator = new RowEnumerator<DummyTableDef>(wrapper, "Water_Mains");
//
//            rowenerator.Dispose();
//            
//        }
//
//        [Test]
//        public void CurrentMustCallMethodToGetRowId()
//        {
//            mockwrapper.Setup(m => m.RunCommand("Water_Mains.RowID"))
//                       .AtMostOnce()
//                       .Verifiable();
//
//            IMapinfoWrapper wrapper = mockwrapper.Object;
//            IEnumerator<DummyTableDef> rowenerator = new RowEnumerator<DummyTableDef>(wrapper, "Water_Mains");
//
//            DummyTableDef row = rowenerator.Current;
//        }
//
//        [Test]
//        public void CurrentMustReturnMapinfoRow()
//        {
//            IMapinfoWrapper wrapper = mockwrapper.Object;
//            IEnumerator<DummyTableDef> rowenerator = new RowEnumerator<DummyTableDef>(wrapper, "Water_Mains");
//
//            Assert.AreEqual(typeof(Row<IBaseTable>), rowenerator.Current.GetType());
//        }
//
//        [Test]
//        public void NonGenericCurrentMustReturnMapinfoRow()
//        {
//            IMapinfoWrapper wrapper = mockwrapper.Object;
//            IEnumerator<DummyTableDef> rowenerator = new RowEnumerator<DummyTableDef>(wrapper, "Water_Mains");
//
//            object row = ((IEnumerator)rowenerator).Current;
//            Assert.AreEqual(typeof(Row<IBaseTable>), row.GetType());
//        }
//   }
//}
