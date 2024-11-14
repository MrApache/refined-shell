using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class SByteParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return sbyte.TryParse(input[0], out sbyte _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return sbyte.Parse(input[0]!);
        }
    }
}