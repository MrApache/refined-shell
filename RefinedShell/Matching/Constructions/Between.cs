namespace RefinedShell.Matching
{
    internal sealed class Between : Rule
    {
        private readonly int _min;
        private readonly int _max;

        public Between(int min, int max, Rule child) : base(child)
        {
            _min = min;
            _max = max;
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            int i = 0;
            int maxCount = _max == -1 ? context.Text.Length : _max;
            for(; i < maxCount; i++) {
                FindStatus status = EvaluateChild(ref context, FindStatus.NotFound);
                if(status == FindStatus.NotFound) {
                   break; 
                }
            }
            return i >= _min ? FindStatus.Found : FindStatus.NotFound;
        }
    }
}
