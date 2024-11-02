using System.Collections.Generic;
using IrisShell.Parsing;
using NUnit.Framework;

namespace IrisShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(BoolParser))]
    internal sealed class BoolParsing : TypeParsing<bool>
    {
        protected override Dictionary<string, (bool result, bool value)> TestCases =>
            new Dictionary<string, (bool result, bool value)>
            {
                { "true", (true, true) },
                { "false", (true, false) },
                { "True", (true, true) },
                { "False", (true, false) },
                { "TRUE", (true, true) },
                { "FALSE", (true, false) },
                { "TruE", (true, true) },
                { "fAlse", (true, false) },
                { "1", (false, default) },
                { "0", (false, default) },
                { "-1", (false, default) },
                { "yes", (false, default) },
                { "no", (false, default) },
                { "on", (false, default) },
                { "off", (false, default) },
                { "T", (false, default) },
                { "F", (false, default) },
                { "", (false, default) },
                { " ", (false, default) },
                { "abc", (false, default) },
                { "123", (false, default) },
                { "-abc", (false, default) },
                { "yesplease", (false, default) },
                { "nope", (false, default) }
            };
    }
}