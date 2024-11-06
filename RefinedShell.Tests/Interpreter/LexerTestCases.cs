using System;
using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Interpreter;
using RefinedShell.Tests.Examples;
using TokenType = RefinedShell.Interpreter.TokenType;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Lexer))]
internal sealed class LexerTestCases
{
    private Lexer _lexer = null!;

    [SetUp]
    public void Setup()
    {
        _lexer = new Lexer();
    }

    [Test]
    public void Tokenize()
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
    public void TokenizeEndOfLine()
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
}