namespace RefinedShell.Execution
{
    internal sealed class ParsedArgument : IArgument
    {
        private readonly object _value;

        public ParsedArgument(object value)
        {
            _value = value;
        }

        public bool CanGetValue() => true;
        public object GetValue() => _value;
    }
}