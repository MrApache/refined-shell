using System;

namespace RefinedShell.Parsing
{
    internal sealed class BoolParser : ITypeParser
    {
        public uint ArgumentCount => 1;
        public uint OptionalCount => 0;
        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return bool.TryParse(input[0], out _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return bool.Parse(input[0]!);
        }
    }
}