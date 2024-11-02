using System.Collections.Generic;
using IrisShell.Parsing;
using NUnit.Framework;

namespace IrisShell.Tests.Parsing
{
    [TestFixture]
    [TestOf(typeof(FloatParser))]
    internal sealed class FloatParsing : TypeParsing<float>
    {
        protected override Dictionary<string, (bool result, float value)> TestCases =>
            new Dictionary<string, (bool result, float value)>
            {
                { "0", (true, 0f) },
                { "1", (true, 1f) },
                { "-1", (true, -1f) },
                { "3.14", (true, 3.14f) },
                { "-3.14", (true, -3.14f) },
                { "123456789.123456", (true, 123456789.123456f) },
                { "3.4028235E+38", (true, float.MaxValue) },
                { "-3.4028235E+38", (true, float.MinValue) },
                { "1.401298E-45", (true, 1.401298E-45f) },
                { "1e10", (true, 1e10f) },
                { "-1e-10", (true, -1e-10f) },
                { "2.5E3", (true, 2500.0f) },
                { "3.4028236E+38", (true, float.PositiveInfinity) },
                { "-3.4028236E+38", (true, float.NegativeInfinity) },
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
                { "9999999999999999999999999999999", (true, 9.99999985E+30f) },
                { "-9999999999999999999999999999999", (true, -9.99999985E+30f) },
                { "1.0e39", (true, float.PositiveInfinity) },
                { "-0", (true, -0.0f) },
                { "-0.0", (true, -0.0f) }
            };
    }
}