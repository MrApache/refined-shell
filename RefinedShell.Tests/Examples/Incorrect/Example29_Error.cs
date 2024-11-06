using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example29_Error : IExample
{
    public string Input => "command $(command";
    public Expression? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnexpectedToken, new Token(17, 0, TokenType.EndOfLine));
    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("command", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(17, 0, ExecutionError.UnexpectedToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}