namespace RefinedShell.test
{
    public class ShellMessages
    {
        public virtual ShellMessage UnknownToken =>
            new ShellMessage("0", "Unknown token");
    }
}