using System;
using NUnit.Framework;
using RefinedShell.Interpreter;
using RefinedShell.Tests.Examples;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Parser))]
internal sealed class ParserTestCases
{
        /*
        {
            "teleport $(getplayerpos current) $(123arg2)", //hmm
            new Expression(
                new CommandNode(new Token(0, 8, TokenType.Identifier), "teleport",
                [
                    new CommandNode(new Token(11, 12, TokenType.Identifier),"getplayerpos",
                    [
                        new ArgumentNode(new Token(24, 7, TokenType.Identifier), "current")
                    ], true),
                    new CommandNode(new Token(35, 7, TokenType.Identifier),"123arg2", [], true)
                ])
            )
        },
        */

    private Parser _parser = null!;

    [SetUp]
    public void Setup()
    {
        _parser = new Parser();
    }

    [Test]
    public void ParseValidTestCases()
    {
        foreach (IExample example in ExampleCollection.Examples)
        {
            try
            {
                Expression actual = _parser.GetExpression(example.Input);
                Assert.That(actual, Is.EqualTo(example.Expression));
            }
            catch(Exception e)
            {
                if(e is InterpreterException ie)
                {
                    Assert.That(ie.Token, Is.EqualTo(example.Exception!.Token),
                    $"Input: {example.Input}, Actual position: {ie.Token.Start}, Excepted: {example.Exception.Token.Start}");
                    Assert.That(ie.Error, Is.EqualTo(example.Exception.Error));
                }
                else
                {
                    Assert.Fail($"Unhandled exception: {e}");
                }
            }
        }
    }
}