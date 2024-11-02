namespace RefinedShell.Execution
{
    internal sealed class UnparsedArgument : IArgument
    {
        private readonly string _argument;

        public UnparsedArgument(string arg)
        {
            _argument = arg;
        }

        public bool CanGetValue() => true;

        public object GetValue() => _argument;
    }
}