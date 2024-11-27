using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class TestCase13_Error : ITestCase
{
    public string Input => "123 123 123";

    public Node? Expression => null;

    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnexpectedToken, new Token(0, 3, TokenType.Number));

    public List<(string, TokenType)> Tokens =>
    [
        ("123", TokenType.Number),
        ("123", TokenType.Number),
        ("123", TokenType.Number)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(0, 3, ExecutionError.UnexpectedToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}