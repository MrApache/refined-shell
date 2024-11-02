using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(ULongParser))]
    internal sealed class ULongParsing : TypeParsing<ulong>
    {
        protected override Dictionary<string, (bool result, ulong value)> TestCases =>
            new Dictionary<string, (bool result, ulong value)>
            {
                { "0", (true, 0ul) },
                { "1", (true, 1ul) },
                { "18446744073709551615", (true, 18446744073709551615ul) },
                { "12345678901234567890", (true, 12345678901234567890ul) },
                { "98765432109876543210", (false, default) },
                { "-1", (false, default) },
                { "18446744073709551616", (false, default) },
                { "abc", (false, default) },
                { "", (false, default) },
                { " ", (false, default) },
                { "3.14", (false, default) },
                { "3.0abc", (false, default) },
                { "1,000", (false, default) },
                { "1_000", (false, default) },
                { "+", (false, default) },
                { "-", (false, default) },
                { "99999999999999999999", (false, default) },
                { "999999999999999999999", (false, default) },
                { "1.0e19", (false, default) },
                { "0xFF", (false, default) },
                { "0b11111111", (false, default) },
                { "18446744073709551614", (true, 18446744073709551614ul) },
                { "9223372036854775807", (true, 9223372036854775807ul) }
            };
    }
}