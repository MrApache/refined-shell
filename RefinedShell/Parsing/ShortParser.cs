using System;

namespace RefinedShell.Parsing
{
    internal sealed class ShortParser : ITypeParser
    {
        public uint OptionsCount => 1;
        public bool CanParse(ReadOnlySpan<string> input)
        {
            return short.TryParse(input[0], out short _);
        }

        public object Parse(ReadOnlySpan<string> input)
        {
            return short.Parse(input[0]);
        }
    }
}