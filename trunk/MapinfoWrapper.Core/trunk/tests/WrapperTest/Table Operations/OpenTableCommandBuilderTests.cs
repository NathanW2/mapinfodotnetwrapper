//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Wrapper.Extensions;
//using Wrapper.TableOperations;
//
//namespace MapinfoWrapperTest.WrapperTest.Table_Operations
//{
//    [TestFixture]
//    public class OpenTableCommandBuilderTest
//    {
//        [Test]
//        public void SetTablePath()
//        {
//            string path = @"C:\Temp\Test.Tab";
//            string expected = @"Open Table {0}".FormatWith(path.InQuotes());
//
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder(path);
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString());
//        }
//
//        [Test]
//        public void SetTableName()
//        {
//            string expected = @"Open Table {0} as {1}".FormatWith("NullPath".InQuotes()
//                                                                  ,"Test");
//
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.HasName("Test");
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString());
//        }
//
//        [Test]
//        public void SetHidden()
//        {
//            string expected = @"Open Table {0} Hide".FormatWith("NullPath".InQuotes());
//
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.IsHidden();
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString()); 
//        }
//
//        [Test]
//        public void SetReadOnly()
//        {
//            string expected = @"Open Table {0} ReadOnly".FormatWith("NullPath".InQuotes());
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.IsReadOnly();
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString()); 
//        }
//
//        [Test]
//        public void SetInteractive()
//        {
//            string expected = @"Open Table {0} Interactive".FormatWith("NullPath".InQuotes());
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.OpenAsInteractive();
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString()); 
//        }
//
//        [Test]
//        public void SetPassword()
//        {
//            string expected = @"Open Table {0} Password {1}".FormatWith("NullPath".InQuotes()
//                                                                       ,"TestPass".InQuotes());
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.Password("TestPass");
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString());          
//        }
//
//        [Test]
//        public void SetNoIndex()
//        {
//            string expected = @"Open Table {0} NoIndex".FormatWith("NullPath".InQuotes());
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.CreateNoIndex();
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString());    
//   
//        }
//
//        [Test]
//        public void SetAutoView()
//        {
//            string expected = @"Open Table {0} View Automatic".FormatWith("NullPath".InQuotes());
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.AutomaticView();
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString());    
//        }
//
//        [Test]
//        public void SetDenyWrite()
//        {
//            string expected = @"Open Table {0} DenyWrite".FormatWith("NullPath".InQuotes());
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.DenyWrite();
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString());      
//        }
//
//        [Test]
//        public void SetGridType()
//        {
//            string expected = @"Open Table {0} VMGrid".FormatWith("NullPath".InQuotes());
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder("NullPath");
//
//            opentablecommands.GridHadler(GridHandleEnum.VMGrid);
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString());  
//        }
//
//        [Test]
//        public void MaximunPossibleBuilderReturnString()
//        {
//            string expected = @"Open Table {0} as {2} Hide ReadOnly Interactive Password {1} NoIndex View Automatic DenyWrite VMGrid".FormatWith(@"C:\Temp\Test.Tab".InQuotes(),
//                                                                                                              "TestPass".InQuotes(),
//                                                                                                              "Test");
//            OpenTableCommandBuilder opentablecommands = new OpenTableCommandBuilder(@"C:\Temp\Test.Tab");
//
//            opentablecommands.HasName("Test")
//                             .IsHidden()
//                             .IsReadOnly()
//                             .OpenAsInteractive()
//                             .Password("TestPass")
//                             .CreateNoIndex()
//                             .AutomaticView()
//                             .DenyWrite()
//                             .GridHadler(GridHandleEnum.VMGrid);
//
//            Assert.AreEqual(expected, opentablecommands.BuildCommandString());
//        }
//    }
//}
