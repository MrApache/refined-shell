using System;

namespace RefinedShell.Parsing
{
    internal sealed class ULongParser : ITypeParser
    {
        public uint ArgumentCount => 1;
        public uint OptionalCount => 0;
        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return ulong.TryParse(input[0], out ulong _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return ulong.Parse(input[0]!);
        }
    }
}