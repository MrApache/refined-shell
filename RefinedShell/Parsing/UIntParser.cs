using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class UIntParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return uint.TryParse(input[0], out uint _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return uint.Parse(input[0]!);
        }
    }
}