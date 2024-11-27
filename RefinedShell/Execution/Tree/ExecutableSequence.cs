namespace RefinedShell.Execution
{
    internal sealed class ExecutableSequence : ExecutableExpression
    {
        private readonly ExecutableExpression _first;
        private readonly ExecutableExpression _second;

        private readonly ExecutionResult[] _pool;

        public ExecutableSequence(ExecutableExpression first, ExecutableExpression second)
        {
            _first = first;
            _second = second;
            _pool = new ExecutionResult[2];
        }

        public override ExecutionResult Execute()
        {
            _pool[0] = _first.Execute();
            _pool[1] = _second.Execute();
            return ExecutionResult.Success(_pool);
        }
    }
}