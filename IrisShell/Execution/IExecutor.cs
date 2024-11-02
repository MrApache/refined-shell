using IrisShell.Interpreter;

namespace IrisShell.Execution
{
    internal interface IExecutor
    {
        public ExecutionResult Execute(CompiledExpression expression);
    }
}