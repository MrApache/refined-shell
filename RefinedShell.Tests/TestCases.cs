using System;
using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Execution;
using RefinedShell.Interpreter;
using RefinedShell.Tests.Examples;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Compiler))]
[TestOf(typeof(Parser))]
[TestOf(typeof(Lexer))]
internal sealed class TestCases
{
    private readonly Shell _shell;
    private readonly Parser _parser;
    private readonly Lexer _lexer;

    public TestCases()
    {
        _shell = new Shell();
        _parser = new Parser();
        _lexer = new Lexer();
    }

    [Test]
    public void Lexer()
    {
        foreach (IExample example in ExampleCollection.Examples)
        {
            List<(string, TokenType)> tokenizeResult = Tokenize(example.Input);
            Assert.That(tokenizeResult.Count, Is.EqualTo(example.Tokens.Count), $"Input: {example.Input}");
            for (int i = 0; i < tokenizeResult.Count; i++)
            {
                ValueTuple<string, TokenType> token = tokenizeResult[i];
                ValueTuple<string, TokenType> testCaseResult = example.Tokens[i];
                bool stringEquals = token.Item1.Equals(testCaseResult.Item1);
                bool typeEquals = token.Item2 == testCaseResult.Item2;
                Assert.That(stringEquals, Is.True, $"Input: {example.Input}, Expected: {token.Item1}, But was: {testCaseResult.Item1}");
                Assert.That(typeEquals, Is.True, $"Input: {example.Input}, Type:{testCaseResult.Item2}, ActualType: {token.Item2}");
            }
        }
    }
    
    [Test]
    public void Lexer_EndOfLine()
    {
        _lexer.SetInputString("command");
        Token token = _lexer.GetNextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.Identifier));
        token = _lexer.GetNextToken();
        Assert.That(token.Type, Is.EqualTo(TokenType.EndOfLine));
    }

    private List<(string, TokenType)> Tokenize(ReadOnlySpan<char> input)
    {
        List<(string, TokenType)> list = [];
        _lexer.SetInputString(input.ToString());
        Token token;
        while((token = _lexer.GetNextToken()).Type != TokenType.EndOfLine)
        {
            string str = input.Slice(token.Start, token.Length).ToString();
            list.Add(new ValueTuple<string, TokenType>(str, token.Type));
        }
        return list;
    }

    [Test]
    public void Parser()
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

    [Test]
    public void Compiler()
    {
        foreach (IExample example in ExampleCollection.Examples)
        {
            example.RegisterCommands(_shell);
            ExecutionResult actualResult = _shell.Execute(example.Input);
            Assert.That(actualResult == example.ExecutionResult, Is.True, $"Input: {example.Input}\nActual result: {actualResult}\nExpected: {example.ExecutionResult}");
            example.UnregisterCommands(_shell);
        }
    }
}

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
