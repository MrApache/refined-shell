namespace RefinedShell.Execution
{
    internal readonly ref struct InternalResult<T> where T : class
    {
        public readonly T? Expression;
        public readonly ProblemSegment Segment;
        public readonly uint CommandsCount;

        public InternalResult(T? expression, ProblemSegment segment, uint commandsCount)
        {
            Expression = expression;
            Segment = segment;
            CommandsCount = commandsCount;
        }

        public InternalResult(ProblemSegment segment)
        {
            Expression = null;
            Segment = segment;
            CommandsCount = 0;
        }
    }
}