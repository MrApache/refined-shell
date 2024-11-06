using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Incorrect;

internal sealed class Example28_Error : IExample
{
    public string Input => "command $(@command)";
    public Expression? Expression => null;
    public InterpreterException Exception =>
        new InterpreterException(ExecutionError.UnknownToken, new Token(10, 1, TokenType.Unknown));

    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("@", TokenType.Unknown),
        ("command", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(10, 1, ExecutionError.UnknownToken));

    public void RegisterCommands(Shell shell) { }
    public void UnregisterCommands(Shell shell) { }
}