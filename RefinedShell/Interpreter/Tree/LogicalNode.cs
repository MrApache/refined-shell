namespace RefinedShell.Interpreter
{
    internal sealed class LogicalNode : SequenceNode
    {
        private readonly LogicalNodeType _type;

        public LogicalNodeType Type => _type;

        public LogicalNode(CommandNode first, Node second, LogicalNodeType type)
            : base(first, second)
        {
            _type = type;
        }

        public override bool Equals(Node? other)
        {
            return other is LogicalNode ln
                   && _type == ln._type
                   && base.Equals(other);
        }

        public enum LogicalNodeType
        {
            And,
            Or
        }
    }
}
