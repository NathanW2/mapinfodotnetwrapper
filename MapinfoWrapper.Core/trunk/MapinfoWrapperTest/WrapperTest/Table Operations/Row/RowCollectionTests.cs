//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Moq;
//using Wrapper.TableOperations.RowOperations;
//using Wrapper;
//using Wrapper.TableOperations;
//
//namespace MapinfoWrapperTest.WrapperTest.Table_Operations.Row
//{
//    [TestFixture]
//    public class RowCollectionTests
//    {
//
//        [Test]
//        public void GetEnumeratorMustReturnRowEnumerator()
//        {
//            IMapinfoWrapper wrapper = new Mock<IMapinfoWrapper>().Object;
//            var collection = new RowList<DummyTableDef>(wrapper, "Water_Mains");
//            Assert.AreEqual(typeof(RowEnumerator<DummyTableDef>), collection.GetEnumerator().GetType());
//        }
//
//    }
//}
