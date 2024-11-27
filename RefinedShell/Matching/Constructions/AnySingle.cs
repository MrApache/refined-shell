namespace RefinedShell.Matching
{
    internal sealed class AnySingle : Rule
    {
        public AnySingle(Rule? child) : base(child)
        {
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            int start = context.Length;
            if (context.Text[start] != '\n') {
                context.Length++;
                return FindStatus.Found;
            }

            return FindStatus.NotFound;
        }
    }
}