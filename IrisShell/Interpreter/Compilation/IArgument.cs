namespace IrisShell.Interpreter
{
    internal interface IArgument
    {
        public bool CanGetValue();
        public object? GetValue();
    }
}