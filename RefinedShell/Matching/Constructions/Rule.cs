namespace RefinedShell.Matching
{
    internal abstract class Rule
    {
        protected readonly Rule? Child;

        protected Rule(Rule? child)
        {
            Child = child;
        }

        public abstract FindStatus Evaluate(ref MatchContext context);

        protected FindStatus EvaluateChild(ref MatchContext context, FindStatus returnValueIfNull = FindStatus.Found)
        {
            if (Child == null)
                return returnValueIfNull;
            return Child.Evaluate(ref context);
        }
    }
}
