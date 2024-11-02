using System.Reflection;
using IrisShell.Interpreter;

namespace IrisShell.Execution
{
    internal sealed class SafeExecutor : IExecutor
    {
        public ExecutionResult Execute(CompiledExpression expression)
        {
            try
            {
                return expression.Execute();
            }
            catch (TargetInvocationException e)
            {
                return new ExecutionResult(false, e.InnerException);
            }
        }
    }
}