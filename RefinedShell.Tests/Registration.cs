using System;
using NUnit.Framework;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Shell))]
internal sealed class Registration
{
    private Shell _shell = null!;

    [SetUp]
    public void Setup()
    {
        _shell = new Shell();
    }

#if NET8_0_OR_GREATER
    [Test]
    public void DelegateRegistrationDOTNET8()
    {
        _shell.Register(DelegateCommand, "cmd_1");
        Assert.That(_shell.GetCommand("cmd_1"), Is.Not.Null);
    }
#endif

    [Test]
    public void DelegateRegistrationDOTNETStandard()
    {
        _shell.Register((Action)DelegateCommand, "cmd_2");
        Assert.That(_shell.GetCommand("cmd_2"), Is.Not.Null);
    }

    [Test]
    public void DelegateRegistrationWithAttribute()
    {
        _shell.RegisterAll(this);
        Assert.That(_shell.GetCommand("attrb"), Is.Not.Null);
    }

    [Test]
    public void DelegateStaticRegistrationWithAttribute()
    {
        _shell.RegisterAll<Registration>(null);
        Assert.That(_shell.GetCommand("st_attrb"), Is.Not.Null);
        _shell.UnregisterAll("st_attrb");
        _shell.UnregisterAll(nameof(Command_Command));
    }

    [Test]
    public void DelegateRegistrationWithReadOnlyMemoryAsName()
    {
        _shell.Register((Action)DelegateCommand, "cmd_3".AsMemory());
        Assert.That(_shell.GetCommand("cmd_3"), Is.Not.Null);
    }

    [Test]
    public void RegisterDuplicate_Exception()
    {
        _shell.Register((Action)DelegateCommand, "cmd_10");
        Assert.Throws<ArgumentException>(() => _shell.Register((Action)DelegateCommand, "cmd_10"));
    }

    [Test]
    public void RegisterNull_Exception()
    {
        Assert.Throws<NullReferenceException>(() => _shell.Register(null!, "null_cmd"));
    }

    [Test]
    public void RegisterAttributeMarkedStaticMethodWithoutName()
    {
        _shell.RegisterAll<Registration>(null);
        Assert.That(_shell.GetCommand(nameof(Command_Command)), Is.Not.Null);
        _shell.UnregisterAll("st_attrb");
        _shell.UnregisterAll(nameof(Command_Command));
    }

    private void DelegateCommand() { }
    [ShellCommand("attrb")]
    private void AttributeMarkedCommand() {}
    [ShellCommand("st_attrb")]
    private static void AttributeMarkedStaticCommand() { }
    [ShellCommand]
    private static void Command_Command() { }
}