using RefinedShell.Parsing;

namespace RefinedShell.Execution
{
    internal sealed class RuntimeArgument : IArgument
    {
        private readonly ITypeParser _parser;
        private readonly IArgument[] _arguments;
        private readonly string?[] _tempPool;

        public RuntimeArgument(IArgument[] arguments, ITypeParser parser)
        {
            _parser = parser;
            _arguments = arguments;
            _tempPool = new string[arguments.Length];
        }

        public bool CanGetValue()
        {
            bool canGetValue = true;
            foreach (IArgument argument in _arguments)
            {
                if (argument is ExecutableInlineCommand cic)
                    canGetValue &= cic.CanGetValue();
            }

            if (!canGetValue)
                return false;

            for (int i = 0; i < _arguments.Length; i++)
            {
                object? value = _arguments[i].GetValue();
                _tempPool[i] = value?.ToString();
            }

            return _parser.CanParse(_tempPool);
        }

        public object GetValue()
        {
            return _parser.Parse(_tempPool);
        }
    }
}