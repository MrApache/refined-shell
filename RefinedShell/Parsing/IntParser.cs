using System;
using System.Collections.Generic;

namespace RefinedShell.Parsing
{
    internal sealed class IntParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return int.TryParse(input[0], out int _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return int.Parse(input[0]!);
        }
    }
}