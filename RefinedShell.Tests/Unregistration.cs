using System;
using NUnit.Framework;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Shell))]
internal sealed class Unregistration
{
    private Shell _shell = null!;

    [SetUp]
    public void Setup()
    {
        _shell = new Shell();
        _shell.Register((Action)DelegateCommand, "cmd_1");
        _shell.Register((Action)DelegateCommand, "cmd_3".AsMemory());
        _shell.RegisterAll(this);
    }

    [Test]
    public void Unregister()
    {
        _shell.Unregister("cmd_1");
        Assert.That(_shell.GetCommand("cmd_1"), Is.Null);
    }

    [Test]
    public void UnregisterAttributeMarkedMethod()
    {
        _shell.Unregister("attrb");
        Assert.That(_shell.GetCommand("attrb"), Is.Null);
    }

    [Test]
    public void UnregisterAttributeMarkedStaticMethod()
    {
        _shell.Unregister("st_attrb");
        Assert.That(_shell.GetCommand("st_attrb"), Is.Null);
    }

    [Test]
    public void UnregisterAllAttributeMarkedStaticMethods()
    {
        _shell.UnregisterAll<Unregistration>(null);
        Assert.That(_shell.GetCommand("st_attrb1"), Is.Null);
        Assert.That(_shell.GetCommand("st_attrb2"), Is.Null);
    }

    [Test]
    public void UnregisterAllAttributeMarkedMethods()
    {
        _shell.UnregisterAll(this);
        Assert.That(_shell.GetCommand("attrb1"), Is.Null);
        Assert.That(_shell.GetCommand("attrb2"), Is.Null);
    }

    [Test]
    public void UnregisterCommandThatNotExists_Nothing()
    {
        int count = _shell.Count;
        _shell.Unregister("command does not exist");
        Assert.That(count, Is.EqualTo(_shell.Count));
    }

    private void DelegateCommand() { }

    [ShellCommand("attrb")]
    private void AttributeMarkedCommand() { }

    [ShellCommand("attrb1")]
    private void AttributeMarkedCommand1() { }

    [ShellCommand("attrb2")]
    private void AttributeMarkedCommand2() { }

    [ShellCommand("st_attrb")]
    private static void AttributeMarkedStaticCommand() { }

    [ShellCommand("st_attrb1")]
    private static void AttributeMarkedStaticCommand1() { }

    [ShellCommand("st_attrb2")]
    private static void AttributeMarkedStaticCommand2() { }
}