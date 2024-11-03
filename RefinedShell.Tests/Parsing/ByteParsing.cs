using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing;

[TestFixture]
[TestOf(typeof(ByteParser))]
internal sealed class ByteParsing : TypeParsing<byte>
{
    protected override Dictionary<string, (bool result, byte value)> TestCases =>
        new Dictionary<string, (bool result, byte value)>
        {
            { "0", (true, 0) },
            { "1", (true, 1) },
            { "255", (true, 255) },
            { "100", (true, 100) },
            { "42", (true, 42) },
            { "200", (true, 200) },
            { "256", (false, default) },
            { "-1", (false, default) },
            { "300", (false, default) },
            { "999", (false, default) },
            { "123.456", (false, default) },
            { "0.5", (false, default) },
            { "3.14", (false, default) },
            { "-0.99", (false, default) },
            { "1e2", (false, default) },
            { "2.5e1", (false, default) },
            { "abc", (false, default) },
            { "true", (false, default) },
            { "false", (false, default) },
            { "1,000", (false, default) },
            { "1_000", (false, default) },
            { "", (false, default) },
            { " ", (false, default) },
            { "0xFF", (false, default) },
            { "0b11111111", (false, default) }
        };
}