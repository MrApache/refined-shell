namespace RefinedShell.Execution
{
    internal interface IExecutor
    {
        public ExecutionResult Execute(ExecutableExpression expression);
    }
}