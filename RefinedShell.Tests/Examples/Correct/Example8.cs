using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example8 : IExample
{
    public string Input => "$(command)";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(2, 7, TokenType.Identifier), "command", [], true));

    public InterpreterException? Exception => null;
    public List<(string, TokenType)> Tokens =>
    [
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("command", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult => new ExecutionResult(true, 993, ProblemSegment.None);

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }

    [ShellCommand("command")]
    private static int GetRandom()
    {
        return 1000 - 7;
    }
}