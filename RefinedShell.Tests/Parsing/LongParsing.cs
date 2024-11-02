using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(LongParser))]
    internal sealed class LongParsing : TypeParsing<long>
    {
        protected override Dictionary<string, (bool result, long value)> TestCases =>
            new Dictionary<string, (bool result, long value)>
            {
                { "0", (true, 0L) },
                { "1", (true, 1L) },
                { "-1", (true, -1L) },
                { "9223372036854775807", (true, 9223372036854775807L) },
                { "-9223372036854775808", (true, -9223372036854775808L) },
                { "123456789", (true, 123456789L) },
                { "-123456789", (true, -123456789L) },
                { "2147483647", (true, 2147483647L) },
                { "-2147483648", (true, -2147483648L) },
                { "1,000", (false, default) },
                { "1_000", (false, default) },
                { "1,234,567", (false, default) },
                { "9_223_372_036_854_775_807", (false, default) },
                { "1e10", (false, default) },
                { "-1e10", (false, default) },
                { "2.5e5", (false, default) },
                { "0x7FFFFFFFFFFFFFFF", (false, default) },
                { "0b101010", (false, default) },
                { "9223372036854775808", (false, default) },
                { "-9223372036854775809", (false, default) },
                { "18446744073709551615", (false, default) },
                { "-18446744073709551616", (false, default) },
                { "abc", (false, default) },
                { "-abc", (false, default) },
                { "123abc", (false, default) },
                { "abc123", (false, default) },
                { "", (false, default) },
                { "+", (false, default) },
                { "-", (false, default) },
                { "+123", (true, 123L) },
                { "-123", (true, -123L) },
                { "9223372036854775800", (true, 9223372036854775800L) },
                { "-9223372036854775800", (true, -9223372036854775800L) },
                { "1000000000000000000", (true, 1000000000000000000L) },
                { "-1000000000000000000", (true, -1000000000000000000L) },
                { "123.456", (false, default) },
                { "-123.456", (false, default) },
                { "3.14", (false, default) },
                { "-3.14", (false, default) },
                { "0.0001", (false, default) },
                { "-0.0001", (false, default) },
                { "9223372036854775807.9999", (false, default) },
                { "-9223372036854775808.9999", (false, default) }
            };
    }
}