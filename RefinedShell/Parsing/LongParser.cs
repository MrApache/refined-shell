using System;

namespace RefinedShell.Parsing
{
    internal sealed class LongParser : ITypeParser
    {
        public uint OptionsCount => 1;
        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return long.TryParse(input[0], out long _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return long.Parse(input[0]!);
        }
    }
}