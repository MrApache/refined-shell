namespace RefinedShell.Matching
{
    internal sealed class Or : Rule
    { 
        private readonly Rule _left;
        private readonly Rule _right;

        public Or(Rule left, Rule right)
            : base(null)
        {
            _left = left;
            _right = right;
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            int start = context.Start;
            int length = context.Length;
            FindStatus leftResult = _left.Evaluate(ref context);
            if (leftResult != FindStatus.Found) {
                context.Start = start;
                context.Length = length;
                return _right.Evaluate(ref context);
            }
            return FindStatus.Found;
        }
    }
}
