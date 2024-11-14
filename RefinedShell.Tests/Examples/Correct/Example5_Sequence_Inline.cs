using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example5_Sequence_Inline : StaticCommandsExample
{
    public override string Input => "move $(wrongformat); teleport here";

    public override Expression Expression =>
        new Expression(
            new CommandNode(new Token(0, 4, TokenType.Identifier), "move",
            [
                new CommandNode(new Token(7, 11, TokenType.Identifier), "wrongformat", [], true)
            ]),
            new CommandNode(new Token(21, 8, TokenType.Identifier), "teleport",
            [
                new ArgumentNode(new Token(30, 4, TokenType.Identifier), "here")
            ])
        );

    public override List<(string, TokenType)> Tokens =>
    [
        ("move", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("wrongformat", TokenType.Identifier),
        (")", TokenType.CloseParenthesis),
        (";", TokenType.Semicolon),
        ("teleport", TokenType.Identifier),
        ("here", TokenType.Identifier)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(new []
    {
        new ExecutionResult(true, 993, ProblemSegment.None),
        new ExecutionResult(true, true, ProblemSegment.None)
    });

    [ShellCommand("move")]
    private static int Move(int position) => 993;

    [ShellCommand("wrongformat")]
    private static int GetPosition() => Random.Shared.Next();

    [ShellCommand("teleport")]
    private static bool Teleport(string place) => true;
}