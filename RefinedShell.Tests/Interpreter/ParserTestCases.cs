using System;
using System.Collections.Generic;
using NUnit.Framework;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Parser))]
internal sealed class ParserTestCases
{
    private readonly Dictionary<string, Expression> _testCases = new Dictionary<string, Expression>
    {
        {
            "",
            new Expression()
        },
        {
            ";",
            new Expression()
        },
        {
            "$(command)",
            new Expression(
                new CommandNode(new Token(2, 7, TokenType.Value), "command", [], true))
        },
        {
            "$(command arg1 arg2)",
            new Expression(
                new CommandNode(new Token(2, 7, TokenType.Value), "command",
                [
                    new ArgumentNode(new Token(9, 4, TokenType.Value), "arg1"),
                    new ArgumentNode(new Token(13, 4, TokenType.Value), "arg2")
                ], true))
        },
        {
            "command arg1 arg2; teleport $(getplayerpos)",
            new Expression(
                new CommandNode(new Token(0, 7, TokenType.Value), "command",
                [
                    new ArgumentNode(new Token(8, 4, TokenType.Value), "arg1"),
                    new ArgumentNode(new Token(13, 4, TokenType.Value), "arg2")
                ]),

                new CommandNode(new Token(19, 8, TokenType.Value), "teleport",
                [
                    new CommandNode(new Token(30, 12, TokenType.Value),"getplayerpos", [], true)
                ])
            )
        },
        {
            "move $(wrongformat); teleport here",
            new Expression(
                new CommandNode(new Token(0, 4, TokenType.Value),"move",
                [
                    new CommandNode(new Token(7, 11, TokenType.Value),"wrongformat", [], true)
                ]),
                new CommandNode(new Token(21, 8, TokenType.Value), "teleport",
                [
                    new ArgumentNode(new Token(30, 4, TokenType.Value),"here")
                ])
            )
        },
        {
            "teleport $(getplayerpos current) $(123arg2)", //hmm
            new Expression(
                new CommandNode(new Token(0, 8, TokenType.Value), "teleport",
                [
                    new CommandNode(new Token(11, 12, TokenType.Value),"getplayerpos",
                    [
                        new ArgumentNode(new Token(24, 7, TokenType.Value), "current")
                    ], true),
                    new CommandNode(new Token(35, 7, TokenType.Value),"123arg2", [], true)
                ])
            )
        },
        {
            "move right; command start run; teleport $(getplayerpos)",
            new Expression(
                new CommandNode(new Token(0, 4, TokenType.Value),"move",
                [
                    new ArgumentNode(new Token(5, 5, TokenType.Value),"right")
                ]),
                new CommandNode(new Token(12, 7, TokenType.Value) ,"command",
                [
                    new ArgumentNode(new Token(20, 5, TokenType.Value),"start"),
                    new ArgumentNode(new Token(26, 3, TokenType.Value),"run")
                ]),
                new CommandNode(new Token(31, 8, TokenType.Value),"teleport",
                [
                    new CommandNode(new Token(42, 12, TokenType.Value),"getplayerpos", [], true)
                ])
            )
        },
        {
            "command ; move          right 20 ; attack",
            new Expression(
                new CommandNode(new Token(0, 7, TokenType.Value),
                    "command", []),
                new CommandNode(new Token(10, 4, TokenType.Value), "move",
                [
                    new ArgumentNode(new Token(24, 5, TokenType.Value),"right"),
                    new ArgumentNode(new Token(30, 2, TokenType.Value),"20")
                ]),
                new CommandNode(new Token(35, 6, TokenType.Value),
                    "attack", [])
            )
        },
        {
            " ; teleport",
            new Expression(
                new CommandNode(new Token(3, 8, TokenType.Value),
                    "teleport",[])
            )
        },
        {
            "command $(getpos $(get_player this)) $ ( getplayerpos )",
            new Expression(
                new CommandNode(new Token(0, 7, TokenType.Value),
                    "command",
                    [
                        new CommandNode(new Token(10, 6, TokenType.Value),
                            "getpos",
                            [
                                new CommandNode(new Token(19, 10, TokenType.Value),
                                    "get_player",
                                    [
                                        new ArgumentNode(new Token(30, 4, TokenType.Value), "this")
                                    ], true)
                            ], true),
                        new CommandNode(new Token(41, 12, TokenType.Value),
                            "getplayerpos", [], true)
                    ])
            )
        }
    };

    private readonly Dictionary<string, InterpreterException> _testCasesWithErrors = new Dictionary<string, InterpreterException>
    {
        {
            "command arg1 arg2; teleport $getplayerpos",
            new InterpreterException("'(' expected", new Token(29, 12, TokenType.Value))
        },
        {
            "command $",
            new InterpreterException("'(' expected", new Token(9, 0, TokenType.EndOfLine))
        },
        {
            "command arg1, arg2",
            new InterpreterException("Unknown token", new Token(8, 5, TokenType.Unknown))
        },
        {
            "(command",
            new InterpreterException("Invalid use of '('", new Token(0, 1, TokenType.OpenParenthesis))
        },
        {
            ")command",
            new InterpreterException("Invalid use of ')'", new Token(0, 1, TokenType.CloseParenthesis))
        },
        {
            "command arg1 arg2)",
            new InterpreterException("Invalid use of ')'", new Token(17, 1, TokenType.CloseParenthesis))
        },
        {
            "command (arg1)",
            new InterpreterException("Invalid use of '('", new Token(8, 1, TokenType.OpenParenthesis))
        },
        {
            "command; teleport $(getplayerpos 1 2 $(spawn_position))) $(get_first_player)",
            new InterpreterException("Invalid use of ')'", new Token(55, 1, TokenType.CloseParenthesis))
        },
        {
            "command $(@command)",
            new InterpreterException("Unexpected token", new Token(10, 8, TokenType.Unknown))
        },
        {
            "command $(command",
            new InterpreterException("')' expected", new Token(17, 0, TokenType.EndOfLine))
        },
        {
            "@command",
            new InterpreterException("Unknown token", new Token(0, 8, TokenType.Unknown))
        }
    };

    private Parser _parser = null!;

    [SetUp]
    public void Setup()
    {
        _parser = new Parser();
    }

    [Test]
    public void ParseValidTestCases()
    {
        foreach ((string input, Expression expectedExpression) in _testCases)
        {
            Expression actual = _parser.GetExpression(input);
            Assert.That(actual, Is.EqualTo(expectedExpression));
        }
    }

    [Test]
    public void ParseInvalidTestCases()
    {
        foreach ((string input , InterpreterException expectedError) in _testCasesWithErrors)
        {
            try
            {
                InterpreterException? exception = Assert.Throws<InterpreterException>(() => _parser.GetExpression(input));
                if(exception == null)
                {
                    Assert.Fail("Exception not thrown");
                    return;
                }

                throw exception;
            }
            catch(Exception e)
            {
                if(e is InterpreterException ie)
                {
                    Assert.That(ie.Token, Is.EqualTo(expectedError.Token), $"Input: {input}, Actual position: {ie.Token.Start}, Excepted: {expectedError.Token.Start}");
                    Assert.That(ie.Message, Is.EquivalentTo(expectedError.Message));
                }
                else
                {
                    Assert.Fail($"Unhandled exception: {e}");
                }
            }
        }
    }
}