using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class ByteParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

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