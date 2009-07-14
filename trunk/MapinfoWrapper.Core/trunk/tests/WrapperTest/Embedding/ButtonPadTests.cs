using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapperTest.WrapperTest.Embedding
{
    using MapinfoWrapper.Mapinfo;
    using MapinfoWrapper.Embedding;

    using NUnit.Framework;

    [TestFixture]
    public class ButtonPadTests
    {
        [Test]
        public void Create_Should_Create_Buttonpad_in_Mapinfo()
        {
            COMMapinfo mi = COMMapinfo.CreateInstance();
            mi.Visible = true;

            PushButton btn = new PushButton(3000);

            PushButton btn2 = new PushButton(4000);

            ButtonPad btnpad = new ButtonPad("Test");
            btnpad.Title = "Test ButtonPad";
            btnpad.Visible = true;
            btnpad.Add(btn);
            btnpad.Add(btn2);

            btnpad.CreateInto(mi);

            string title = btnpad.Title;
            btnpad.Title = "Hello World";

            bool visible = btnpad.Visible;
            btnpad.Visible = false;
        }

        [Test]
        public void Count_Should_be_less_after_remove()
        {
            ButtonPad btnpad = new ButtonPad("Test");
            PushButton btn1 = new PushButton(3000);
            PushButton btn2 = new PushButton(4000);

            btnpad.Add(btn1);
            btnpad.Add(btn2);

            int count = btnpad.Count;

            btnpad.Remove(btn1);

            Assert.Less(btnpad.Count,count);
        }

        [Test]
        public void Count_Should_be_less_after_remove_after_create()
        {
            COMMapinfo mi = COMMapinfo.CreateInstance();
            mi.Visible = true;

            ButtonPad btnpad = new ButtonPad("Test");
            PushButton btn1 = new PushButton(3000);
            PushButton btn2 = new PushButton(4000);

            btnpad.Add(btn1);
            btnpad.Add(btn2);

            int count = btnpad.Count;

            btnpad.CreateInto(mi);

            btnpad.Remove(btn1);

            Assert.Less(btnpad.Count, count);
        }
    }
}
