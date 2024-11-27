namespace RefinedShell.Matching
{
    internal sealed class ExactlyOf : Rule
    {
        private readonly int _count;

        public ExactlyOf(int count, Rule child) : base(child)
        {
            _count = count;
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            int i = -1;
            FindStatus status = FindStatus.Found;
            while (status == FindStatus.Found) {
                i++;
                status = EvaluateChild(ref context, FindStatus.NotFound);
            }
            return i == _count ? FindStatus.Found : FindStatus.NotFound;
        }
    }
}
