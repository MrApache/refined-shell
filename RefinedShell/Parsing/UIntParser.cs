using System;

namespace RefinedShell.Parsing
{
    internal sealed class UIntParser : ITypeParser
    {
        public uint OptionsCount => 1;
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