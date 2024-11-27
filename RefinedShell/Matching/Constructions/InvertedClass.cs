namespace RefinedShell.Matching
{
    internal sealed class InvertedClass : Rule
    {
        private readonly string _symbols;

        public InvertedClass(string symbols)
            : base(null)
        {
            _symbols = symbols;
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            FindStatus status = FindStatus.NotFound;
            if (context.Length + context.Start< context.Text.Length) {
                bool result = !_symbols.Contains(context.Text[context.Length + context.Start]);
                status = result ? FindStatus.Found : FindStatus.NotFound;
            }

            if (status == FindStatus.Found) {
                context.Length++;
            }

            return status == FindStatus.Found
                   || EvaluateChild(ref context, FindStatus.NotFound) == FindStatus.Found
                ? FindStatus.Found
                : FindStatus.NotFound;
        }
    }
}
