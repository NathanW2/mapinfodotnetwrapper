namespace MapinfoWrapper.Embedding
{
    using System;
    using Mapinfo;

    /// <summary>
    /// The base class for all button pad butttons in Mapinfo.
    /// </summary>
    public abstract class MIButton
    {
        public delegate void ButtonDelegate(MIButton button);
        public event ButtonDelegate ButtonClicked;

        /// <summary>
        /// Gets or sets the ID of the button.
        /// </summary>
        public int ID { get; set;}
        protected string IDFormated
        {
            get
            {
                return "ID " + this.ID;
            }
        }

        /// <summary>
        /// Gets or sets if the button is enabled.
        /// </summary>
        public bool Enabled { get; set; }
        protected string EnabledFormated
        {
            get
            {
                if (this.Enabled)
                    return "Enable";
                else 
                    return "Disable";
            }
        }

        /// <summary>
        /// Clickes the current button in Mapinfo.
        /// <para>Throws a <see cref="ArgumentNullException"/> if the button doesn't belong to a button pad in a Mapinfo instance.</para>
        /// <para></para>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the button doesn't belong to a button pad in a Mapinfo instance.</exception>
        public void Raise()
        {
            if (misession == null) 
                throw new ArgumentNullException("misession","The button pad doesn't belong to a buttonpad that is created in Mapinfo.");

            // This only handles running custom menus ATM
            this.misession.RunCommand("Run Menu Command " + this.IDFormated);

            if (ButtonClicked != null)
                    ButtonClicked(this);
        }

        private MapinfoSession misession;
        public MapinfoSession MISession
        {
            get
            {
                return misession;
            }
            internal set
            {
                this.misession = value;
            }
        }

        public abstract string ToButtonString();
    }
}