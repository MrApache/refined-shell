using System;

namespace IrisShell.Parsing
{
    internal sealed class ByteParser : ITypeParser
    {
        public uint OptionsCount => 1;

        public bool CanParse(ReadOnlySpan<string> input)
        {
            return byte.TryParse(input[0], out byte _);
        }

        public object Parse(ReadOnlySpan<string> input)
        {
            return byte.Parse(input[0]);
        }
    }
}