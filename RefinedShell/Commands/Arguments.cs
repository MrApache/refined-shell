using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RefinedShell.Commands
{
    public sealed class Arguments : IReadOnlyList<Argument>
    {
        internal readonly int MinArgumentsCount;
        internal readonly int MaxArgumentCount;

        private readonly Argument[] _arguments;

        public Argument this[int index] => _arguments[index];
        public int Count => _arguments.Length;

        public Arguments(Argument[] arguments)
        {
            _arguments = arguments;
        }

        public IEnumerator<Argument> GetEnumerator()
        {
            return _arguments.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}