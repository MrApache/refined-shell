using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example10 : IExample
{
    public string Input => "move right; command start run; teleport $(getplayerpos)";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(0, 4, TokenType.Identifier), "move",
            [
                new ArgumentNode(new Token(5, 5, TokenType.Identifier), "right")
            ]),
            new CommandNode(new Token(12, 7, TokenType.Identifier), "command",
            [
                new ArgumentNode(new Token(20, 5, TokenType.Identifier), "start"),
                new ArgumentNode(new Token(26, 3, TokenType.Identifier), "run")
            ]),
            new CommandNode(new Token(31, 8, TokenType.Identifier), "teleport",
            [
                new CommandNode(new Token(42, 12, TokenType.Identifier), "getplayerpos", [], true)
            ])
        );

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
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
    private static void Move(string direction){}

    [ShellCommand("command")]
    private static void DoAction(string action1, string action2) { }

    [ShellCommand("teleport")]
    private static void Teleport(float position){}

    [ShellCommand("getplayerpos")]
    private static float GetPosition() => 123;
}