namespace RefinedShell.Matching
{
    internal sealed class ZeroOrMore : Rule
    {
        public ZeroOrMore(Rule? child) : base(child)
        {}

        public override FindStatus Evaluate(ref MatchContext context)
        {
            FindStatus status = FindStatus.Found;
            while (status == FindStatus.Found && context.Length + context.Start < context.Text.Length) {
                status = EvaluateChild(ref context, FindStatus.NotFound);
            }
            return FindStatus.Found;
        }
    }
}
