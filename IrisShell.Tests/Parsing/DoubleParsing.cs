using System.Collections.Generic;
using IrisShell.Parsing;
using NUnit.Framework;

namespace IrisShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(DoubleParser))]
    internal sealed class DoubleParsing : TypeParsing<double>
    {
        protected override Dictionary<string, (bool result, double value)> TestCases =>
            new Dictionary<string, (bool result, double value)>
            {
                { "0", (true, 0.0) },
                { "1", (true, 1.0) },
                { "-1", (true, -1.0) },
                { "3.14", (true, 3.14) },
                { "-3.14", (true, -3.14) },
                { "123456789.123456789", (true, 123456789.123456789) },
                { "1.7976931348623157E+308", (true, double.MaxValue) },
                { "-1.7976931348623157E+308", (true, double.MinValue) },
                { "2.2250738585072014E-308", (true, 2.2250738585072014E-308) },
                { "1e10", (true, 1e10) },
                { "-1e-10", (true, -1e-10) },
                { "2.5E3", (true, 2500.0) },
                { "1.8E308", (true, double.PositiveInfinity) },
                { "-1.8E308", (true, double.NegativeInfinity) },
                { "abc", (false, default) },
                { "", (false, default) },
                { " ", (false, default) },
                { "3.14abc", (false, default) },
                { "abc3.14", (false, default) },
                { "1,000", (false, default) },
                { "1_000", (false, default) },
                { "+", (false, default) },
                { "-", (false, default) },
                { "123.456.789", (false, default) },
                { "3..14", (false, default) },
                { "9999999999999999999999999999999", (true, 9.9999999999999996E+30) },
                { "-9999999999999999999999999999999", (true, -9.9999999999999996E+30) },
                { "2.0e309", (true, double.PositiveInfinity) },
                { "-0", (true, -0.0) },
                { "-0.0", (true, -0.0) }
            };
    }
}