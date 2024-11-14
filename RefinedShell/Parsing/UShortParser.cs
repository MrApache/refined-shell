using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class UShortParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

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