using System;
using NUnit.Framework;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Expression))]
internal sealed class ExpressionEquality
{
    private readonly Expression _expression = new Expression(
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

    private readonly Expression _otherExpression = new Expression(
        new CommandNode(new Token(0, 7, TokenType.Identifier), "command",
        [
            new TestNode(),
            new TestNode(),
            new TestNode()
        ]));

    private static Node CopyArgumentNode(Node node)
    {
        return node switch
        {
            ArgumentNode an => new ArgumentNode(an.Token, an.Argument),
            CommandNode cn => CopyCommandNode(cn),
            _ => throw new Exception($"Unhandled exception: unknown node type '{node.GetType().Name}'")
        };
    }

    private static CommandNode CopyCommandNode(CommandNode commandNode)
    {
        Node[] arguments = new Node[commandNode.Arguments.Length];
        for (int i = 0; i < arguments.Length; i++)
        {
            Node argumentNode = commandNode.Arguments[i];
            arguments[i] = CopyArgumentNode(argumentNode);
        }

        return new CommandNode(commandNode.Token, commandNode.Command.ToString(), arguments,
            commandNode.Inline);
    }

    private static Expression CopyExpression(Expression source)
    {
        Expression copy = new Expression();
        foreach (CommandNode node in source)
        {
            copy.Add(CopyCommandNode(node));
        }

        return copy;
    }

    [Test]
    public void Equals_Copy()
    {
        bool result = _expression.Equals(CopyExpression(_expression));
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_Same()
    {
        bool result = _expression.Equals(_expression);
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_Null()
    {
        bool result = _expression.Equals(null);
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_Other()
    {
        bool result = _expression.Equals(_otherExpression);
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_Generic_Other()
    {
        bool result = _expression.Equals((object)_otherExpression);
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_Generic_Copy()
    {
        bool result = _expression.Equals((object)CopyExpression(_expression));
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_Generic_Null()
    {
        bool result = _expression.Equals((object)null!);
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_Generic_Same()
    {
        bool result = _expression.Equals((object)_expression);
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_Generic_NotNode()
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        bool result = _expression.Equals(new Token());
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_HashCode_Same()
    {
        // ReSharper disable once EqualExpressionComparison
        Assert.That(_expression.GetHashCode() == _expression.GetHashCode(), Is.True);
    }
}