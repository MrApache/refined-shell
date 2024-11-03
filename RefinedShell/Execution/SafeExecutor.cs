using System.Reflection;

namespace RefinedShell.Execution
{
    internal sealed class SafeExecutor : IExecutor
    {
        public ExecutionResult Execute(ExecutableExpression expression)
        {
            try
            {
                return expression.Execute();
            }
            catch (TargetInvocationException e)
            {
                return new ExecutionResult(false, 0, 0, ExecutionError.UnhandledException, e.InnerException);
            }
        }
    }
}