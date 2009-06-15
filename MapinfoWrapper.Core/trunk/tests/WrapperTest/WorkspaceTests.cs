using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Extensions;
using Wrapper;
using NUnit.Framework;
using Moq;

namespace MapinfoWrapperTest.WrapperTest
{
    [TestFixture]
    public class WorkspaceTests
    {
        private Mock<IMapinfoWrapper> mockmapinfo;

        [SetUp]
        public void CreateMocks()
        {
            mockmapinfo = new Mock<IMapinfoWrapper>();
        }

        [Test]
        public void OpenWorkSpaceReturnsWorkspaceObject()
        {
            Assert.AreEqual(typeof(Workspace), Workspace.OpenWorkspace(mockmapinfo.Object,
                                                                       @"C:\Temp\Test.Wor").GetType());
        }

        [Test]
        public void OpenWorkspaceCallsRunApplicationInMapinfo()
        {
            mockmapinfo.Setup(m => m.RunCommand("Run Application {0}".FormatWith(@"C:\Temp\Test.Wor".InQuotes())))
                       .AtMostOnce()
                       .Verifiable();

            Workspace workspace = Workspace.OpenWorkspace(mockmapinfo.Object,
                                                          @"C:\Temp\Test.Wor");

            mockmapinfo.Verify();
        }
    }
}
