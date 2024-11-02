using NUnit.Framework;
using RefinedShell.Interpreter;

namespace RefinedShell.Tests
{
    [TestFixture]
    [TestOf(typeof(Token))]
    internal sealed class TokenEquality
    {
        private readonly Token _token = new Token(0, 10, TokenType.Value);

        [Test]
        public void Equals_Copy()
        {
            bool result = _token.Equals(_token);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_Null()
        {
            bool result = _token.Equals(null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Other()
        {
            bool result = _token.Equals(new Token(10, 0, TokenType.EndOfLine));
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Generic_Other()
        {
            bool result = _token.Equals((object)new Token(1, 1, TokenType.Semicolon));
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Generic_Copy()
        {
            bool result = _token.Equals((object)_token);
            Assert.That(result, Is.True);
        }
    }
}