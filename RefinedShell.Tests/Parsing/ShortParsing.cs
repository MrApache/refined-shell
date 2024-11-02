using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(ShortParser))]
    internal sealed class ShortParsing : TypeParsing<short>
    {
        protected override Dictionary<string, (bool result, short value)> TestCases =>
            new Dictionary<string, (bool result, short value)>
            {
                { "0", (true, 0) },
                { "1", (true, 1) },
                { "-1", (true, -1) },
                { "32767", (true, 32767) },
                { "-32768", (true, -32768) },
                { "32768", (false, default) },
                { "-32769", (false, default) },
                { "abc", (false, default) },
                { "", (false, default) },
                { " ", (false, default) },
                { "3.14", (false, default) },
                { "3.0abc", (false, default) },
                { "1,000", (false, default) },
                { "1_000", (false, default) },
                { "+", (false, default) },
                { "-", (false, default) },
                { "999999999999999", (false, default) },
                { "-999999999999999", (false, default) },
                { "1.0e3", (false, default) },
                { "-0", (true, 0) },
                { "-0.0", (false, default) }
            };
    }
}