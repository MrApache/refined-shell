namespace IrisShell.Interpreter
{
    internal sealed class CompiledInlineCommand : IArgument
    {
        private readonly CompiledCommand _command;

        public CompiledInlineCommand(CompiledCommand compiledCommand)
        {
            _command = compiledCommand;
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