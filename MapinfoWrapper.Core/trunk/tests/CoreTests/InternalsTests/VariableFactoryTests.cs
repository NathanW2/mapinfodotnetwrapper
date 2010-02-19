using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.MapbasicOperations;
using NUnit.Framework;
using Moq;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapperTest.CoreTests.InternalsTests
{
    [TestFixture]
    public class VariableFactoryTests
    {
        //private Mock<IMapinfoWrapper> mockmapinfo;

        //[SetUp]
        //public void Setup()
        //{
        //    IMapinfoWrapper wrapper = new Mock<IMapinfoWrapper>().Object;
        //    DependencyResolver resolver = new DependencyResolver();
        //    resolver.Register(typeof(IMapinfoWrapper),wrapper);
        //    IoC.Initialize(resolver);
        //}

        //[Test]
        //public void WireUp()
        //{
        //    VariableFactory factory = new VariableFactory();
        //}

        //[Test]
        //public void FacotryShouldNotAssignVariable()
        //{
        //    VariableFactory factory = new VariableFactory();
        //    IVariable variable = factory.CreateNewWithGUID(Variable.VariableType.Object);

        //    Assert.False(variable.IsAssigned);
        //}
    }
}
