using System.Collections.Generic;

namespace RefinedShell.Matching
{
    internal sealed class Group : Rule
    {
        private readonly List<Rule> _rules;

        public Group(List<Rule> rules) : base(null)
        {
            _rules = rules;
        }

        public override FindStatus Evaluate(ref MatchContext context)
        {
            foreach(Rule rule in _rules) {
                FindStatus status = rule.Evaluate(ref context);
                if (status == FindStatus.NotFound)
                    return FindStatus.NotFound;
            }

            return FindStatus.Found;
        }
    }
}
