namespace RefinedShell.test
{
    public readonly struct ShellMessage
    {
        public readonly string Code;
        public readonly string Message;

        public ShellMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}