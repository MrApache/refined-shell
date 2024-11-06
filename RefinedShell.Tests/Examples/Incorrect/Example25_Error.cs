using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example25_Error : IExample
{
    public string Input => "command arg1 arg2)";
    public Expression? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.InvalidUsageOfToken, new Token(17, 1, TokenType.CloseParenthesis));
    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("arg1", TokenType.Identifier),
        ("arg2", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(17, 1, ExecutionError.InvalidUsageOfToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}