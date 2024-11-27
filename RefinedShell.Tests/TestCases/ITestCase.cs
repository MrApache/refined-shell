using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests;

internal interface ITestCase
{
    public string Input { get; }
    public Node? Expression { get; }
    public InterpreterException? Exception { get; }
    public List<(string, TokenType)> Tokens { get; }
    public ExecutionResult ExecutionResult { get; }
    public void RegisterCommands(Shell shell);
    public void UnregisterCommands(Shell shell);
}