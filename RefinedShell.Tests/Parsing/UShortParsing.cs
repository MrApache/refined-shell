using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(UShortParser))]
    internal sealed class UShortParsing : TypeParsing<ushort>
    {
        protected override Dictionary<string, (bool result, ushort value)> TestCases =>
            new Dictionary<string, (bool result, ushort value)>
            {
                { "0", (true, 0) },
                { "1", (true, 1) },
                { "65535", (true, 65535) },
                { "12345", (true, 12345) },
                { "54321", (true, 54321) },
                { "65536", (false, default) },
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
                { "65534", (true, 65534) },
                { "999999", (false, default) },
                { "1.0e5", (false, default) },
                { "0xFF", (false, default) }
            };
    }
}