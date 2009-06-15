using System;
using NUnit.Framework;
using Moq;
using MapinfoWrapper.Mapinfo;
using System.Runtime.InteropServices;
using MapinfoWrapper.Exceptions;
using MapinfoWrapper.Mapinfo.Internals;

namespace MapinfoWrapperTest.WrapperTest.MapinfoFactoryTest
{
	[TestFixture]
	public class COMMapinfoTests
	{
		private Mock<DMapInfo> mockmapinfo;
		
		[SetUp]
		public void Setup()
		{
			mockmapinfo = new Mock<DMapInfo>();
		}
		
		[Test]
		public void RunCommandWithInvaildArgRethrowsMapinfoExceptionFromCOMException()
		{
			mockmapinfo.Setup(m => m.Do("InvailedArg"))
					   .Throws<COMException>();
			
			COMMapinfo mapinfo = new COMMapinfo(mockmapinfo.Object);
			
			Assert.Throws<MapinfoException>(() => mapinfo.RunCommand("InvailedArg"));
		}
		
		[Test]
		public void EvaluateWithInvaildArgRethrowsMapinfoExceptionFromCOMException()
		{
			mockmapinfo.Setup(m => m.Eval("InvailedArg"))
					   .Throws<COMException>();
			
			COMMapinfo mapinfo = new COMMapinfo(mockmapinfo.Object);
			
			Assert.Throws<MapinfoException>(() => mapinfo.Evaluate("InvailedArg"));
		}
		
		[Test]
		public void GetUnderlyingInstanceReturnsAssignedInstance()
		{
			DMapInfo expected = mockmapinfo.Object;
			
			COMMapinfo mapinfo = new COMMapinfo(expected);
			
			object result = mapinfo.GetUnderlyingMapinfoInstance();
			
			Assert.IsInstanceOf(typeof(DMapInfo),result);
			Assert.AreSame(expected,result);
		}
		
		[Test]
		public void RunCommandPassedNullOrEmptyStringShouldThrow(
			[Values("",null)] string command)
		{
			COMMapinfo mapinfo = new COMMapinfo(mockmapinfo.Object);
			Assert.Throws<ArgumentNullException>(() => mapinfo.RunCommand(command));
		}
		
		[Test]
		public void EvaluatePassedNullOrEmptyStringShouldThrow(
			[Values("",null)] string command)
		{
			COMMapinfo mapinfo = new COMMapinfo(mockmapinfo.Object);
			Assert.Throws<ArgumentNullException>(() => mapinfo.Evaluate(command));
		}
		
		[Test]
		public void MapinfoWithErrorCodeGreaterThenZeroShouldThrow()
		{
			mockmapinfo.Setup(m => m.LastErrorCode)
					   .Returns(1);
			
			COMMapinfo mapinfo = new COMMapinfo(mockmapinfo.Object);
            Assert.Throws<MapinfoException>(() => mapinfo.RunCommand("DummyArg"),"RunCommand didn't throw a MapinfoException");
            Assert.Throws<MapinfoException>(() => mapinfo.Evaluate("DummyArg"),"Evaluate didn't throw a MapinfoException");
		}

        [Test]
        public void MapinfoWithNoErrorCodeNotThrow()
        {
            mockmapinfo.Setup(m => m.LastErrorCode)
                       .Returns(0);

            COMMapinfo mapinfo = new COMMapinfo(mockmapinfo.Object);
            Assert.DoesNotThrow(() => mapinfo.RunCommand("DummyArg"), "RunCommand threw an Exception");
            Assert.DoesNotThrow(() => mapinfo.Evaluate("DummyArg"), "Evaluate threw an Exception");
        }
	}
}
