using System;
using System.Collections.Generic;
using System.Globalization;

namespace RefinedShell.Parsing
{
    internal sealed class FloatParser : ITypeParser
    {
        private readonly NumberFormatInfo _format;

        public IEnumerator<ArgumentInfo> GetArgumentInfo()
        {
            yield return new ArgumentInfo(1, false);
        }

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