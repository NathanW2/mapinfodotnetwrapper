using MapinfoWrapper.Core.IoC;
using NUnit.Framework;
using MapinfoWrapper.Mapinfo;
using Moq;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.Internals;
using MapinfoWrapper.TableOperations;

namespace MapinfoWrapperTest.CoreTests.InternalsTests
{
    [TestFixture]
    public class TableCommandRunnerTests
    {
        Mock<IMapinfoWrapper> mockmapinfo;

        [SetUp]
        public void SetUp()
        {
            mockmapinfo = new Mock<IMapinfoWrapper>();
            DependencyResolver resolver = new DependencyResolver();
            resolver.Register(typeof(IMapinfoWrapper), mockmapinfo.Object);
            IoC.Initialize(resolver);
        }

        [Test]
        public void IsWiredUp()
        {
            IMapinfoWrapper wrapper = IoC.Resolve<IMapinfoWrapper>();
            Assert.AreSame(mockmapinfo.Object, wrapper);
        }

        [Test]
        public void OpenTableCallsMapinfoOpenTable()
        {
            TableCommandRunner runner = new TableCommandRunner();
            runner.OpenTable("TestPath");
            string expectedcommand = "Open Table {0}".FormatWith("TestPath".InQuotes());            
            mockmapinfo.Verify(m => m.RunCommand(expectedcommand));
        }

        [Test]
        public void GetNameCallsRunTableInfoForName()
        {
        	OverriddenTableCommandRunner runner = new OverriddenTableCommandRunner();
            string name = runner.GetName(0);
			
            Assert.True(runner.NameWasCalled,"Name attribute was not called");
        }
        
        [Test]
        public void RunTableInfoCallsMapinfoTablInfo()
        {
			TableCommandRunner runner = new TableCommandRunner();
            
			string value = (string)runner.RunTableInfo("DummyTable", TableInfo.Name);
			
			int enumValue = (int)TableInfo.Name;
			string command = "TableInfo(DummyTable,{0})".FormatWith(enumValue);
            mockmapinfo.Verify(m => m.Evaluate(command));
        }
    }
    
    internal class OverriddenTableCommandRunner
    	: TableCommandRunner
    {
    	public bool NameWasCalled;
		public override object RunTableInfo(string tableName, TableInfo attribute)
		{
			if (attribute == TableInfo.Name)
				this.NameWasCalled = true;
			return base.RunTableInfo(tableName,attribute);
		}
    }
}
