namespace MapinfoWrapper.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Core;
    using Core.Extensions;
    using Mapinfo;

    /// <summary>
    /// Represents a button pad in Mapinfo.
    /// 
    /// <para>Call CreateInto method after pad setup to create the button pad and buttons in Mapinfo.</para>
    /// <para><B>Any calls to the button pad or buttons after the CreateInto method will make direct changes in Mapinfo.</B></para>
    /// </summary>
    public class ButtonPad : ICollection<MIButton>
    {
        /// <summary>
        /// The style used to display the button pad.
        /// </summary>
        public enum Style
        {
            Fixed = 0,
            Floating = 1
        }

        private ICollection<MIButton> buttons;
        private MapinfoSession misession;

        public ButtonPad(string title)
        {
            Guard.AgainstNullOrEmpty(title, "name");

            this.Title = title;
            this.buttons = new List<MIButton>();
        }

        private string title;

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
                string titlestring = "";
                if (this.misession != null)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        titlestring = "Title " + value.InQuotes();
                    }
                    this.misession.RunCommand("Alter ButtonPad {0} {1}".FormatWith(this.Title.InQuotes(), titlestring));
                }               
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
        /// Returns a mapbasic formated string for the buttons in the button pad.
        /// </summary>
        /// <returns></returns>
        private string GetButtonsFormated()
        {
            string buttonsstring = "";
            foreach (var btn in this)
            {
                buttonsstring += " " + btn.ToButtonString();
            }
            return buttonsstring;
        }

        private bool visible;

        /// <summary>
        /// Toogles the buttons pad visible property.
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.misession != null)
                {
                    string command = this.visible ? "Hide" : "Show";
                    this.misession.RunCommand("Alter ButtonPad {0} {1}".FormatWith(this.Title.InQuotes(), command));
                }
                this.visible = value;
            }
        }

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
        /// Adds an button to the button pad.
        /// </summary>
        /// <param name="item">The <see cref="MIButton"/> to add to the <see cref="ButtonPad"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Add(MIButton item)
        {
            Guard.AgainstNull(item,"item");

            if (this.IsReadOnly) throw new NotSupportedException("The collection is read-only");

            // TODO: Add some logic to handle adding the same button.

            if (misession != null)
            {
                string command = item.ToButtonString();
                this.misession.RunCommand("Alter ButtonPad {0} Add {1}".FormatWith(this.Title.InQuotes(),
                                                                                   command));
                item.MISession = this.misession;
            }
            this.buttons.Add(item);
        }

        /// <summary>
        /// Removes buttons items from the button pad.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. 
        ///                 </exception>
        public void Clear()
        {
            if (this.IsReadOnly) throw new NotSupportedException("The Collection is read-only");

            string idlist = "";
            foreach (MIButton btn in this)
            {
                idlist = " ID " + btn.ID + ",";
            }
            idlist = idlist.TrimEnd(',');
            
            if (misession != null)
            {
                this.misession.RunCommand("Alter ButtonPad {0} Remove {1}".FormatWith(this.Title.InQuotes(), idlist));
            }

            this.buttons.Clear();
        }

        /// <summary>
        /// Determines whether the button pad contains a specific <see cref="MIButton"/>.
        /// </summary>
        /// <returns>true if <paramref name="item"/> is found in the button pad; otherwise, false.</returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(MIButton item)
        {
            return this.buttons.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.
        ///                 </param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.
        ///                 </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.
        ///                 </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.
        ///                 </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.
        ///                     -or-
        ///                 <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        ///                     -or-
        ///                     The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        ///                     -or-
        ///                     Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        ///                 </exception>
        public void CopyTo(MIButton[] array, int arrayIndex)
        {
            this.buttons.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///                 </exception>
        public bool Remove(MIButton item)
        {
            if (this.misession != null)
            {
                string command = "ID " + item.ID;
                this.misession.RunCommand("Alter ButtonPad {0} Remove {1}".FormatWith(this.Title.InQuotes(),command));
            }
            return this.buttons.Remove(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get
            {
                return this.buttons.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get
            {
                return this.buttons.IsReadOnly;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<MIButton> GetEnumerator()
        {
            return this.buttons.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public MapinfoSession MISession
        {
            get { return misession; }
            internal set { misession = value; }
        }

        /// <summary>
        /// Returns the command string that represents Create ButtonPad mapbasic command.
        /// </summary>
        /// <returns>A Mapbasic command string for the Create ButtonPad command.</returns>
        public string ToCreateCommand()
        {
            string commandstring = @"Create ButtonPad {0} as 
                                            {2}    
                                            {1}
                                            {3}
                                            {4}".FormatWith(this.Title.InQuotes(),
                                                            this.GetTitleFormated(),
                                                            this.GetButtonsFormated(),
                                                            this.GetVisibleFormated(),
                                                            this.GetStyleFormatted());
            return commandstring;
        }
  }
}
