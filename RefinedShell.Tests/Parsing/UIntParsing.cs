using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(UIntParser))]
    internal sealed class UIntParsing : TypeParsing<uint>
    {
        protected override Dictionary<string, (bool result, uint value)> TestCases =>
            new Dictionary<string, (bool result, uint value)>
            {
                { "0", (true, 0u) },
                { "1", (true, 1u) },
                { "4294967295", (true, 4294967295u) },
                { "123456789", (true, 123456789u) },
                { "987654321", (true, 987654321u) },
                { "4294967296", (false, default) },
                { "-1", (false, default) },
                { "abc", (false, default) },
                { "", (false, default) },
                { " ", (false, default) },
                { "3.14", (false, default) },
                { "3.0abc", (false, default) },
                { "1,000", (false, default) },
                { "1_000", (false, default) },
                { "+", (false, default) },
                { "-", (false, default) },
                { "9999999999", (false, default) },
                { "99999999999", (false, default) },
                { "4.0e9", (false, default) },
                { "0xFF", (false, default) },
                { "0b11111111", (false, default) }
            };
    }
}