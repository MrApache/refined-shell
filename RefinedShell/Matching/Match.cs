namespace RefinedShell.Matching
{
    internal readonly struct Match
    {
        public readonly int Start;
        public readonly int Length;
        public readonly bool Success;

        public Match(int start, int length, bool success)
        {
            Start = start;
            Length = length;
            Success = success;
        }
    }
}
