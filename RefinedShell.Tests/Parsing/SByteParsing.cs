using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(SByteParser))]
    internal sealed class SByteParsing : TypeParsing<sbyte>
    {
        protected override Dictionary<string, (bool result, sbyte value)> TestCases =>
            new Dictionary<string, (bool result, sbyte value)>
            {
                { "0", (true, 0) },
                { "1", (true, 1) },
                { "-1", (true, -1) },
                { "127", (true, 127) },
                { "-128", (true, -128) },
                { "128", (false, default) },
                { "-129", (false, default) },
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