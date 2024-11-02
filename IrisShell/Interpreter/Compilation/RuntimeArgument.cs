using IrisShell.Parsing;

namespace IrisShell.Interpreter
{
    internal sealed class RuntimeArgument : IArgument
    {
        private readonly ITypeParser _parser;
        private readonly IArgument[] _arguments;
        private readonly string[] _tempPool;

        public RuntimeArgument(IArgument[] arguments, ITypeParser parser)
        {
            _parser = parser;
            _arguments = arguments;
            _tempPool = new string[arguments.Length];
        }

        public bool CanGetValue()
        {
            bool result = true;
            foreach (IArgument argument in _arguments)
            {
                if (argument is CompiledInlineCommand cic)
                    result &= cic.CanGetValue();
            }

            return result;
        }

        public object GetValue()
        {
            for (int i = 0; i < _arguments.Length; i++)
            {
                _tempPool[i] = _arguments[i].GetValue()?.ToString();
            }

            return _parser.Parse(_tempPool);
        }
    }
}