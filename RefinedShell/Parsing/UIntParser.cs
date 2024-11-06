using System;

namespace RefinedShell.Parsing
{
    internal sealed class UIntParser : ITypeParser
    {
        public uint ArgumentCount => 1;
        public uint OptionalCount => 0;
        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return uint.TryParse(input[0], out uint _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return uint.Parse(input[0]!);
        }
    }
}