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

            PushButton btn = new PushButton();
            btn.ID = 3000;

            PushButton btn2 = new PushButton();
            btn2.ID = null;

            ButtonPad btnpad = new ButtonPad("Test");
            btnpad.Title = "Test ButtonPad";
            btnpad.Buttons.Add(btn);
            btnpad.Buttons.Add(btn2);

            btnpad.CreateInto(mi);

            string title = btnpad.Title;
            btnpad.Title = "Hello World";
        }
    }
}
