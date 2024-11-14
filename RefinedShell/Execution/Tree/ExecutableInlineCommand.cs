namespace RefinedShell.Execution
{
    internal sealed class ExecutableInlineCommand : IArgument
    {
        private readonly ExecutableCommand _command;

        public ExecutableInlineCommand(ExecutableCommand executableCommand)
        {
            _command = executableCommand;
        }

        public bool CanGetValue()
        {
            return _command.Command.IsValid();
        }

        public object? GetValue()
        {
            return _command.Execute().ReturnValue;
        }
    }
}