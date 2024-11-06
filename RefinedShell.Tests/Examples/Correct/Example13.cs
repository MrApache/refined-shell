using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example13 : IExample
{
    public string Input => " ; teleport";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(3, 8, TokenType.Identifier),
                "teleport", [])
        );
    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        (";", TokenType.Semicolon),
        ("teleport", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult => new ExecutionResult(true, null, ProblemSegment.None);

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }
    
    [ShellCommand("teleport")]
    private static void Teleport(){}
}