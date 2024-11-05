using System;

namespace RefinedShell.Parsing
{
    internal sealed class IntParser : ITypeParser
    {
        public uint OptionsCount => 1;
        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return int.TryParse(input[0], out int _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return int.Parse(input[0]!);
        }
    }
}