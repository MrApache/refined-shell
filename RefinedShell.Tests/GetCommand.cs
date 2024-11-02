using System;
using System.Linq;
using NUnit.Framework;

namespace RefinedShell.Tests
{
    [TestFixture]
    [TestOf(typeof(Shell))]
    public sealed class GetCommand
    {
        private Shell _shell = null!;

        [SetUp]
        public void Setup()
        {
            _shell = new Shell();
            _shell.RegisterAll(this);
        }

        [Test]
        public void GetExistsCommand()
        {
            ICommand? command = _shell.GetCommand(nameof(Command1));
            Assert.That(command, Is.Not.Null);
        }

        [Test]
        public void GetNotExistsCommand()
        {
            ICommand? command = _shell.GetCommand("cmd");
            Assert.That(command, Is.Null);
        }

        [Test]
        public void GetCommandsThatStartsWith()
        {
            ICommand[] commands = _shell.GetCommands(c => c.Name.StartsWith("Command")).ToArray();
            Assert.That(commands.Length, Is.EqualTo(3));
        }

        /*
        [Test]
        public void GetAllCommandNames()
        {
            string[] names = _shell.GetCommands(c => c.Value.Span.StartsWith("")).ToArray();
            Assert.That(names.Length, Is.EqualTo(3));
        }
        */

        [Test]
        public void GetCommand_ReadOnlyMemory()
        {
            const string name = "___Command1___";
            ICommand? command = _shell.GetCommand(name.AsMemory().Slice(3, 8));
            Assert.That(command, Is.Not.Null);
        }

        [ShellCommand]
        private static void Command1() { }
        [ShellCommand]
        private static void Command2() { }
        [ShellCommand]
        private static void Command3() { }
    }
}