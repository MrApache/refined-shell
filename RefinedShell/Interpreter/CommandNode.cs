using System;
using System.Linq;
using RefinedShell.Utilities;

namespace RefinedShell.Interpreter
{
    internal sealed class CommandNode : Node
    {
        private readonly Token _token;
        private readonly StringToken _command;
        private readonly Node[] _arguments;
        private readonly bool _inline;

        public Token Token => _token;
        public StringToken Command => _command;
        public Node[] Arguments => _arguments;
        public bool Inline => _inline;

        public CommandNode(Token token, string command, Node[] arguments, bool inline = false)
        {
            _token = token;
            _command = command;
            _arguments = arguments;
            _inline = inline;
        }

        public override bool Equals(Node? other)
        {
            if (other is CommandNode cn)
                return _command.Equals(cn._command)
                       && _arguments.SequenceEqual(cn._arguments)
                       && _inline == cn._inline
                       && _token.Equals(cn._token);
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
            return HashCode.Combine(_token, _command, _inline, _arguments);
        }
    }
}