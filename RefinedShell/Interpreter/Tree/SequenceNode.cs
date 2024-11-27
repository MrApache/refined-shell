namespace RefinedShell.Interpreter
{
    internal class SequenceNode : Node
    {
        public CommandNode First => _first;
        public Node Second => _second;

        private readonly CommandNode _first;
        private readonly Node _second;

        public SequenceNode(CommandNode first, Node second)
        {
            _first = first;
            _second = second;
        }

        public override bool Equals(Node? other)
        {
            return other is SequenceNode np &&
                   _first.Equals(np._first)
                   && _second.Equals(np._second);
        }
    }
}