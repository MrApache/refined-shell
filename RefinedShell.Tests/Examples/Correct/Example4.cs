using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example4 : IExample
{
    public string Input => "command 12345 67890";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(0, 7, TokenType.Identifier), "command",
            [
                new ArgumentNode(new Token(8, 5, TokenType.Number), "12345"),
                new ArgumentNode(new Token(14, 5, TokenType.Number), "67890")
            ]));

    public InterpreterException? Exception => null;
    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("12345", TokenType.Number),
        ("67890", TokenType.Number)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(true, 12345 + 67890, ProblemSegment.None);

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }

    [ShellCommand("command")]
    private static int Command(short value1, int value2)
    {
        return value1 + value2;
    }
}