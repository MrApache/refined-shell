namespace RefinedShell.Matching
{
    internal sealed class SymbolMatching : Rule
    {
        private readonly string _symbols;

        public SymbolMatching(string symbols) : base(null)
        {
            _symbols = symbols;
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            int left = context.Text.Length - context.Start;
            if (left < _symbols.Length)
                return FindStatus.NotFound;

            bool found = false;
            for (; context.Start < context.Text.Length; context.Start++) {
                context.Length = 0;
                if (FindMatch(ref context)) {
                    found = true;
                    break;
                }
            }

            return found ? FindStatus.Found : FindStatus.NotFound;
        }

        private bool FindMatch(ref MatchContext context)
        {
            for (int j = 0; j < _symbols.Length; j++) {
                if (_symbols[j] != context.CurrentChar)
                    return false;
                context.Length++;
            }

            return true;
        }
    }
}