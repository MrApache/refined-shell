using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class TestCase16_Error : ITestCase
{
    public string Input => ";";
    public Node? Expression => null;

    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.InvalidUsageOfToken, new Token(0, 1, TokenType.Semicolon));
    public List<(string, TokenType)> Tokens =>
    [
        (";", TokenType.Semicolon)
    ];

    public ExecutionResult ExecutionResult => ExecutionResult.Error(new ProblemSegment(0, 1, ExecutionError.InvalidUsageOfToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}