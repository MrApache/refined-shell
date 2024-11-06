using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests;

internal interface IExample
{
    public string Input { get; }
    public Expression? Expression { get; }
    public InterpreterException? Exception { get; }
    public List<(string, TokenType)> Tokens { get; }
    public ExecutionResult ExecutionResult { get; }
    public void RegisterCommands(Shell shell);
    public void UnregisterCommands(Shell shell);
}