using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example24_Error : IExample
{
    public string Input => ")command";
    public Expression? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.InvalidUsageOfToken, new Token(0, 1, TokenType.CloseParenthesis));
    public List<(string, TokenType)> Tokens =>
    [
        (")", TokenType.CloseParenthesis),
        ("command", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(0, 1, ExecutionError.InvalidUsageOfToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}