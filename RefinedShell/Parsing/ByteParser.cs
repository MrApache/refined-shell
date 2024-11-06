using System;

namespace RefinedShell.Parsing
{
    internal sealed class ByteParser : ITypeParser
    {
        public uint ArgumentCount => 1;
        public uint OptionalCount => 0;

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return byte.TryParse(input[0], out byte _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return byte.Parse(input[0]!);
        }
    }
}