using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example9 : IExample
{
    public string Input => "$(command arg1 arg2)";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(2, 7, TokenType.Identifier), "command",
            [
                new ArgumentNode(new Token(9, 4, TokenType.Identifier), "arg1"),
                new ArgumentNode(new Token(13, 4, TokenType.Identifier), "arg2")
            ], true));

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("command", TokenType.Identifier),
        ("arg1", TokenType.Identifier),
        ("arg2", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult => new ExecutionResult(true, "arg1arg2", ProblemSegment.None);

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }

    [ShellCommand("command")]
    private static string FormatString(string arg1, string arg2)
    {
        return arg1 + arg2;
    }
}