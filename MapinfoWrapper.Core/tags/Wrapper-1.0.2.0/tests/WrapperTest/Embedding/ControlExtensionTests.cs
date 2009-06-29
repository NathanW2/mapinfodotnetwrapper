//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using System.Windows.Forms;
//using Moq;
//using Wrapper;
//using Wrapper.Extensions;
//using Wrapper.Embedding;
//
//namespace MapinfoWrapperTest.WrapperTest.Embedding
//{
//    [TestFixture]
//    [Ignore]
//    public class ControlExtensionTests
//    {
//        private Mock<IMapinfoWrapper> mockmapinfo;
//
//        [SetUp]
//        public void CreateMockMapinfo()
//        {
//            mockmapinfo = new Mock<IMapinfoWrapper>();
//        }
//
//        [Test]
//        public void SetApplicationWindowToControlHandle()
//        {
//            Control control = new Control();
//            mockmapinfo.Setup(m => m.RunCommand("Set Application Window {0}".FormatWith(control.Handle.ToString())))
//                .AtMostOnce();
//
//            control.SetAsMapinfoApplicationWindow(mockmapinfo.Object);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void SetNextDocumentAsControlTest()
//        {
//            Control control = new Control();
//            mockmapinfo.Setup(m => m.RunCommand("Set Next Document Parent {0} Style {1}".FormatWith(control.Handle.ToString(),
//                                                                                                    (int)NextDocumentEnum.WIN_STYLE_CHILD)))
//                       .AtMostOnce();
//
//            control.SetAsNextDocumentParent(mockmapinfo.Object,NextDocumentEnum.WIN_STYLE_CHILD);
//
//            mockmapinfo.VerifyAll();
//
//        }
//    }
//}
