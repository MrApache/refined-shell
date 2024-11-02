using System;

namespace IrisShell.Parsing
{
    public interface ITypeParser
    {
        public uint OptionsCount { get; }
        public bool CanParse(ReadOnlySpan<string> input);
        public object Parse(ReadOnlySpan<string> input);
    }
}