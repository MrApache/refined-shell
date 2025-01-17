using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class TestCase20_Error : ITestCase
{
    public string Input => "command $";
    public Node? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnexpectedToken, new Token(9, 0, TokenType.EndOfLine));
    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("$", TokenType.Dollar)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(9, 0, ExecutionError.UnexpectedToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}