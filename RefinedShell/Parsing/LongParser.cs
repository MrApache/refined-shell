using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class LongParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return long.TryParse(input[0], out long _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return long.Parse(input[0]!);
        }
    }
}