using RefinedShell;
using RefinedShell.Example;
using RefinedShell.Execution;

Shell shell = new Shell();
CommandCollection commandCollection = new CommandCollection(shell);
commandCollection.RegisterCommands();
Console.WriteLine($"Available commands: {shell.Count}");
shell.Register(Console.Clear, "clear");
shell.CreateAlias("ch", "clear; help");

while(true)
{
    string? input = Console.ReadLine();
    if(input == null)
    {
        continue;
    }
    ExecutionResult result = shell.Execute(input);
    Console.WriteLine($"[{result.Success}]->{result.ReturnValue}");
}