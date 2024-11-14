using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example36_Optional_Error : IExample
{
    public string Input => "SyncTime UTC local";

    public Expression Expression => new Expression(
        new CommandNode(new Token(0, 8, TokenType.Identifier), "SyncTime",
        [
            new ArgumentNode(new Token(9, 3, TokenType.Identifier), "UTC"),
            new ArgumentNode(new Token(13, 5, TokenType.Identifier), "local")
        ]));

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("SyncTime", TokenType.Identifier),
        ("UTC", TokenType.Identifier),
        ("local", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult =>
        new ExecutionResult(false, null, new ProblemSegment(9, 9, ExecutionError.TooManyArguments));

    [ShellCommand]
    private static void SyncTime(string utc) { }

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }
}