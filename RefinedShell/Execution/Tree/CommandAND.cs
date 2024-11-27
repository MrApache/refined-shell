namespace RefinedShell.Execution
{
    internal sealed class CommandAND : ExecutableExpression
    {
        private readonly ExecutableExpression _first;
        private readonly ExecutableExpression _second;

        private readonly ExecutionResult[] _pool;

        public CommandAND(ExecutableExpression first, ExecutableExpression second)
        {
            _first = first;
            _second = second;
            _pool = new ExecutionResult[2];
        }

        public override ExecutionResult Execute()
        {
            ExecutionResult firstResult = _first.Execute();
            if (!firstResult.IsSuccess)
                return firstResult;

            ExecutionResult secondResult = _second.Execute();
            _pool[0] = firstResult; _pool[1] = secondResult;
            return ExecutionResult.Success(_pool);
        }
    }
}