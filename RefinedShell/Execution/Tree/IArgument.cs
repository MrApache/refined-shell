namespace RefinedShell.Execution
{
    internal interface IArgument
    {
        public bool CanGetValue();
        public object? GetValue();
    }
}