//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Wrapper;
//using System.Windows.Forms;
//using Wrapper.Embedding;
//using Wrapper.Extensions;
//
//namespace MapinfoWrapperTest.WrapperTest.Embedding
//{
//    [TestFixture]
//    public class SystemInfoTests
//    {
//        private Moq.Mock<IMapinfoWrapper> mockmapinfo;
//
//        [SetUp]
//        public void CreateMockMapinfo()
//        {
//            mockmapinfo = new Moq.Mock<IMapinfoWrapper>();
//        }
//
//        [Test]
//        public void GetMapinfoFrameHandleHandleTest()
//        {
//            string expectedcommand = "SystemInfo({0})".FormatWith((int)SystemInfoEnum.SYS_INFO_MAPINFOWND);
//            mockmapinfo.Setup(m => m.Evaluate(expectedcommand))
//                       .Returns("123456");
//            
//            IMapinfoWrapper wrappedmapinfo = mockmapinfo.Object;
//
//            IntPtr expected = new IntPtr(123456);
//            Wrapper.Embedding.SystemInfo sysinfo = new Wrapper.Embedding.SystemInfo(wrappedmapinfo);
//            Assert.AreEqual(expected, sysinfo.MapinfoFrameHandle);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void GetMapinfoFrameHandleZeroTest()
//        {
//            string expectedcommand = "SystemInfo({0})".FormatWith((int)SystemInfoEnum.SYS_INFO_MAPINFOWND);
//            mockmapinfo.Setup(m => m.Evaluate(expectedcommand))
//                       .Returns("0");
//
//            IMapinfoWrapper wrappedmapinfo = mockmapinfo.Object;
//
//            IntPtr expected = new IntPtr(0);
//            SystemInfo sysinfo = new SystemInfo(wrappedmapinfo);
//            Assert.AreEqual(expected, sysinfo.MapinfoFrameHandle);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void GetMdiClientHandleTest()
//        {
//            string expectedcommand = "SystemInfo({0})".FormatWith((int)SystemInfoEnum.SYS_INFO_MDICLIENTWND);
//            mockmapinfo.Setup(m => m.Evaluate(expectedcommand))
//                       .Returns("123456");
//
//            IMapinfoWrapper wrappedmapinfo = mockmapinfo.Object;
//
//            IntPtr expected = new IntPtr(123456);
//            SystemInfo sysinfo = new SystemInfo(wrappedmapinfo);
//            Assert.AreEqual(expected, sysinfo.MdiHandle);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void GetMdiClientHandleZeroTest()
//        {
//            string expectedcommand = "SystemInfo({0})".FormatWith((int)SystemInfoEnum.SYS_INFO_MDICLIENTWND);
//            mockmapinfo.Setup(m => m.Evaluate(expectedcommand))
//                       .Returns("0");
//
//            IMapinfoWrapper wrappedmapinfo = mockmapinfo.Object;
//
//            IntPtr expected = new IntPtr(0);
//            SystemInfo sysinfo = new SystemInfo(wrappedmapinfo);
//            Assert.AreEqual(expected, sysinfo.MdiHandle);
//
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void RunSystemInfoCommandTest()
//        {
//            string expectedcommand = "SystemInfo({0})".FormatWith((int)SystemInfoEnum.SYS_INFO_PRODUCTLEVEL);
//            mockmapinfo.Setup(m => m.Evaluate(expectedcommand))
//                       .Returns("9.5");
//
//            string expected = "9.5";
//            SystemInfo sysinfo = new SystemInfo(mockmapinfo.Object);
//            Assert.AreEqual(expected, sysinfo.RunSystemInfoCommand(SystemInfoEnum.SYS_INFO_PRODUCTLEVEL));
//        }
//    }
//}
