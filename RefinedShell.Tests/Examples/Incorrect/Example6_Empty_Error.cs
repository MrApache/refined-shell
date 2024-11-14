using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example6_Empty_Error : IExample
{
    public string Input => "";
    public Expression Expression => new Expression();
    public InterpreterException? Exception => null;
    public List<(string, TokenType)> Tokens => [];
    public ExecutionResult ExecutionResult => ExecutionResult.Empty;
    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}