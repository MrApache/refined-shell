using System;
using System.Globalization;
using System.Numerics;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing;

internal sealed class Vector2Parser : ITypeParser
{
    private readonly NumberFormatInfo _format;
    public uint ArgumentCount => 2;
    public uint OptionalCount => 0;

    public Vector2Parser()
    {
        _format = new NumberFormatInfo();
        _format.NegativeSign = "-";
        _format.NumberDecimalSeparator = ".";
    }

    public bool CanParse(ReadOnlySpan<string?> input)
    {
        bool firstValue = float.TryParse(input[0],
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent,
            _format, out float _);

        bool secondValue = float.TryParse(input[1],
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent,
            _format, out float _);

        return firstValue && secondValue;
    }

    public object Parse(ReadOnlySpan<string?> input)
    {
        float x = float.Parse(input[0]!, _format);
        float y = float.Parse(input[1]!, _format);
        return new Vector2(x, y);
    }
}
