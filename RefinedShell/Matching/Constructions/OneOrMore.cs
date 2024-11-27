namespace RefinedShell.Matching
{
    internal sealed class OneOrMore : Rule
    {
        public OneOrMore(Rule? child) : base(child)
        {
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            int previousStart = context.Start;

            int count = 0;
            int start = context.Start;
            while (context.Start < context.Text.Length) {
                FindStatus status = EvaluateChild(ref context, FindStatus.NotFound);
                if (status == FindStatus.NotFound) {
                    if (count == 0) {
                        start = ++context.Start;
                        continue;
                    }
                    break;
                }
                count++;
                context.Start = start + count;
                context.Length = 0;
            }

            if (count > 0) {
                context.Start = start;
                context.Length = count;
                return FindStatus.Found;
            }

            context.Start = previousStart;
            context.Length = 0;
            return FindStatus.NotFound;
        }
    }
}