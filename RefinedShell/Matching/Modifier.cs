using System;

namespace RefinedShell.Matching
{
    [Flags]
    internal enum Modifier : byte
    {
        None,
        Escape,
        Symbol,
        Invert
    }
}