using System.Reflection;
using IrisShell.Interpreter;

namespace IrisShell
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