using System;
using NUnit.Framework;

namespace RefinedShell.Tests
{
    [TestFixture]
    [TestOf(typeof(Shell))]
    internal sealed class Aliases
    {
        private readonly Shell _shell;

        public Aliases()
        {
            _shell = new Shell();
            _shell.RegisterAll<Aliases>(null);
            _shell.CreateAlias("phw","print hello_world");
        }

        [ShellCommand("print")]
        private static void Print(string message)
        {
            Console.WriteLine(message);
        }

        [ShellCommand("print_err")]
        private static void PrintError(string message)
        {
            Console.WriteLine($"[Error] {message}");
        }

        [Test]
        public void Execute_Success()
        {
            bool result = _shell.Execute("phw").Success;
            Assert.That(result, Is.True);
        }

        [Test]
        public void Execute_Fail()
        {
            bool result = _shell.Execute("pwd").Success;
            Assert.That(result, Is.False);
        }

        [Test]
        public void Execute_Redefine()
        {
            _shell.CreateAlias("phw", "print_err file_not_found");
            bool result = _shell.Execute("phw").Success;
            Assert.That(result, Is.True);
            _shell.CreateAlias("phw","print hello_world");
        }
    }
}