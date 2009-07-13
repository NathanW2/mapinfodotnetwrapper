using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.Embedding
{
    using Core;
    using Core.Extensions;
    using Mapinfo;

    public interface IButtonPad
    {
        /// <summary>
        /// The name of the button pad in Mapinfo. 
        /// <para>This property can not be altered once set by the constructor.</para>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets and Sets the title of the buttonpad.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The collection of buttons that the button pad owns.
        /// </summary>
        ICollection<MIButton> Buttons { get; }

        /// <summary>
        /// Toogles the buttons pad visible property.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets and Sets the style of the button pad in Mapinfo.
        /// </summary>
        ButtonPad.Style PadStyle { get; set; }
    }

    /// <summary>
    /// Represents a button pad in Mapinfo.
    /// </summary>
    public class ButtonPad : IButtonPad
    {
        public enum Style
        {
            Fixed = 0,
            Floating = 1
        }

        private ICollection<MIButton> buttons = new List<MIButton>();

        public ButtonPad(string name)
        {
            Guard.AgainstNullOrEmpty(name, "name");
            this.Name = name;
        }

        private string name;
        /// <summary>
        /// The name of the button pad in Mapinfo. 
        /// <para>This property can not be altered once set by the constructor.</para>
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                this.name = value;
            }
        }

        private string title;

        private MIButtonPad MIPadProxy;

        /// <summary>
        /// Gets and Sets the title of the buttonpad.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;    
            }
        }
        
        /// <summary>
        /// Returns a mapbasic formated string title string.
        /// </summary>
        /// <returns></returns>
        private string GetTitleFormated()
        {
            return String.IsNullOrEmpty(this.Title) ? "" : " Title " + this.Title.InQuotes();
        }
    
        /// <summary>
        /// The collection of buttons that the button pad owns.
        /// </summary>
        public ICollection<MIButton> Buttons
        {
            get
            {
                return buttons;
            }
        }

        /// <summary>
        /// Returns a mapbasic formated string for the buttons in the button pad.
        /// </summary>
        /// <returns></returns>
        private string GetButtonsFormated()
        {
            string buttonsstring = "";
            foreach (var btn in this.Buttons)
            {
                buttonsstring += " " + btn.ToButtonString();
            }
            return buttonsstring;
        }

        /// <summary>
        /// Toogles the buttons pad visible property.
        /// </summary>
        public bool Visible { get; set; }
        private string GetVisibleFormated()
        {
            if (this.Visible)
                return " Show ";
            else
                return " Hide ";

        }

        /// <summary>
        /// Gets and Sets the style of the button pad in Mapinfo.
        /// </summary>
        public Style PadStyle { get; set; }

        private string GetStyleFormatted()
        {
            if (this.PadStyle == Style.Fixed)
                return " Fixed";
            else
                return " Floating";
        }

        /// <summary>
        /// Creates the button pad in Mapinfo.  Returns the newly created button pad in Mapinfo.
        /// </summary>
        /// <returns>The button pad created in Mapinfo as a <see cref="ButtonPad"/></returns>
        public ButtonPad CreateInto(MapinfoSession MISession)
        {
            string commandstring = @"Create ButtonPad {0} as 
                                            {2}    
                                            {1}
                                            {3}
                                            {4}".FormatWith(this.Name.InQuotes(),
                                                            this.GetTitleFormated(),
                                                            this.GetButtonsFormated(),
                                                            this.GetVisibleFormated(),
                                                            this.GetStyleFormatted());
            MISession.RunCommand(commandstring);
            this.MIPadProxy = new MIButtonPad(MISession,this.Name);
            return this;
        }
    }

    internal class MIButtonPad : IButtonPad
    {
        private readonly MapinfoSession MiSession;
        private readonly string name;

        public MIButtonPad(MapinfoSession MISession, string padName)
        {
            MiSession = MISession;
            this.name = padName;
        }

        /// <summary>
        /// The name of the button pad in Mapinfo. 
        /// <para>This property can not be altered once set by the constructor.</para>
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        private string title;
        /// <summary>
        /// Gets and Sets the title of the buttonpad.
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                string titlestring = "";
                if (!string.IsNullOrEmpty(value))
                {
                    titlestring = "Title " + value.InQuotes();
                }
                this.MiSession.RunCommand("Alter ButtonPad {0} Title {1}".FormatWith(this.Name.InQuotes(), titlestring));
                this.title = value;
            }
        }

        /// <summary>
        /// The collection of buttons that the button pad owns.
        /// </summary>
        public ICollection<MIButton> Buttons
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Toogles the buttons pad visible property.
        /// </summary>
        public bool Visible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets and Sets the style of the button pad in Mapinfo.
        /// </summary>
        public ButtonPad.Style PadStyle
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
