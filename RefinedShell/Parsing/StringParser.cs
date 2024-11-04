using System;

namespace RefinedShell.Parsing
{
    internal sealed class StringParser : ITypeParser
    {
        public uint OptionsCount => 1;

        public bool CanParse(ReadOnlySpan<string> input)
        {
            return true;
        }

        public object Parse(ReadOnlySpan<string> input)
        {
            return input[0];
        }
    }
}