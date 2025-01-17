using RefinedShell.Commands;

namespace RefinedShell.Example;

internal sealed class CommandCollection
{
    private readonly Shell _shell;

    [ShellFunction("Health")]
    public static int Health { get; set; }

    [ShellFunction("speed")]
    public static float Speed => _damage;

    [ShellFunction("damage")]
    public static float Damage
    {
        set => _damage = value;
    }

    private static float _damage;

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
        _shell.RegisterAllWithAttribute(this);
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

    [ShellFunction("print")]
    private static void Print(string message)
    {
        Console.WriteLine(message);
    }

    private string Help()
    {
        string result = string.Empty;
        IEnumerable<string> commands = _shell.GetCommands(_ => true).Select(c => c.Name);
        result = commands.Aggregate(result, (current, command) => current + (command + '\n'));
        return result;
    }

    [ShellFunction("rm_cmd")]
    private bool RemoveCommand(string name)
    {
        return _shell.Unregister(name);
    }

    [ShellFunction("dispose")]
    private bool DisposeCommand(string name)
    {
        ICommand? command = _shell.GetCommand(name);
        if (command == null)
            return false;
        command.Dispose();
        return true;
    }

    [ShellFunction("null")]
    private string? GetNullString()
    {
        return null;
    }

    [ShellFunction("attrb")]
    private string GetAttrb(string name, int value = 0)
    {
        return $"{name}: {value}";
    }
}