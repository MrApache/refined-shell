using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;
using RefinedShell.Tests.Examples;

namespace RefinedShell.Tests;

internal sealed class TestCase42_IfElse : StaticCommandsTestCase
{
    public override string Input => "send_request; wait_answer && print $(get_answer) || print \"Error: 404\"";
    public override Node Expression =>
        new SequenceNode(
            new CommandNode(new Token(0,12, TokenType.Identifier), "send_request", []),
            new LogicalNode(
                new CommandNode(new Token(14, 11, TokenType.Identifier), "wait_answer", []),
                new LogicalNode(
                    new CommandNode(new Token(29, 5, TokenType.Identifier), "print",
                        [
                            new CommandNode(new Token(37, 10, TokenType.Identifier), "get_answer", [], true),
                        ]),
                    new CommandNode(new Token(52, 5, TokenType.Identifier), "print",
                        [
                            new ArgumentNode(new Token(58,12, TokenType.String), "\"Error: 404\"")
                        ]),
                    LogicalNode.LogicalNodeType.Or
                    ),
                LogicalNode.LogicalNodeType.And)
                );

    public override List<(string, TokenType)> Tokens =>
    [
        ("send_request", TokenType.Identifier),
        (";", TokenType.Semicolon),
        ("wait_answer", TokenType.Identifier),
        ("&", TokenType.Ampersand),
        ("&", TokenType.Ampersand),
        ("print", TokenType.Identifier),
        ("$", TokenType.Dollar),
        ("(", TokenType.OpenParenthesis),
        ("get_answer", TokenType.Identifier),
        (")", TokenType.CloseParenthesis),
        ("|", TokenType.VerticalBar),
        ("|", TokenType.VerticalBar),
        ("print", TokenType.Identifier),
        ("\"Error: 404\"", TokenType.String)
    ];

    //Success
    /*
    public override ExecutionResult ExecutionResult =>
        ExecutionResult.Success(new[]
            {
                ExecutionResult.Success(),
                ExecutionResult.Success(new []
                {
                    ExecutionResult.Success(),
                    ExecutionResult.Success()
                })
            });
    */

    //Fail
    public override ExecutionResult ExecutionResult =>
        ExecutionResult.Success(new[]
            {
                ExecutionResult.Success(),
                ExecutionResult.Success(new []
                {
                    ExecutionResult.Success(),
                    ExecutionResult.Success()
                })
            });

    private static string _request = string.Empty;

    [ShellFunction("send_request")]
    private static void SendRequest()
    {
    }

    [ShellFunction("wait_answer")]
    private static void WaitAnswer() {}

    [ShellFunction("get_answer")]
    private static string GetAnswer()
    {
        return "Hello!!!";
    }

    [ShellFunction("print")]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }
}