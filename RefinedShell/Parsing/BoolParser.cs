using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class BoolParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return bool.TryParse(input[0], out _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return bool.Parse(input[0]!);
        }
    }
}