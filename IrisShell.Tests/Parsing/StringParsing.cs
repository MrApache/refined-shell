using System.Collections.Generic;
using IrisShell.Parsing;
using NUnit.Framework;

namespace IrisShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(StringParser))]
    internal sealed class StringParsing : TypeParsing<string>
    {
        protected override Dictionary<string, (bool result, string value)> TestCases =>
            new Dictionary<string, (bool result, string value)>
            {
                {
                    "whatever", (true, "whatever")
                }
            };
    }
}