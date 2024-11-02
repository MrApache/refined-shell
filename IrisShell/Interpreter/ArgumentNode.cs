using System;

namespace IrisShell.Interpreter
{
    internal sealed class ArgumentNode : Node
    {
        private readonly Token _token;
        private readonly string _argument;
        public string Argument => _argument;
        public Token Token => _token;

        public ArgumentNode(Token token, string argument)
        {
            _argument = argument;
            _token = token;
        }

        public override bool Equals(Node? other)
        {
            if (other is ArgumentNode an)
                return _argument.Equals(an._argument);
            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Node node)
                return Equals(node);
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_argument, _token);
        }
    }
}