using System;
using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Interpreter;
using TokenType = RefinedShell.Interpreter.TokenType;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Lexer))]
internal sealed class LexerTestCases
{
    private readonly Dictionary<string, List<(string, TokenType)>> _testCases
        = new Dictionary<string, List<(string, TokenType)>>
        {

            ["print \"hello, world!!!()@(@_)_\""] =
            [
                ("print", TokenType.Identifier),
                ("\"hello, world!!!()@(@_)_\"", TokenType.String)
            ],

            ["print \"Hello world@-42=[]]-'l.\""] =
            [
                ("print", TokenType.Identifier),
                ("\"Hello world@-42=[]]-'l.\"", TokenType.String)
            ],

            ["command arg1 arg2; teleport $(getplayerpos)"] =
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
            ],

            ["command 12345 67890 teleport"] =
            [
                ("command", TokenType.Identifier),
                ("12345", TokenType.Number),
                ("67890", TokenType.Number),
                ("teleport", TokenType.Identifier)
            ],

            ["move $(wrongformat); teleport here"] =
            [
                ("move", TokenType.Identifier),
                ("$", TokenType.Dollar),
                ("(", TokenType.OpenParenthesis),
                ("wrongformat", TokenType.Identifier),
                (")", TokenType.CloseParenthesis),
                (";", TokenType.Semicolon),
                ("teleport", TokenType.Identifier),
                ("here", TokenType.Identifier)
            ],
            ["teleport $(getplayerpos arg1 arg2 arg3) $(123arg2)"] =
            [
                ("teleport", TokenType.Identifier),
                ("$", TokenType.Dollar),
                ("(", TokenType.OpenParenthesis),
                ("getplayerpos", TokenType.Identifier),
                ("arg1", TokenType.Identifier),
                ("arg2", TokenType.Identifier),
                ("arg3", TokenType.Identifier),
                (")", TokenType.CloseParenthesis),
                ("$", TokenType.Dollar),
                ("(", TokenType.OpenParenthesis),
                ("123", TokenType.Number), //Error
                ("arg2", TokenType.Identifier), //Error
                (")", TokenType.CloseParenthesis)
            ],
            ["command ; move          right 20 ; attack"] =
            [
                ("command", TokenType.Identifier),
                (";", TokenType.Semicolon),
                ("move", TokenType.Identifier),
                ("right", TokenType.Identifier),
                ("20", TokenType.Number),
                (";", TokenType.Semicolon),
                ("attack", TokenType.Identifier)
            ],

            [" ; teleport"] =
            [
                (";", TokenType.Semicolon),
                ("teleport", TokenType.Identifier)
            ],

            ["command $(getpos $(get_player this)) $ ( getplayerpos )"] =
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
            ],

            ["123 123 123"] =
            [
                ("123", TokenType.Number),
                ("123", TokenType.Number),
                ("123", TokenType.Number)
            ],

            ["-command arg1 arg2"] =
            [
                ("-command", TokenType.Unknown),
                ("arg1", TokenType.Identifier),
                ("arg2", TokenType.Identifier)
            ],

            ["-command arg@1 arg@2"] =
            [
                ("-command", TokenType.Unknown),
                ("arg", TokenType.Identifier),
                ("@1", TokenType.Unknown),
                ("arg", TokenType.Identifier),
                ("@2", TokenType.Unknown)
            ],

            ["command $ teleport, arg1 arg2 , $(getpos)"] =
            [
                ("command", TokenType.Identifier),
                ("$", TokenType.Dollar),
                ("teleport", TokenType.Identifier),
                (", arg1", TokenType.Unknown),
                ("arg2", TokenType.Identifier),
                (", $", TokenType.Unknown),
                ("(", TokenType.OpenParenthesis),
                ("getpos", TokenType.Identifier),
                (")", TokenType.CloseParenthesis)
            ],
            ["print \"message \" \" "] =
            [
                ("print", TokenType.Identifier),
                ("\"message \"", TokenType.String),
                ("\" ", TokenType.Unknown)
            ]
        };

    private Lexer _lexer = null!;

    [SetUp]
    public void Setup()
    {
        _lexer = new Lexer();
    }

    [Test]
    public void Tokenize()
    {
        foreach ((string input, List<(string, TokenType)> result) in _testCases)
        {
            List<(string, TokenType)> tokenizeResult = Tokenize(input);
            Assert.That(tokenizeResult.Count, Is.EqualTo(result.Count), $"Input: {input}");
            for (int i = 0; i < tokenizeResult.Count; i++)
            {
                ValueTuple<string, TokenType> token = tokenizeResult[i];
                ValueTuple<string, TokenType> testCaseResult = result[i];
                bool stringEquals = token.Item1.Equals(testCaseResult.Item1);
                bool typeEquals = token.Item2 == testCaseResult.Item2;
                Assert.That(stringEquals, Is.True, $"Expected: {token.Item1}, But was: {testCaseResult.Item1}");
                Assert.That(typeEquals, Is.True, $"Input: {input}, Type:{testCaseResult.Item2}, ActualType: {token.Item2}");
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