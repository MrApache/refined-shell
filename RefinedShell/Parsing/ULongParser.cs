using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class ULongParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return ulong.TryParse(input[0], out ulong _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return ulong.Parse(input[0]!);
        }
    }
}