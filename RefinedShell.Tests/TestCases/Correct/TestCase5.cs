using System.Collections.Generic;
using RefinedShell.Execution;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase5 : StaticCommandsTestCase
{
    public override string Input => "move right; command start run; teleport $(getplayerpos)";

    public override Node Expression =>
        new SequenceNode(
            new CommandNode(new Token(0, 4, TokenType.Identifier), "move",
            [
                new ArgumentNode(new Token(5, 5, TokenType.Identifier), "right")
            ]),
            new SequenceNode(
                new CommandNode(new Token(12, 7, TokenType.Identifier), "command",
                [
                    new ArgumentNode(new Token(20, 5, TokenType.Identifier), "start"),
                    new ArgumentNode(new Token(26, 3, TokenType.Identifier), "run")
                ]),
                new CommandNode(new Token(31, 8, TokenType.Identifier), "teleport",
                [
                    new CommandNode(new Token(42, 12, TokenType.Identifier), "getplayerpos", [], true)
                ])
            )
        );

    public override List<(string, TokenType)> Tokens =>
    [
        ("move", TokenType.Identifier),
        ("right", TokenType.Identifier),
        (";", TokenType.Semicolon),
        ("command", TokenType.Identifier),
        ("start", TokenType.Identifier),
        ("run", TokenType.Identifier),
        (";", TokenType.Semicolon),
        ("teleport", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("getplayerpos", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public override ExecutionResult ExecutionResult => ExecutionResult.Success(
        new[]
        {
            ExecutionResult.Success(),
            ExecutionResult.Success(new []
            {
                ExecutionResult.Success(),
                ExecutionResult.Success()
            })
        });

    [ShellFunction("move")]
    private static void Move(string direction){}

    [ShellFunction("command")]
    private static void DoAction(string action1, string action2) { }

    [ShellFunction("teleport")]
    private static void Teleport(float position){}

    [ShellFunction("getplayerpos")]
    private static float GetPosition() => 123;
}