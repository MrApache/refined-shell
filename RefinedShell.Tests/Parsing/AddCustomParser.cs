using System;
using System.Numerics;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing;

[TestFixture]
[TestOf(typeof(TypeParsers))]
internal sealed class AddCustomParser
{
    [Test]
    public void Add()
    {
        Assert.That(TypeParsers.Contains(typeof(Vector2)), Is.False);
        TypeParsers.AddParser<Vector2>(new Vector2Parser());
        Assert.That(TypeParsers.Contains(typeof(Vector2)), Is.True);
        TypeParsers.Remove<Vector2>();
    }

    [Test]
    public void AddAlreadyRegistered_Exception()
    {
        TypeParsers.AddParser<Vector2>(new Vector2Parser());
        Assert.That(TypeParsers.Contains(typeof(Vector2)), Is.True);
        Assert.Throws<ArgumentException>(() => TypeParsers.AddParser<Vector2>(new Vector2Parser()));
        TypeParsers.Remove<Vector2>();
    }

    [Test]
    public void AddNull_Exception()
    {
        Assert.Throws<ArgumentNullException>(() => TypeParsers.AddParser<Vector2>(null!));
    }
}