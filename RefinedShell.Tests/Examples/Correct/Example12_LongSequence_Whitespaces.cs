using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example12_LongSequence_Whitespaces : StaticCommandsExample
{
    public override string Input => "command ; move          right 20 ; attack";

    public override Expression Expression =>
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

    public override List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        (";", TokenType.Semicolon),
        ("move", TokenType.Identifier),
        ("right", TokenType.Identifier),
        ("20", TokenType.Number),
        (";", TokenType.Semicolon),
        ("attack", TokenType.Identifier)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(Array.Empty<ExecutionResult>());

    [ShellCommand("move")]
    private static void Move(string direction, int steps) {}

    [ShellCommand("command")]
    private static void DoAction() { }

    [ShellCommand("attack")]
    private static void Attack() { }
}