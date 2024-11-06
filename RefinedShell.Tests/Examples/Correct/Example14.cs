using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example14 : IExample
{
    public string Input => "command $(getpos $(get_player this)) $ ( getplayerpos )";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(0, 7, TokenType.Identifier),
                "command",
                [
                    new CommandNode(new Token(10, 6, TokenType.Identifier),
                        "getpos",
                        [
                            new CommandNode(new Token(19, 10, TokenType.Identifier),
                                "get_player",
                                [
                                    new ArgumentNode(new Token(30, 4, TokenType.Identifier), "this")
                                ], true)
                        ], true),
                    new CommandNode(new Token(41, 12, TokenType.Identifier),
                        "getplayerpos", [], true)
                ])
        );

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("getpos", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("get_player", TokenType.Identifier),
        ("this", TokenType.Identifier),
        (")", TokenType.CloseParenthesis),
        (")", TokenType.CloseParenthesis),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("getplayerpos", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
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

    [ShellCommand("command")]
    private static void DoAction(float position, float playerPosition) { }

    [ShellCommand("getpos")]
    private static float GetPos(bool playerFound) => 123123;

    [ShellCommand("get_player")]
    private static bool GetPlayer(string name) => true;

    [ShellCommand("getplayerpos")]
    private static float GetPlayerPos() => 2343243;

}