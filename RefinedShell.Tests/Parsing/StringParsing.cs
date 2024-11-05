using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing;

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