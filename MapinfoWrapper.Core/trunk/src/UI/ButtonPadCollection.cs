namespace MapinfoWrapper.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using MapinfoWrapper.Mapinfo;

    public class ButtonPadCollection : IEnumerable<ButtonPad>
    {
        private MapinfoSession misession;
        private IList<ButtonPad> innerlist;

        public ButtonPadCollection(MapinfoSession MISession)
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