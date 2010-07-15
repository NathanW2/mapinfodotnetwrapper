using MapInfo.Wrapper.Mapinfo;
using System.Collections;
using System.Collections.Generic;

namespace MapInfo.Wrapper.UI
{
    public class ButtonPadCollection : IEnumerable<ButtonPad>
    {
        private MapInfoSession misession;
        private IList<ButtonPad> innerlist;

        public ButtonPadCollection(MapInfoSession MISession)
        {
            this.misession = MISession;
            this.innerlist = new List<ButtonPad>();
        }
    
        public void Add(ButtonPad buttonPad)
        {
            string commandstring = buttonPad.ToCreateCommand();
            this.misession.Do(commandstring);

            buttonPad.MISession = this.misession;

            // Set the Mapinfo session for each button in the pad to the pads instance.
            foreach (var btn in buttonPad)
            {
                btn.MISession = this.misession;
            }
        }

        public IEnumerator<ButtonPad> GetEnumerator()
        {
            return this.innerlist.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}