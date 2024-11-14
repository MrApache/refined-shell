using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example3_Instance_Sequence_Inline : IExample
{
    public string Input => "command arg1 arg2; teleport $(getplayerpos)";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(0, 7, TokenType.Identifier), "command",
            [
                new ArgumentNode(new Token(8, 4, TokenType.Identifier), "arg1"),
                new ArgumentNode(new Token(13, 4, TokenType.Identifier), "arg2")
            ]),

            new CommandNode(new Token(19, 8, TokenType.Identifier), "teleport",
            [
                new CommandNode(new Token(30, 12, TokenType.Identifier), "getplayerpos", [], true)
            ])
        );

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("command", TokenType.Identifier),
        ("arg1", TokenType.Identifier),
        ("arg2", TokenType.Identifier),
        (";", TokenType.Semicolon),
        ("teleport", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("getplayerpos", TokenType.Identifier),
        (")", TokenType.CloseParenthesis)
    ];

    public ExecutionResult ExecutionResult => ExecutionResult.Success(Array.Empty<ExecutionResult>());

    public void RegisterCommands(Shell shell)
    {
        shell.Register(Command, "command");
        shell.Register(Teleport, "teleport");
        shell.Register(GetPlayerPos, "getplayerpos");
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.Unregister("command");
        shell.Unregister("teleport");
        shell.Unregister("getplayerpos");
    }

    private void Command(string arg1, string arg2) { }
    private void Teleport(short position) { }
    private short GetPlayerPos() { return (short)Random.Shared.Next(); }
}