using System;

namespace IrisShell.Parsing
{
    internal sealed class ULongParser : ITypeParser
    {
        public uint OptionsCount => 1;
        public bool CanParse(ReadOnlySpan<string> input)
        {
            return ulong.TryParse(input[0], out ulong _);
        }

        public object Parse(ReadOnlySpan<string> input)
        {
            return ulong.Parse(input[0]);
        }
    }
}