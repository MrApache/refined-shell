using System;
using System.Collections.Generic;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests.Examples.Correct;

internal sealed class Example1 : IExample
{
    public string Input => "print \"hello, world!!!()@(@_)_\"";

    public Expression Expression =>
        new Expression(
            new CommandNode(new Token(0, 5, TokenType.Identifier), "print",
            [
                new ArgumentNode(new Token(7, 23, TokenType.String), "hello, world!!!()@(@_)_")
            ]));

    public InterpreterException? Exception => null;

    public List<(string, TokenType)> Tokens =>
    [
        ("print", TokenType.Identifier),
        ("\"hello, world!!!()@(@_)_\"", TokenType.String)
    ];

    public ExecutionResult ExecutionResult => new ExecutionResult(true, null, ProblemSegment.None);

    [ShellCommand("print")]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    public void RegisterCommands(Shell shell)
    {
        shell.RegisterAllWithAttribute(this);
    }

    public void UnregisterCommands(Shell shell)
    {
        shell.UnregisterAllWithAttribute(this);
    }
}