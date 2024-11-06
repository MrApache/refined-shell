using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example12 : IExample
{
    public string Input => "command ; move          right 20 ; attack";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(0, 7, TokenType.Identifier),
                "command", []),
            new CommandNode(new Token(10, 4, TokenType.Identifier), "move",
            [
                new ArgumentNode(new Token(24, 5, TokenType.Identifier), "right"),
                new ArgumentNode(new Token(30, 2, TokenType.Identifier), "20")
            ]),
            new CommandNode(new Token(35, 6, TokenType.Identifier),
                "attack", [])
        );

    public InterpreterException? Exception => null;


    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        (";", TokenType.Semicolon),
        ("move", TokenType.Identifier),
        ("right", TokenType.Identifier),
        ("20", TokenType.Number),
        (";", TokenType.Semicolon),
        ("attack", TokenType.Identifier)
    ];

    public ExecutionResult ExecutionResult => new ExecutionResult(true, Array.Empty<ExecutionResult>(), ProblemSegment.None);

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }

    [ShellCommand("move")]
    private static void Move(string direction, int steps) {}

    [ShellCommand("command")]
    private static void DoAction() { }

    [ShellCommand("attack")]
    private static void Attack() { }
}