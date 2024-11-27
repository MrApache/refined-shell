namespace RefinedShell.Commands
{
    public sealed class Command
    {
        public readonly ICommand Instance;
        public readonly IArgumentProvider? ArgumentProvider;

        internal Command(ICommand instance, IArgumentProvider? provider)
        {
            Instance = instance;
            ArgumentProvider = provider;
        }
    }
}