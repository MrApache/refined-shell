using System;

namespace IrisShell.Parsing
{
    internal sealed class BoolParser : ITypeParser
    {
        public uint OptionsCount => 1;
        public bool CanParse(ReadOnlySpan<string> input)
        {
            return bool.TryParse(input[0], out _);
        }

        public object Parse(ReadOnlySpan<string> input)
        {
            return bool.Parse(input[0]);
        }
    }
}