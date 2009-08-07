using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.Wrapper.MapbasicOperations
{
    internal class MapbasicCommand
    {
        public MapbasicCommand()
        { }

        public MapbasicCommand(string mapbasicCommand)
        {
            this.Command = mapbasicCommand;
        }

        public string Command { get; set; }

        public override string ToString()
        {
            return this.Command;
        }

        public static implicit operator MapbasicCommand(string mapbasicCommand)
        {
            return new MapbasicCommand(mapbasicCommand);
        }
    }
}
