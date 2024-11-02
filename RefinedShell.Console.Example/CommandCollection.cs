using RefinedShell.Utilities;

namespace RefinedShell.Example;

internal sealed class CommandCollection
{
    private readonly Shell _shell;

    public CommandCollection(Shell shell)
    {
        _shell = shell;
    }

    public void RegisterCommands()
    {
        _shell.Register(Add, "add");
        _shell.Register(Divide, "divide");
        _shell.Register(Subtract, "subtract");
        _shell.Register(Multiply, "multiply");
        _shell.Register(Help, "help");
        _shell.RegisterAll<CommandCollection>(null);
    }

    private int Add(int a, int b)
    {
        return a + b;
    }

    private int Divide(int a, int b)
    {
        return a / b;
    }

    private int Subtract(int a, int b)
    {
        return a - b;
    }

    private int Multiply(int a, int b)
    {
        return a * b;
    }

    [ShellCommand("print")]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    private string Help()
    {
        string result = string.Empty;
        IEnumerable<string> commands = _shell.GetCommands(c => (StringToken)c != null!);
        result = commands.Aggregate(result, (current, command) => current + (command + '\n'));
        return result;
    }
}