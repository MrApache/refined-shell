using System;

namespace RefinedShell.Parsing
{
    internal sealed class StringParser : ITypeParser
    {
        public uint OptionsCount => 1;

        public bool CanParse(ReadOnlySpan<string> input)
        {
            if (input.IsEmpty)
                return true;

            #if NET8_0_OR_GREATER
            if (!input[0].Contains(' '))
                return true;
            #else
            if (!input[0].Contains(" ", StringComparison.InvariantCultureIgnoreCase))
                return true;
            #endif

            if(input[0][0] == '\"' && input[0][^1] == '\"')
                return true;

            return false;
        }

        public object Parse(ReadOnlySpan<string> input)
        {
            return input[0];
        }
    }
}