using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing;

[TestFixture]
[TestOf(typeof(Vector2Parser))]
[TestOf(typeof(TypeParsers))]
internal sealed class CustomTypeParsing : TypeParsing<Vector2>
{
    protected override Dictionary<string, (bool result, Vector2 value)> TestCases =>
        new Dictionary<string, (bool result, Vector2 value)>
        {
            {"0 0", (true, Vector2.Zero)},
            {"1 1", (true, Vector2.One)},
            {"3.14 3.14", (true, new Vector2(3.14f, 3.14f))}
        };

    public CustomTypeParsing()
    {
        TypeParsers.AddParser<Vector2>(new Vector2Parser());
    }
}