using IrisShell.Interpreter;

namespace IrisShell.Execution
{
    internal sealed class UnsafeExecutor : IExecutor
    {
        public ExecutionResult Execute(CompiledExpression expression)
        {
            return expression.Execute();
        }
    }
}