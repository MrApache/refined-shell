namespace RefinedShell.Execution
{
    internal sealed class CommandOR : ExecutableExpression
    {
        private readonly ExecutableExpression _first;
        private readonly ExecutableExpression _second;

        public CommandOR(ExecutableExpression first, ExecutableExpression second)
        {
            _first = first;
            _second = second;
        }

        public override ExecutionResult Execute()
        {
            ExecutionResult firstResult = _first.Execute();
            if (firstResult.IsSuccess)
                return firstResult;

            return _second.Execute();
        }
    }
}