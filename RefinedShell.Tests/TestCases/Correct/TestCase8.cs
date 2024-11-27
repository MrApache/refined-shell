using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase8 : StaticCommandsTestCase
{
    public override string Input => "move $(wrongformat); teleport here";

    public override Node Expression =>
        new SequenceNode(
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

    [ShellFunction("move")]
    private static int Move(int position) => 993;

    [ShellFunction("wrongformat")]
    private static int GetPosition() => Random.Shared.Next();

    [ShellFunction("teleport")]
    private static bool Teleport(string place) => true;
}