using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class TestCase1_Instance : ITestCase
{
    public string Input => "print \"hello, world!!!()@(@_)_\"";

    public Node Expression =>
        new CommandNode(new Token(0, 5, TokenType.Identifier), "print",
        [
            new ArgumentNode(new Token(7, 23, TokenType.String), "hello, world!!!()@(@_)_")
        ]);

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("print", TokenType.Identifier),
        ("\"hello, world!!!()@(@_)_\"", TokenType.String)
    ];

    public ExecutionResult ExecutionResult => ExecutionResult.Success();

    private void Print(string message)
    {
        Console.WriteLine(message);
    }

    public void RegisterCommands(Shell shell)
    {
        shell.Register(Print, "print");
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.Unregister("print");
    }
}