//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Wrapper.Embedding;
//using System.Windows.Forms;
//
//namespace MapinfoWrapperTest.WrapperTest.Embedding
//{
//    [TestFixture]
//    public class MapinfoWindowHandleTests
//    {
//        [Test]
//        public void ConstructorTest()
//        {
//            IntPtr handle = new IntPtr(100);
//            MapinfoWindowHandle winhandle = new MapinfoWindowHandle(handle);
//            Assert.AreEqual(handle, ((IWin32Window)winhandle).Handle);
//        }
//
//    }
//}
