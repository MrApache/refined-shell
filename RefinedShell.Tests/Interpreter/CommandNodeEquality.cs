using NUnit.Framework;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests
{
    [TestFixture]
    [TestOf(typeof(CommandNode))]
    internal sealed class CommandNodeEquality
    {
        private readonly CommandNode _commandNode =
            new CommandNode(new Token(0, 7, TokenType.Value), "command", new Node[]
            {
                new ArgumentNode(new Token(8, 4, TokenType.Value), "arg1"),
                new ArgumentNode(new Token(13, 4, TokenType.Value), "arg2")
            });

        [Test]
        public void Equals_OtherNode()
        {
            bool result = _commandNode.Equals(new TestNode());
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Generic_OtherNode()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            bool result = _commandNode.Equals((object)new TestNode());
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Null()
        {
            bool result = _commandNode.Equals(null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Generic_Null()
        {
            bool result = _commandNode.Equals((object)null!);
            Assert.That(result, Is.False);
        }
    }
}