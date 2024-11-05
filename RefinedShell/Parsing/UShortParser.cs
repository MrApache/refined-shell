using System;

namespace RefinedShell.Parsing
{
    internal sealed class UShortParser : ITypeParser
    {
        public uint OptionsCount => 1;
        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return ushort.TryParse(input[0], out ushort _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return ushort.Parse(input[0]!);
        }
    }
}