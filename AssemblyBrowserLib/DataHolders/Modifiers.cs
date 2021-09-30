using System;

namespace AssemblyBrowserLib.DataHolders
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
