using System;
using System.Numerics;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests.Parsing;

[TestFixture]
[TestOf(typeof(ParserLibrary))]
internal sealed class AddCustomParser
{
    private static readonly ParserLibrary _parserLibrary = ParserLibrary.Default;

    [Test]
    public void Add()
    {
        Assert.That(_parserLibrary.Contains(typeof(Vector2)), Is.False);
        _parserLibrary.AddParser<Vector2>(new Vector2Parser());
        Assert.That(_parserLibrary.Contains(typeof(Vector2)), Is.True);
        _parserLibrary.Remove<Vector2>();
    }

    [Test]
    public void AddAlreadyRegistered_Exception()
    {
        _parserLibrary.AddParser<Vector2>(new Vector2Parser());
        Assert.That(_parserLibrary.Contains(typeof(Vector2)), Is.True);
        Assert.Throws<ArgumentException>(() => _parserLibrary.AddParser<Vector2>(new Vector2Parser()));
        _parserLibrary.Remove<Vector2>();
    }

    [Test]
    public void AddNull_Exception()
    {
        Assert.Throws<ArgumentNullException>(() => _parserLibrary.AddParser<Vector2>(null!));
    }
}