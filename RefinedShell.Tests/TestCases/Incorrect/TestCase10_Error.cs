using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase10_Error : ITestCase
{
    public string Input => "";

    public Node? Expression => null;

    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnexpectedToken, new Token(0, 0, TokenType.EndOfLine));

    public List<(string, TokenType)> Tokens => [];
    public ExecutionResult ExecutionResult => ExecutionResult.Empty;
    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}