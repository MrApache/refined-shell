using System;

namespace RefinedShell.Parsing
{
    internal sealed class SByteParser : ITypeParser
    {
        public uint OptionsCount => 1;

        public bool CanParse(ReadOnlySpan<string> input)
        {
            return sbyte.TryParse(input[0], out sbyte _);
        }

        public object Parse(ReadOnlySpan<string> input)
        {
            return sbyte.Parse(input[0]);
        }
    }
}