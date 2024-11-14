using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class StringParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return input[0] != null;
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return input[0]!;
        }
    }
}