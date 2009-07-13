namespace MapinfoWrapper.Embedding
{
    using System;

    /// <summary>
    /// The base class for all button pad butttons in Mapinfo.
    /// </summary>
    public abstract class MIButton
    {
        public int? ID { get; set;}
        protected string IDFormated
        {
            get
            {
                if (this.ID == null)
                    return "";
                else return "ID " + this.ID.Value;

            }
        }
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

        public Action Calls { get; set; }

        public abstract string ToButtonString();
    }
}