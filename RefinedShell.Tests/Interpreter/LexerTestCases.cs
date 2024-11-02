using System;
using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests
{
    [TestFixture]
    [TestOf(typeof(Lexer))]
    internal sealed class LexerTestCases
    {
        private readonly Dictionary<string, List<(string, TokenType)>> _testCases
            = new Dictionary<string, List<(string, TokenType)>>
            {
                ["command arg1 arg2; teleport $(getplayerpos)"] =
                    new List<(string, TokenType)>
                    {
                        ("command", TokenType.Value),
                        ("arg1", TokenType.Value),
                        ("arg2", TokenType.Value),
                        (";", TokenType.Semicolon),
                        ("teleport", TokenType.Value),
                        ("$", TokenType.Dollar),
                        ("(", TokenType.OpenParenthesis),
                        ("getplayerpos", TokenType.Value),
                        (")", TokenType.CloseParenthesis)
                    },

                ["command 12345 67890 teleport"] =
                    new List<(string, TokenType)>
                    {
                        ("command", TokenType.Value),
                        ("12345", TokenType.Value),
                        ("67890", TokenType.Value),
                        ("teleport", TokenType.Value)
                    },

                ["move $(wrongformat); teleport here"] =
                    new List<(string, TokenType)>
                    {
                        ("move", TokenType.Value),
                        ("$", TokenType.Dollar),
                        ("(", TokenType.OpenParenthesis),
                        ("wrongformat", TokenType.Value),
                        (")", TokenType.CloseParenthesis),
                        (";", TokenType.Semicolon),
                        ("teleport", TokenType.Value),
                        ("here", TokenType.Value)
                    },

                ["teleport $(getplayerpos arg1 arg2 arg3) $(123arg2)"] =
                    new List<(string, TokenType)>
                    {
                        ("teleport", TokenType.Value),
                        ("$", TokenType.Dollar),
                        ("(", TokenType.OpenParenthesis),
                        ("getplayerpos", TokenType.Value),
                        ("arg1", TokenType.Value),
                        ("arg2", TokenType.Value),
                        ("arg3", TokenType.Value),
                        (")", TokenType.CloseParenthesis),
                        ("$", TokenType.Dollar),
                        ("(", TokenType.OpenParenthesis),
                        ("123arg2", TokenType.Value), //hmm
                        (")", TokenType.CloseParenthesis)
                    },

                ["command ; move          right 20 ; attack"] =
                    new List<(string, TokenType)>
                    {
                        ("command", TokenType.Value),
                        (";", TokenType.Semicolon),
                        ("move", TokenType.Value),
                        ("right", TokenType.Value),
                        ("20", TokenType.Value),
                        (";", TokenType.Semicolon),
                        ("attack", TokenType.Value)
                    },

                [" ; teleport"] =
                    new List<(string, TokenType)>
                    {
                        (";", TokenType.Semicolon),
                        ("teleport", TokenType.Value)
                    },

                ["command $(getpos $(get_player this)) $ ( getplayerpos )"] =
                    new List<(string, TokenType)>
                    {
                        ("command", TokenType.Value),
                        ("$", TokenType.Dollar),
                        ("(", TokenType.OpenParenthesis),
                        ("getpos", TokenType.Value),
                        ("$", TokenType.Dollar),
                        ("(", TokenType.OpenParenthesis),
                        ("get_player", TokenType.Value),
                        ("this", TokenType.Value),
                        (")", TokenType.CloseParenthesis),
                        (")", TokenType.CloseParenthesis),
                        ("$", TokenType.Dollar),
                        ("(", TokenType.OpenParenthesis),
                        ("getplayerpos", TokenType.Value),
                        (")", TokenType.CloseParenthesis)
                    },

                ["123 123 123"] = //hmm
                    new List<(string, TokenType)>
                    {
                        ("123", TokenType.Value),
                        ("123", TokenType.Value),
                        ("123", TokenType.Value)
                    },

                ["-command arg1 arg2"] =
                    new List<(string, TokenType)>
                    {
                        ("-command", TokenType.Unknown),
                        ("arg1", TokenType.Value),
                        ("arg2", TokenType.Value)
                    },

                ["-command arg@1 arg@2"] =
                    new List<(string, TokenType)>
                    {
                        ("-command", TokenType.Unknown),
                        ("arg@1", TokenType.Unknown),
                        ("arg@2", TokenType.Unknown)
                    },

                ["command $ teleport, arg1 arg2 , $(getpos)"] =
                    new List<(string, TokenType)>
                    {
                        ("command", TokenType.Value),
                        ("$", TokenType.Dollar),
                        ("teleport,", TokenType.Unknown),
                        ("arg1", TokenType.Value),
                        ("arg2", TokenType.Value),
                        (",", TokenType.Unknown),
                        ("$", TokenType.Dollar),
                        ("(", TokenType.OpenParenthesis),
                        ("getpos", TokenType.Value),
                        (")", TokenType.CloseParenthesis)
                    }
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
            Assert.That(token.Type, Is.EqualTo(TokenType.Value));
            token = _lexer.GetNextToken();
            Assert.That(token.Type, Is.EqualTo(TokenType.EndOfLine));
        }

        private List<(string, TokenType)> Tokenize(ReadOnlySpan<char> input)
        {
            List<(string, TokenType)> list = new List<(string, TokenType)>();
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
}