using System;
using System.Reflection;
using NUnit.Framework;
using RefinedShell.Execution;

namespace RefinedShell.Tests
{
    [TestFixture]
    [TestOf(typeof(Shell))]
    [TestOf(typeof(IExecutor))]
    [TestOf(typeof(SafeExecutor))]
    [TestOf(typeof(UnsafeExecutor))]
    internal sealed class ShellExecutor
    {
        private readonly Shell _unsafe;
        private readonly Shell _safe;

        public ShellExecutor()
        {
            _unsafe = new Shell(false);
            _safe = new Shell();
            _unsafe.RegisterAll<ShellExecutor>(null);
            _safe.RegisterAll<ShellExecutor>(null);
        }

        [ShellCommand("getPlayer")]
        private static void GetPlayer(bool throwException)
        {
            if (throwException)
                throw new ArgumentException();
        }

        [Test]
        public void Execute_Unsafe()
        {
            bool result = _unsafe.Execute("getPlayer false").Success;
            Assert.That(result, Is.True);
        }

        [Test]
        public void Execute_Unsafe_Exception()
        {
            Assert.Throws<TargetInvocationException>(() => _unsafe.Execute("getPlayer true"));
        }

        [Test]
        public void Execute_Safe()
        {
            _safe.Execute("getPlayer false");
        }

        [Test]
        public void Execute_Safe_Exception()
        {
            ExecutionResult result = _safe.Execute("getPlayer true");
            Assert.That(result.Success, Is.False);
            Assert.That(result.ReturnValue, Is.TypeOf<ArgumentException>());
        }
    }
}