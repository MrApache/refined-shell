namespace IrisShell.Utilities
{
    internal readonly struct StringSegment
    {
        public readonly StringToken Source;
        public readonly short Start;
        public readonly short Length;

        public StringSegment(StringToken source, short start, short length)
        {
            Source = source;
            Start = start;
            Length = length;
        }

        public void Deconstruct(out short start, out short length)
        {
            start = Start;
            length = Length;
        }
    }
}