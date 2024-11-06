using System;
using System.Globalization;

namespace RefinedShell.Parsing
{
    internal sealed class FloatParser : ITypeParser
    {
        private readonly NumberFormatInfo _format;
        public uint ArgumentCount => 1;
        public uint OptionalCount => 0;

        public FloatParser()
        {
            _format = new NumberFormatInfo();
            _format.NegativeSign = "-";
            _format.NumberDecimalSeparator = ".";
        }

        public bool CanParse(ReadOnlySpan<string?> input)
        {
            return float.TryParse(input[0],
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent,
                _format, out float _);
        }

        public object Parse(ReadOnlySpan<string?> input)
        {
            return float.Parse(input[0]!, _format);
        }
    }
}