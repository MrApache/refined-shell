namespace RefinedShell.Matching
{
    internal sealed class ZeroOrOne : Rule
    {
        public ZeroOrOne(Rule? child) : base(child)
        {
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            EvaluateChild(ref context, FindStatus.NotFound);
            return FindStatus.Found;
        }
    }
}