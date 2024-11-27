using System;

namespace RefinedShell.Matching
{
    internal ref struct MatchContext
    {
        public readonly ReadOnlySpan<char> Text;
        public int Start;
        public int Length;

        public char CurrentChar => Text[Start + Length];
        public char FirstChar => Text[Start];

        public MatchContext(ReadOnlySpan<char> input)
        {
            Text = input;
            Length = 0;
            Start = 0;
        }
    }
}
