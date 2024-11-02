using System.Reflection;
using RefinedShell.Execution;

namespace RefinedShell
{
    public interface ICommand
    {
        public string Name { get; }
        public ParameterInfo[] Arguments { get; }
        public bool ReturnsResult { get; }
        public ExecutionResult Execute(object[] args);
        public bool IsValid();
    }
}