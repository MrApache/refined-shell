using NUnit.Framework;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests
{
    [TestFixture]
    [TestOf(typeof(CommandNode))]
    internal sealed class ArgumentNodeEquality
    {
        private readonly ArgumentNode _argumentNode =
            new ArgumentNode(new Token(8, 4, TokenType.Value), "arg1");

        [Test]
        public void Equals_OtherNode()
        {
            bool result = _argumentNode.Equals(new TestNode());
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Generic_OtherNode()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            bool result = _argumentNode.Equals((object)new TestNode());
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Null()
        {
            bool result = _argumentNode.Equals(null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Generic_Null()
        {
            bool result = _argumentNode.Equals((object)null!);
            Assert.That(result, Is.False);
        }
    }
}