using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapinfoWrapper.Core.Internals
{
    internal interface ITableCommandRunner
    {
        string GetName(int tableNumber);

        void OpenTable(string p);
    }
}
