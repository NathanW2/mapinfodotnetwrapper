//using NUnit.Framework;
//using Moq;
//using Wrapper;
//using Wrapper.Extensions;
//using System;
//using Wrapper.MapbasicOperations;
//using Wrapper.ObjectOperations;
//using Specifications;
//using X = Xunit;
//
//
//namespace MapinfoWrapperTest.WrapperTest.MapbasicOperations
//{
//    [TestFixture]
//    public class MapbasicObjectVariableTests
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
//        public void DeclareReturnsNewMapbasicObjectVariable()
//        {
//            Type objecttype = MapbasicVariable.Declare(mockmapinfo.Object, "Test","Object").GetType();
//            Assert.AreEqual(typeof(MapbasicVariable), objecttype);
//        }
//
//        [Test]
//        public void DeclareMustCallDim()
//        {
//            mockmapinfo.Setup(m => m.RunCommand("Dim Test as Object")).AtMostOnce();
//            MapbasicVariable variable = MapbasicVariable.Declare(mockmapinfo.Object, "Test","Object");
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void ToStringReturnObjectTypeString()
//        {
//            mockmapinfo.Setup(m => m.Evaluate("Test")).Returns("Region");
//            MapbasicVariable variable = MapbasicVariable.Declare(mockmapinfo.Object, "Test","Object");
//            Assert.AreEqual("Region", variable.ToString());
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void AssignReturnsSameInstaceMBObjectVarible()
//        {
//            MapbasicVariable variable = MapbasicVariable.Declare(mockmapinfo.Object, "Test","Object");
//            Assert.AreSame(variable,variable.AssignFromMapbasicCommand(It.IsAny<String>()));
//        }
//
//        [Test]
//        public void AssignCallsEquals()
//        {
//            mockmapinfo.Setup(m => m.RunCommand("Test = expression"));
//            MapbasicVariable variable = MapbasicVariable.Declare(mockmapinfo.Object, "Test","Object");
//            variable.AssignFromMapbasicCommand("expression");
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void DisposeCallsUnDim()
//        {
//            mockmapinfo.Setup(m => m.RunCommand("Undim Test"));
//            MapbasicVariable variable = MapbasicVariable.Declare(mockmapinfo.Object,"Test","Object");
//            variable.Dispose();
//            mockmapinfo.VerifyAll();
//        }
//
//        [Test]
//        public void VariableTypeReturnsTypeDeclaredWith()
//        {
//            MapbasicVariable variable = MapbasicVariable.Declare(mockmapinfo.Object, "Test", "Object");
//            Assert.AreEqual(variable.DeclaredAs, "Object");
//        }
//    }
//}
