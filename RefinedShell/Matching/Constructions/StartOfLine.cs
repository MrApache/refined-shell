namespace RefinedShell.Matching
{
    internal sealed class StartOfLine : Rule
    {
        public StartOfLine(Rule child) : base(child) { }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            return EvaluateChild(ref context) == FindStatus.Found && context.Start == 0
                ? FindStatus.Found
                : FindStatus.NotFound;
        }
    }
}
