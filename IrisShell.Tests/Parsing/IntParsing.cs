using System.Collections.Generic;
using IrisShell.Parsing;
using NUnit.Framework;

namespace IrisShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(IntParser))]
    internal sealed class IntParsing : TypeParsing<int>
    {
        protected override Dictionary<string, (bool result, int value)> TestCases =>
            new Dictionary<string, (bool result, int value)>
            {
                { "0", (true, 0) },
                { "1", (true, 1) },
                { "-1", (true, -1) },
                { "2147483647", (true, 2147483647) },
                { "-2147483648", (true, -2147483648) },
                { "123456", (true, 123456) },
                { "-123456", (true, -123456) },
                { "1,000", (false, default) },
                { "1_000", (false, default) },
                { "1,234,567", (false, default) },
                { "2_147_483_647", (false, default) },
                { "1e3", (false, default) },
                { "-1e3", (false, default) },
                { "3.14e2", (false, default) },
                { "0x7FFFFFFF", (false, default) },
                { "0b101010", (false, default) },
                { "2147483648", (false, default) },
                { "-2147483649", (false, default) },
                { "4294967295", (false, default) },
                { "-4294967296", (false, default) },
                { "abc", (false, default) },
                { "-abc", (false, default) },
                { "123abc", (false, default) },
                { "abc123", (false, default) },
                { "", (false, default) },
                { "+", (false, default) },
                { "-", (false, default) },
                { "+123", (true, 123) },
                { "-123", (true, -123) },
                { "2147483640", (true, 2147483640) },
                { "-2147483640", (true, -2147483640) },
                { "1000000000", (true, 1000000000) },
                { "-1000000000", (true, -1000000000) },
                { "123.456", (false, default) },
                { "-123.456", (false, default) },
                { "3.14", (false, default) },
                { "-3.14", (false, default) },
                { "0.9999", (false, default) },
                { "-0.9999", (false, default) },
                { "2147483647.9999", (false, default) },
                { "-2147483648.9999", (false, default) },
            };
    }
}