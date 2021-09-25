using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    [Flags]
    public enum Modifiers
    {
        Abstract = 1,
        Virtual = 2,
        Static = 4,
        Sealed = 8,
        Readonly = 16
    }
}
