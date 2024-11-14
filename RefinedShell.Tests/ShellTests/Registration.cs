using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RefinedShell.Parsing;

namespace RefinedShell.Tests;

[TestFixture]
[TestOf(typeof(Shell))]
internal sealed class Registration
{
    private readonly Shell _shell;

    public Registration()
    {
        _shell = new Shell();
    }

    [Test]
    public void RegisterAllWithAttribute_StaticOnly()
    {
        _shell.RegisterAllWithAttribute<MethodsWithAttributes>(null);
        Assert.That(_shell.Count, Is.EqualTo(4));
        _shell.UnregisterAllWithAttribute<MethodsWithAttributes>(null);
    }

    [Test]
    public void RegisterAllWithAttribute_Instance_Static()
    {
        MethodsWithAttributes instance = new MethodsWithAttributes();
        _shell.RegisterAllWithAttribute(instance);
        Assert.That(_shell.Count, Is.EqualTo(6));
        _shell.UnregisterAllWithAttribute(instance);
    }

    [Test]
    public void RegisterDelegate_StaticOnly()
    {
        _shell.Register(MethodsWithAttributes.Command1, "command1");
        _shell.Register(MethodsWithAttributes.Command2, "command2");
        Assert.That(_shell.Count, Is.EqualTo(2));
        _shell.UnregisterAll();
    }

    [Test]
    public void RegisterDelegate_Instance_Static()
    {
        MethodsWithAttributes instance = new MethodsWithAttributes();
        _shell.Register(MethodsWithAttributes.Command1, "command1");
        _shell.Register(MethodsWithAttributes.Command2, "command2");
        _shell.Register(instance.Command3, "command3");
        Assert.That(_shell.Count, Is.EqualTo(3));
        _shell.UnregisterAll();
    }

    [Test]
    public void RegisterDelegateWithEmptyName()
    {
        _shell.Register(MethodsWithAttributes.Command1, string.Empty);
        Assert.That(_shell.Count, Is.EqualTo(0));
    }

    [Test]
    public void UnregisterCommandWithEmptyName()
    {
        _shell.Register(MethodsWithAttributes.Command1, "command1");
        bool result = _shell.Unregister(string.Empty);
        Assert.That(_shell.Count, Is.EqualTo(1));
        Assert.That(result, Is.False);
        _shell.Unregister("command1");
    }

    [Test]
    public void RegisterMember_StaticOnly()
    {
        _shell.RegisterMember<MethodsWithAttributes>("Name", "playerName", null);
        _shell.RegisterMember<MethodsWithAttributes>("Damage", "playerDamage", null);
        _shell.RegisterMember<MethodsWithAttributes>("Command1", "command1", null);
        Assert.That(_shell.Count, Is.EqualTo(3));
        _shell.UnregisterAll();
    }

    [Test]
    public void RegisterMember_InstanceStatic()
    {
        MethodsWithAttributes methods = new MethodsWithAttributes();
        _shell.RegisterMember("Health", "health", methods);
        _shell.RegisterMember("Command3", "command3", methods);
        Assert.That(_shell.Count, Is.EqualTo(2));
        _shell.UnregisterAll();
    }

    [Test]
    public void RegisterMemberWithIncorrectName_Exception()
    {
        Assert.Throws<Exception>(() => _shell.RegisterMember<MethodsWithAttributes>("PlayerName", "playerName", null));
    }

    [Test]
    public void RegisterWithCustomTypeWithoutParser_Exception()
    {
        Assert.Throws<Exception>(() => _shell.Register(MethodsWithAttributes.Command4_Exception, "command4"));
    }

    [Test]
    public void RegisterWithCustomTypeWithParser()
    {
        ParserLibrary.Default.AddParser<CustomType>(new CustomTypeParser());
        _shell.Register(MethodsWithAttributes.Command5, "command5");
        ParserLibrary.Default.Remove<CustomType>();
        _shell.UnregisterAll();
    }

    [Test]
    public void RegisterAlreadyAddedCommand_Exception()
    {
        MethodsWithAttributes methods = new MethodsWithAttributes();
        _shell.RegisterMember("Health", "health", methods);
        Assert.Throws<ArgumentException>(() => _shell.RegisterMember("Name", "health", methods));
        _shell.UnregisterAll();
    }

    private sealed class MethodsWithAttributes
    {
        [ShellCommand]
        public static string Name { get; set; } = "Value";

        [ShellCommand]
        public float Health { get; }

        [ShellCommand]
        public static int Damage { get; }

        [ShellCommand]
        public static void Command1() {}

        [ShellCommand("cmd2")]
        public static void Command2(int value1, bool value2) { }

        [ShellCommand("instance")]
        public void Command3() {}

        public static void Command4_Exception(MethodsWithAttributes value) {}

        public static void Command5(CustomType type) {}
    }

    private readonly struct CustomType;
    
    private sealed class CustomTypeParser : ITypeParser
    {
        public IEnumerator<ArgumentInfo> GetArgumentInfo() => Enumerable.Empty<ArgumentInfo>().GetEnumerator();
        public bool CanParse(ReadOnlySpan<string?> input) => true;
        public object Parse(ReadOnlySpan<string?> input) => new CustomType();
    }
}