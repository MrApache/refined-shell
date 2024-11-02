using System;
using NUnit.Framework;

namespace IrisShell.Tests
{
    [TestFixture]
    [TestOf(typeof(Shell))]
    public sealed class Misc
    {
        private Shell _shell = null!;

        [SetUp]
        public void Setup()
        {
            _shell = new Shell();
            _shell.Register((Action)DelegateCommand, "cmd_1");
            _shell.RegisterAll(this);
        }

        [Test]
        public void CreateInstance()
        {
            /*
            Shell shell = new Shell(16);
            Assert.That(shell.BufferSize, Is.EqualTo(16));
        */
        }

        [Test]
        public void ShellCountTest()
        {
            Assert.That(_shell.Count, Is.EqualTo(7));
        }

        /*
        [Test]
        public void SetLogger()
        {
            _logger.Level = LogLevel.None;
            _shell.SetLogger(_logger);
            _shell.Execute("logger_test_command");
            Assert.That(_logger.Level, Is.EqualTo(LogLevel.Error));
        }
        */

        /*
        [Test]
        public void SetNullLogger()
        {
            _logger.Level = LogLevel.Critical;
            _shell.SetLogger(null);
            _shell.Execute("logger_test_command");
            Assert.That(_logger.Level, Is.EqualTo(LogLevel.Critical));
        }
        */

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
}