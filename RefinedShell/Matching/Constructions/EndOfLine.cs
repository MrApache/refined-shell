namespace RefinedShell.Matching
{
    internal sealed class EndOfLine : Rule
    {
        public EndOfLine(Rule child) : base(child)
        {
        }
        
        public override FindStatus Evaluate(ref MatchContext context)
        {
            return EvaluateChild(ref context) == FindStatus.Found && context.Start + context.Length == context.Text.Length
                ? FindStatus.Found
                : FindStatus.NotFound;
        }
    }
}