namespace RefinedShell.Execution
{
    internal sealed class UnsafeExecutor : IExecutor
    {
        public ExecutionResult Execute(ExecutableExpression expression)
        {
            return expression.Execute();
        }
    }
}